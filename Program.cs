using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using KartRider.IO.Packet;

#region Windows API

public static class Kernel32
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool QueryFullProcessMemoryInfo(IntPtr hProcess, out IntPtr pBuffer);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetProcessWorkingSetSize(IntPtr hProcess, out IntPtr lpMinimumWorkingSetSize, out IntPtr lpMaximumWorkingSetSize);

    [DllImport("kernel32.dll")]
    public static extern void CloseHandle(IntPtr hObject);

    public const uint PROCESS_QUERY_INFORMATION = 0x0400;
    public const uint PROCESS_VM_READ = 0x0010;
}

[StructLayout(LayoutKind.Sequential)]
public struct MEMORY_BASIC_INFORMATION
{
    public IntPtr BaseAddress;
    public IntPtr AllocationBase;
    public uint AllocationProtect;
    public IntPtr RegionSize;
    public uint State;
    public uint Protect;
    public uint Type;
}

public static class MemoryScanner
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

    // 搜索目标：两个四字节 uint 值 (小端序)
    // 1006110391 = 0x3BF806B7 → 字节序 B7 06 F8 3B
    // 349832159  = 0x14DA03DF → 字节序 DF 03 DA 14
    // 匹配后的偏移规则：
    //   1006110391 → 向高地址方向跳过 8 字节开始读取
    //   349832159  → 向高地址方向跳过 1 字节开始读取
    private static readonly uint[] SearchValues = new uint[] { 1006110391, 349832159 };

    public static byte[]? ScanProcessMemory(Process process, int outputBytes, Func<byte[], bool> validateFirstValue)
    {
        IntPtr hProcess = Kernel32.OpenProcess(Kernel32.PROCESS_QUERY_INFORMATION | Kernel32.PROCESS_VM_READ, false, process.Id);
        if (hProcess == IntPtr.Zero)
        {
            Console.WriteLine($"无法打开进程，错误代码: {Marshal.GetLastWin32Error()}");
            return null;
        }

        try
        {
            // 全地址循环扫描，直到找到有效数据
            while (true)
            {
                // 进程退出时返回 null，让外层重新等待新进程
                if (process.HasExited)
                {
                    Console.WriteLine("进程已退出，停止当前扫描。");
                    return null;
                }

                IntPtr address = (IntPtr)0x10000000; // 跳过小于 10000000 的低地址
                int pageSize = 0x10000; // 64KB 扫描块

                while (true)
                {
                    MEMORY_BASIC_INFORMATION memInfo;
                    int result = VirtualQueryEx(hProcess, address, out memInfo, Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                    if (result == 0)
                        break;

                    // 只扫描已提交且可读的内存页面
                    // 排除 PAGE_NOACCESS(0x01)、PAGE_GUARD(0x100)、PAGE_NOCACHE(0x200)、PAGE_WRITECOMBINE(0x400)
                    if ((memInfo.State & 0x1000) != 0 && // MEM_COMMIT = 0x1000
                        (memInfo.Protect & 0x01) == 0 && // 排除 PAGE_NOACCESS
                        (memInfo.Protect & 0x100) == 0)  // 排除 PAGE_GUARD
                    {
                        IntPtr blockStart = memInfo.BaseAddress;
                        IntPtr blockEnd = (IntPtr)((long)blockStart + (long)memInfo.RegionSize);

                        while ((long)blockStart < (long)blockEnd)
                        {
                            int readSize = (int)Math.Min(pageSize, (long)blockEnd - (long)blockStart);
                            byte[] buffer = new byte[readSize];
                            int bytesRead;

                            bool success = Kernel32.ReadProcessMemory(hProcess, blockStart, buffer, readSize, out bytesRead);
                            if (success && bytesRead > 0)
                            {
                                // 在这个块中搜索两个 uint 值
                                for (int i = 0; i <= bytesRead - 4; i++)
                                {
                                    uint value = BitConverter.ToUInt32(buffer, i);

                                    if (value == 1006110391 || value == 349832159)
                                    {
                                        // 根据匹配到的值决定偏移量
                                        int skipBytes = (value == 1006110391) ? 12 : 5;

                                        IntPtr matchAddress = (IntPtr)((long)blockStart + i);
                                        IntPtr readStartAddress = (IntPtr)((long)matchAddress + skipBytes);
                                        byte[] resultBytes = new byte[outputBytes];
                                        int bytesReadOutput;

                                        if (Kernel32.ReadProcessMemory(hProcess, readStartAddress, resultBytes, outputBytes, out bytesReadOutput) && bytesReadOutput == outputBytes)
                                        {
                                            // 用 firstValue 验证数据是否有效
                                            if (validateFirstValue(resultBytes))
                                            {
                                                return resultBytes;
                                            }
                                        }
                                    }
                                }
                            }

                            blockStart = (IntPtr)((long)blockStart + (long)bytesRead);
                            if (bytesRead == 0)
                                break;
                        }
                    }

                    // 移动到下一个内存区域
                    address = (IntPtr)((long)memInfo.BaseAddress + (long)memInfo.RegionSize);
                }
            }
        }
        finally
        {
            Kernel32.CloseHandle(hProcess);
        }
    }
}

#endregion

class BinaryFileSplitter
{
    public static int updateCount = 0;

    /// <summary>由 kartspec.json 动态定义的所有 kart 属性值</summary>
    public static readonly Dictionary<string, object> KartProperties = new();

