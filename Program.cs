using System;
using System.Xml;

namespace KartSpec;

internal class Program
{
    private const string DefaultInput = "F:\\nexon\\kart_\\2021kclV1\\param@cn.xml";

    private static void Main(string[] args)
    {
        string input = GetInputFileName(args);
        if (input.EndsWith(".xml"))
        {
            KartSpec(input);
        }
    }

    private static string GetInputFileName(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            return DefaultInput;
        }
        return args.Length >= 1 ? args[0] : string.Empty;
    }

    private static void KartSpec(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(input);

        XmlNodeList bodyParams = doc.GetElementsByTagName("BodyParam");
        if (bodyParams.Count > 0)
        {
            foreach (XmlNode xn in bodyParams)
            {
                XmlElement xe = (XmlElement)xn;
                SaveKartSpec(CreateKartSpec(xe));
            }
        }
    }

    private static string CreateKartSpec(XmlElement xe)
    {
        var attributes = new[]
        {
        ("draftMulAccelFactor", 0M, 1M, 1M),
        ("draftTick", 0M, 0M, 1M),
        ("driftBoostMulAccelFactor", 0M, 1.31M, 1M),
        ("driftBoostTick", 0M, 0M, 1M),
        ("chargeBoostBySpeed", 0M, 2M, 1M),
        ("SpeedSlotCapacity", 0M, 2M, 1M),
        ("ItemSlotCapacity", 0M, 2M, 1M),
        ("SpecialSlotCapacity", 0M, 1M, 1M),
        ("UseTransformBooster", 0M, 0M, 1M),
        ("motorcycleType", 0M, 0M, 1M),
        ("BikeRearWheel", 0M, 1M, 1M),
        ("Mass", 0M, 100M, 1M),
        ("AirFriction", 0M, 3M, 1M),
        ("DragFactor", 0.75M, 0.75M, 1M),
        ("ForwardAccelForce", 2150M, 2150M, 1M),
        ("BackwardAccelForce", 1725M, 1725M, 1M),
        ("GripBrakeForce", 2070M, 2070M, 1M),
        ("SlipBrakeForce", 1415M, 1415M, 1M),
        ("MaxSteerAngle", 0M, 10M, 1M),
        ("SteerConstraint", 22.25M, 22.25M, 1M),
        ("FrontGripFactor", 0M, 5M, 1M),
        ("RearGripFactor", 0M, 5M, 1M),
        ("DriftTriggerFactor", 0M, 0.2M, 1M),
        ("DriftTriggerTime", 0M, 0.2M, 1M),
        ("DriftSlipFactor", 0M, 0.2M, 1M),
        ("DriftEscapeForce", 2600M, 2600M, 1M),
        ("CornerDrawFactor", 0.18M, 0.18M, 1M),
        ("DriftLeanFactor", 0.07M, 0.07M, 1M),
        ("SteerLeanFactor", 0.01M, 0.01M, 1M),
        ("DriftMaxGauge", 4300M, 4300M, 1M),
        ("NormalBoosterTime", 3000M, 3000M, 1M),
        ("ItemBoosterTime", 3000M, 3000M, 1M),
        ("TeamBoosterTime", 4500M, 4500M, 1M),
        ("AnimalBoosterTime", 4000M, 4000M, 1M),
        ("SuperBoosterTime", 3500M, 3500M, 1M),
        ("TransAccelFactor", -0.0045M, 1.4955M, 1M),
        ("BoostAccelFactor", -0.006M, 1.494M, 1M),
        ("StartBoosterTimeItem", 0M, 1000M, 1M),
        ("StartBoosterTimeSpeed", 0M, 1000M, 1M),
        ("StartForwardAccelForceItem", 171.6355M, 2304M, 2102.325M),
        ("StartForwardAccelForceSpeed", 176.132M, 2304M, 2099.68M),
        ("DriftGaguePreservePercent", 0M, 0M, 1M),
        ("UseExtendedAfterBooster", 0M, 0M, 1M),
        ("BoostAccelFactorOnlyItem", 0M, 1.5M, 1M),
        ("antiCollideBalance", 0M, 1M, 1M),
        ("dualBoosterSetAuto", 0M, 0M, 1M),
        ("dualBoosterTickMin", 0M, 40M, 1M),
        ("dualBoosterTickMax", 0M, 60M, 1M),
        ("dualMulAccelFactor", 0M, 1.1M, 1M),
        ("dualTransLowSpeed", 0M, 100M, 1M),
        ("PartsEngineLock", 0M, 0M, 1M),
        ("PartsWheelLock", 0M, 0M, 1M),
        ("PartsSteeringLock", 0M, 0M, 1M),
        ("PartsBoosterLock", 0M, 0M, 1M),
        ("PartsCoatingLock", 0M, 0M, 1M),
        ("PartsTailLampLock", 0M, 0M, 1M),
        ("chargeInstAccelGaugeByBoost", 0M, 0.02M, 1M),
        ("chargeInstAccelGaugeByGrip", 0M, 0.02M, 1M),
        ("chargeInstAccelGaugeByWall", 0M, 0.2M, 1M),
        ("instAccelFactor", 0M, 1.25M, 1M),
        ("instAccelGaugeCooldownTime", 0M, 0M, 1000M),
        ("instAccelGaugeLength", 0M, 0M, 1000M),
        ("instAccelGaugeMinUsable", 0M, 0M, 1000M),
        ("instAccelGaugeMinVelBound", 0M, 200M, 1M),
        ("instAccelGaugeMinVelLoss", 0M, 50M, 1M),
        ("useExtendedAfterBoosterMore", 0M, 0M, 1M),
        ("wallCollGaugeCooldownTime", 0M, 0M, 1000M),
        ("wallCollGaugeMaxVelLoss", 0M, 200M, 1M),
        ("wallCollGaugeMinVelBound", 0M, 200M, 1M),
        ("wallCollGaugeMinVelLoss", 0M, 50M, 1M),
        ("modelMaxX", 0M, 0M, 1M),
        ("modelMaxY", 0M, 0M, 1M),
        };

        string kartSpec = "\t<id*";
        foreach (var (attributeName, fallbackValue, defaultValue, scale) in attributes)
        {
            string attrValue = GetAttributeValue(xe, attributeName, fallbackValue, defaultValue, scale);
            kartSpec += $" {attributeName}='{attrValue}'";
        }
        kartSpec += " />\n";

        return kartSpec;
    }

    private static string GetAttributeValue(XmlElement xe, string attributeName, decimal fallbackValue, decimal defaultValue, decimal scale)
    {
        if (attributeName == "UseTransformBooster" || attributeName == "motorcycleType" || attributeName == "BikeRearWheel" || attributeName == "UseExtendedAfterBooster" || attributeName == "dualBoosterSetAuto" || attributeName == "PartsEngineLock" || attributeName == "PartsWheelLock" || attributeName == "PartsSteeringLock" || attributeName == "PartsBoosterLock" || attributeName == "PartsCoatingLock" || attributeName == "PartsTailLampLock" || attributeName == "useExtendedAfterBoosterMore")
        {
            return bool.TryParse(xe.GetAttribute(attributeName), out var value) ? (value ? 1M : 0M).ToString("G29") : defaultValue.ToString("G29");
        }
        else
        {
            if (attributeName == "StartForwardAccelForceItem")
            {
                return decimal.TryParse(xe.GetAttribute("StartForwardAccelFactorItem"), out var value) ? (value * scale + fallbackValue).ToString("G29") : defaultValue.ToString("G29");
            }
            if (attributeName == "StartForwardAccelForceSpeed")
            {
                return decimal.TryParse(xe.GetAttribute("StartForwardAccelFactorSpeed"), out var value) ? (value * scale + fallbackValue).ToString("G29") : defaultValue.ToString("G29");
            }
            if (attributeName == "instAccelGaugeMinUsable")
            {
                decimal instAccelGaugeLength = decimal.TryParse(xe.GetAttribute("instAccelGaugeLength"), out var tempinstAccelGaugeLength) ? tempinstAccelGaugeLength : 0M;
                return decimal.TryParse(xe.GetAttribute(attributeName), out var value) ? (value * instAccelGaugeLength * scale + fallbackValue).ToString("G29") : defaultValue.ToString("G29");
            }
            else
            {
                return decimal.TryParse(xe.GetAttribute(attributeName), out var value) ? (value * scale + fallbackValue).ToString("G29") : defaultValue.ToString("G29");
            }
        }
    }

    private static void SaveKartSpec(string kartSpec)
    {
        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "KartSpec.xml", kartSpec);
    }
}