    static void Main()
    {
        Console.WriteLine("=== KartRider Memory Scanner ===");
        ScanKartRiderMemory();
    }

    static void ScanKartRiderMemory()
    {
        Console.WriteLine("开始循环扫描 KartRider.exe 进程...");
        Console.WriteLine("等待进程启动...");
        Console.WriteLine("按 ESC 键退出");
        Console.WriteLine();

        byte[]? lastData = null;
        int scanCount = 0;
        bool processFound = false;
        int outputBytes = CalculateKartSpecLength();

        while (true)
        {
            // 检查是否按下了 ESC 键
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n已停止扫描");
                    break;
                }
            }

            scanCount++;

            // 查找进程
            Process[] processes = Process.GetProcessesByName("KartRider");
            
            if (processes.Length == 0)
            {
                processes = Process.GetProcessesByName("KartRiderdx11");
            }

            if (processes.Length > 0)
            {
                if (!processFound)
                {
                    processFound = true;
                    Console.WriteLine($"进程已找到: {processes[0].ProcessName} (PID: {processes[0].Id})");
                }

                Process kartProcess = processes[0];

                // 检查进程是否还在运行
                if (kartProcess.HasExited)
                {
                    Console.WriteLine("\n进程已退出，等待重新启动...");
                    processFound = false;
                    lastData = null;
                    Thread.Sleep(500);
                    continue;
                }

                byte[]? result = MemoryScanner.ScanProcessMemory(kartProcess, outputBytes, ValidateFirstValue);

                if (result != null)
                {
                    bool isNewData = lastData == null || !result.SequenceEqual(lastData);

                    if (isNewData)
                    {
                        // 直接将数据作为 iPacket 解析
                        InPacket iPacket = new InPacket(result);
                        ParseKartSpec(iPacket);

                        lastData = result;
                    }
                }
            }
            else
            {
                if (processFound)
                {
                    Console.WriteLine("\n进程已关闭，等待重新启动...");
                    processFound = false;
                    lastData = null;
                }
            }

            // 扫描间隔 (毫秒)
            Thread.Sleep(100);
        }
        
        Console.WriteLine();
        Console.WriteLine($"总共扫描 {scanCount} 次，更新 {updateCount} 次");
    }

    static bool ValidateFirstValue(byte[] data)
    {
        try
        {
            InPacket iPacket = new InPacket(data);
            decimal firstValue = KartSpecConfigs[0].ReadFunc(iPacket);
            // draftMulAccelFactor 典型值约 1.1，合理范围 1 ~ 2
            bool valid = firstValue >= 1m && firstValue <= 2m;
            return valid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [验证异常] {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }

    static void ParseKartSpec(InPacket iPacket)
    {
        updateCount++;

        Console.WriteLine();
        Console.WriteLine($"========== 数据更新 #{updateCount} ({DateTime.Now:HH:mm:ss}) ==========");

        try
        {
            foreach (var config in KartSpecConfigs)
            {
                decimal rawValue = config.ReadFunc(iPacket);
                // decimal calculatedValue = (rawValue - config.FallbackValue) / config.Scale;
                // config.SetKartProperty(calculatedValue);
                config.SetKartProperty(rawValue);
                Console.WriteLine($"{config.AttributeName}:{rawValue}");
            }
        }
        catch (OverflowException)
        {
            return;
        }
    }

    static int CalculateKartSpecLength()
    {
        int total = 0;
        foreach (var config in KartSpecConfigs)
        {
            total += config.ValueType switch
            {
                "float" => 4,
                "int"   => 4,
                "byte"  => 1,
                _       => 4
            };
        }
        return total;
    }

    private class KartSpecJsonEntry
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "float";
    }

    private class KartSpecConfig
    {
        public string AttributeName { get; }
        public string ValueType { get; }
        public Func<InPacket, decimal> ReadFunc { get; }
        public Action<decimal> SetKartProperty { get; }

        public KartSpecConfig(
            string attributeName,
            string valueType,
            Func<InPacket, decimal> readFunc,
            Action<decimal> setKartProperty)
        {
            AttributeName = attributeName;
            ValueType = valueType;
            ReadFunc = readFunc;
            SetKartProperty = setKartProperty;
        }
    }

    private static readonly List<KartSpecConfig> KartSpecConfigs = new();

    static BinaryFileSplitter()
    {
        string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kartspec.json");
        string json = File.ReadAllText(configPath);
        var entries = JsonSerializer.Deserialize<List<KartSpecJsonEntry>>(json)
            ?? throw new InvalidOperationException("kartspec.json 解析失败或为空");

        foreach (var entry in entries)
        {
            Func<InPacket, decimal> readFunc = entry.Type switch
            {
                "float" => p => (decimal)p.ReadEncodedFloat(),
                "int"   => p => (decimal)p.ReadEncodedInt(),
                "byte"  => p => (decimal)p.ReadEncodedByte(),
                _       => throw new InvalidOperationException($"不支持的类型: {entry.Type}")
            };

            Action<decimal> setKartProperty = val =>
            {
                object boxed = entry.Type switch
                {
                    "float" => (float)val,
                    "int"   => (int)val,
                    "byte"  => (byte)val,
                    _       => (float)val
                };
                KartProperties[entry.Name] = boxed;
            };

            KartSpecConfigs.Add(new KartSpecConfig(
                entry.Name,
                entry.Type,
                readFunc,
                setKartProperty));
        }
    }
}