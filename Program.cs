using System;
using System.Xml;

namespace KartSpec;

internal class Program
{
    private static void Main(string[] args)
    {
        string input;
        if (args == null || args.Length == 0)
        {
            input = "F:\\nexon\\kart_\\2021kclV1\\param@cn.xml";
        }
        else if (args.Length == 1)
        {
            input = args[0];
        }
        else
        {
            if (args.Length != 2)
                return;
            input = args[0];
        }
        if (input.EndsWith(".xml"))
        {
            Program.KartSpec(input);
        }
    }

    private static void KartSpec(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(input);
        if (!(doc.GetElementsByTagName("BodyParam") == null))
        {
            XmlNodeList lis = doc.GetElementsByTagName("BodyParam");
            foreach (XmlNode xn in lis)
            {
                XmlElement xe = (XmlElement)xn;
                decimal draftMulAccelFactor = decimal.TryParse(xe.GetAttribute("draftMulAccelFactor"), out var tempDraftMulAccelFactor) ? tempDraftMulAccelFactor : 1M;
                decimal draftTick = decimal.TryParse(xe.GetAttribute("draftTick"), out var tempdraftTick) ? tempdraftTick : 0M;
                decimal driftBoostMulAccelFactor = decimal.TryParse(xe.GetAttribute("driftBoostMulAccelFactor"), out var tempdriftBoostMulAccelFactor) ? tempdriftBoostMulAccelFactor : 1.31M;
                decimal driftBoostTick = decimal.TryParse(xe.GetAttribute("driftBoostTick"), out var tempdriftBoostTick) ? tempdriftBoostTick : 0M;
                decimal chargeBoostBySpeed = decimal.TryParse(xe.GetAttribute("chargeBoostBySpeed"), out var tempchargeBoostBySpeed) ? tempchargeBoostBySpeed : 2M;
                decimal SpeedSlotCapacity = decimal.TryParse(xe.GetAttribute("SpeedSlotCapacity"), out var tempSpeedSlotCapacity) ? tempSpeedSlotCapacity : 2M;
                decimal ItemSlotCapacity = decimal.TryParse(xe.GetAttribute("ItemSlotCapacity"), out var tempItemSlotCapacity) ? tempItemSlotCapacity : 1M;
                decimal SpecialSlotCapacity = decimal.TryParse(xe.GetAttribute("SpecialSlotCapacity"), out var tempSpecialSlotCapacity) ? tempSpecialSlotCapacity : 0M;
                int UseTransformBooster = (bool.TryParse(xe.GetAttribute("UseTransformBooster"), out var tempUseTransformBooster) ? tempUseTransformBooster : false)? 1 : 0;
                int motorcycleType = (bool.TryParse(xe.GetAttribute("motorcycleType"), out var tempmotorcycleType) ? tempmotorcycleType : false)? 1 : 0;
                int BikeRearWheel = (bool.TryParse(xe.GetAttribute("BikeRearWheel"), out var tempBikeRearWheel) ? tempBikeRearWheel : true)? 1 : 0;
                decimal Mass = 100M;
                decimal AirFriction = 3M;
                decimal DragFactor = decimal.TryParse(xe.GetAttribute("DragFactor"), out var tempDragFactor) ? tempDragFactor + 0.75M : 0.75M;
                decimal ForwardAccelForce = decimal.TryParse(xe.GetAttribute("ForwardAccelForce"), out var tempForwardAccelForce) ? tempForwardAccelForce + 2150M : 2150M;
                decimal BackwardAccelForce = decimal.TryParse(xe.GetAttribute("BackwardAccelForce"), out var tempBackwardAccelForce) ? tempBackwardAccelForce + 1725M : 1725M;
                decimal GripBrakeForce = decimal.TryParse(xe.GetAttribute("GripBrakeForce"), out var tempGripBrakeForce) ? tempGripBrakeForce + 2070M : 2070M;
                decimal SlipBrakeForce = decimal.TryParse(xe.GetAttribute("SlipBrakeForce"), out var tempSlipBrakeForce) ? tempSlipBrakeForce + 1415M : 1415M;
                decimal MaxSteerAngle = 10M;
                decimal SteerConstraint = decimal.TryParse(xe.GetAttribute("SteerConstraint"), out var tempSteerConstraint) ? tempSteerConstraint + 22.25M : 22.25M;
                decimal FrontGripFactor = 5M;
                decimal RearGripFactor = 5M;
                decimal DriftTriggerFactor = 0.2M;
                decimal DriftTriggerTime = 0.2M;
                decimal DriftSlipFactor = 0.2M;
                decimal DriftEscapeForce = decimal.TryParse(xe.GetAttribute("DriftEscapeForce"), out var tempDriftEscapeForce) ? tempDriftEscapeForce + 2600M : 2600M;
                decimal CornerDrawFactor = decimal.TryParse(xe.GetAttribute("CornerDrawFactor"), out var tempCornerDrawFactor) ? tempCornerDrawFactor + 0.18M : 0.18M;
                decimal DriftLeanFactor = decimal.TryParse(xe.GetAttribute("DriftLeanFactor"), out var tempDriftLeanFactor) ? tempDriftLeanFactor + 0.07M : 0.07M;
                decimal SteerLeanFactor = decimal.TryParse(xe.GetAttribute("SteerLeanFactor"), out var tempSteerLeanFactor) ? tempSteerLeanFactor + 0.01M : 0.01M;
                decimal DriftMaxGauge = decimal.TryParse(xe.GetAttribute("DriftMaxGauge"), out var tempDriftMaxGauge) ? tempDriftMaxGauge + 4300M : 4300M;
                decimal NormalBoosterTime = decimal.TryParse(xe.GetAttribute("NormalBoosterTime"), out var tempNormalBoosterTime) ? tempNormalBoosterTime + 3000M : 3000M;
                decimal ItemBoosterTime = decimal.TryParse(xe.GetAttribute("ItemBoosterTime"), out var tempItemBoosterTime) ? tempItemBoosterTime + 3000M : 3000M;
                decimal TeamBoosterTime = decimal.TryParse(xe.GetAttribute("TeamBoosterTime"), out var tempTeamBoosterTime) ? tempTeamBoosterTime + 4500M : 4500M;
                decimal AnimalBoosterTime = decimal.TryParse(xe.GetAttribute("AnimalBoosterTime"), out var tempAnimalBoosterTime) ? tempAnimalBoosterTime + 4000M : 4000M;
                decimal SuperBoosterTime = decimal.TryParse(xe.GetAttribute("SuperBoosterTime"), out var tempSuperBoosterTime) ? tempSuperBoosterTime + 3500M : 3500M;
                decimal TransAccelFactor = decimal.TryParse(xe.GetAttribute("TransAccelFactor"), out var tempTransAccelFactor) ? tempTransAccelFactor + -0.0045M : 1.4955M;
                decimal BoostAccelFactor = decimal.TryParse(xe.GetAttribute("BoostAccelFactor"), out var tempBoostAccelFactor) ? tempBoostAccelFactor + -0.006M : 1.494M;
                decimal StartBoosterTimeItem = decimal.TryParse(xe.GetAttribute("StartBoosterTimeItem"), out var tempStartBoosterTimeItem) ? tempStartBoosterTimeItem : 1000M;
                decimal StartBoosterTimeSpeed = decimal.TryParse(xe.GetAttribute("StartBoosterTimeSpeed"), out var tempStartBoosterTimeSpeed) ? tempStartBoosterTimeSpeed : 1000M;
                decimal StartForwardAccelForceItem = decimal.TryParse(xe.GetAttribute("StartForwardAccelFactorItem"), out var tempStartForwardAccelForceItem) ? tempStartForwardAccelForceItem * 2102.325M + 171.6355M : 2304M;
                decimal StartForwardAccelForceSpeed = decimal.TryParse(xe.GetAttribute("StartForwardAccelFactorSpeed"), out var tempStartForwardAccelForceSpeed) ? tempStartForwardAccelForceSpeed * 2099.68M + 176.132M : 2304M;
                decimal DriftGaguePreservePercent = decimal.TryParse(xe.GetAttribute("DriftGaguePreservePercent"), out var tempDriftGaguePreservePercent) ? tempDriftGaguePreservePercent : 0M;
                int UseExtendedAfterBooster = (bool.TryParse(xe.GetAttribute("UseExtendedAfterBooster"), out var tempUseExtendedAfterBooster) ? tempUseExtendedAfterBooster : false)? 1 : 0;
                decimal BoostAccelFactorOnlyItem = 1.5M;
                decimal antiCollideBalance = decimal.TryParse(xe.GetAttribute("antiCollideBalance"), out var tempantiCollideBalance) ? tempantiCollideBalance : 1M;
                int dualBoosterSetAuto = (bool.TryParse(xe.GetAttribute("dualBoosterSetAuto"), out var tempdualBoosterSetAuto) ? tempdualBoosterSetAuto : false)? 1 : 0;
                decimal dualBoosterTickMin = decimal.TryParse(xe.GetAttribute("dualBoosterTickMin"), out var tempdualBoosterTickMin) ? tempdualBoosterTickMin : 40M;
                decimal dualBoosterTickMax = decimal.TryParse(xe.GetAttribute("dualBoosterTickMax"), out var tempdualBoosterTickMax) ? tempdualBoosterTickMax : 60M;
                decimal dualMulAccelFactor = decimal.TryParse(xe.GetAttribute("dualMulAccelFactor"), out var tempdualMulAccelFactor) ? tempdualMulAccelFactor : 1.1M;
                decimal dualTransLowSpeed = decimal.TryParse(xe.GetAttribute("dualTransLowSpeed"), out var tempdualTransLowSpeed) ? tempdualTransLowSpeed : 100M;
                int PartsEngineLock = (bool.TryParse(xe.GetAttribute("PartsEngineLock"), out var tempPartsEngineLock) ? tempPartsEngineLock : false)? 1 : 0;
                int PartsWheelLock = (bool.TryParse(xe.GetAttribute("PartsWheelLock"), out var tempPartsWheelLock) ? tempPartsWheelLock : false)? 1 : 0;
                int PartsSteeringLock = (bool.TryParse(xe.GetAttribute("PartsSteeringLock"), out var tempPartsSteeringLock) ? tempPartsSteeringLock : false)? 1 : 0;
                int PartsBoosterLock = (bool.TryParse(xe.GetAttribute("PartsBoosterLock"), out var tempPartsBoosterLock) ? tempPartsBoosterLock : false)? 1 : 0;
                int PartsCoatingLock = (bool.TryParse(xe.GetAttribute("PartsCoatingLock"), out var tempPartsCoatingLock) ? tempPartsCoatingLock : false)? 1 : 0;
                int PartsTailLampLock = (bool.TryParse(xe.GetAttribute("PartsTailLampLock"), out var tempPartsTailLampLock) ? tempPartsTailLampLock : false)? 1 : 0;
                decimal chargeInstAccelGaugeByBoost = decimal.TryParse(xe.GetAttribute("chargeInstAccelGaugeByBoost"), out var tempchargeInstAccelGaugeByBoost) ? tempchargeInstAccelGaugeByBoost : 0.02M;
                decimal chargeInstAccelGaugeByGrip = decimal.TryParse(xe.GetAttribute("chargeInstAccelGaugeByGrip"), out var tempchargeInstAccelGaugeByGrip) ? tempchargeInstAccelGaugeByGrip : 0.02M;
                decimal chargeInstAccelGaugeByWall = decimal.TryParse(xe.GetAttribute("chargeInstAccelGaugeByWall"), out var tempchargeInstAccelGaugeByWall) ? tempchargeInstAccelGaugeByWall : 0.2M;
                decimal instAccelFactor = decimal.TryParse(xe.GetAttribute("instAccelFactor"), out var tempinstAccelFactor) ? tempinstAccelFactor : 1.25M;
                decimal instAccelGaugeCooldownTime = decimal.TryParse(xe.GetAttribute("instAccelGaugeCooldownTime"), out var tempinstAccelGaugeCooldownTime) ? tempinstAccelGaugeCooldownTime * 1000M : 0M;
                decimal instAccelGaugeLength = decimal.TryParse(xe.GetAttribute("instAccelGaugeLength"), out var tempinstAccelGaugeLength) ? tempinstAccelGaugeLength * 1000M : 0M;
                decimal instAccelGaugeMinUsable = decimal.TryParse(xe.GetAttribute("instAccelGaugeMinUsable"), out var tempinstAccelGaugeMinUsable) ? tempinstAccelGaugeMinUsable * instAccelGaugeLength : 0M;
                decimal instAccelGaugeMinVelBound = decimal.TryParse(xe.GetAttribute("instAccelGaugeMinVelBound"), out var tempinstAccelGaugeMinVelBound) ? tempinstAccelGaugeMinVelBound : 200M;
                decimal instAccelGaugeMinVelLoss = decimal.TryParse(xe.GetAttribute("instAccelGaugeMinVelLoss"), out var tempinstAccelGaugeMinVelLoss) ? tempinstAccelGaugeMinVelLoss : 50M;
                int useExtendedAfterBoosterMore = (bool.TryParse(xe.GetAttribute("useExtendedAfterBoosterMore"), out var tempuseExtendedAfterBoosterMore) ? tempuseExtendedAfterBoosterMore : false)? 1 : 0;
                decimal wallCollGaugeCooldownTime = decimal.TryParse(xe.GetAttribute("wallCollGaugeCooldownTime"), out var tempwallCollGaugeCooldownTime) ? tempwallCollGaugeCooldownTime * 1000M : 0M;
                decimal wallCollGaugeMaxVelLoss = decimal.TryParse(xe.GetAttribute("wallCollGaugeMaxVelLoss"), out var tempwallCollGaugeMaxVelLoss) ? tempwallCollGaugeMaxVelLoss : 200M;
                decimal wallCollGaugeMinVelBound = decimal.TryParse(xe.GetAttribute("wallCollGaugeMinVelBound"), out var tempwallCollGaugeMinVelBound) ? tempwallCollGaugeMinVelBound : 200M;
                decimal wallCollGaugeMinVelLoss = decimal.TryParse(xe.GetAttribute("wallCollGaugeMinVelLoss"), out var tempwallCollGaugeMinVelLoss) ? tempwallCollGaugeMinVelLoss : 50M;
                decimal modelMaxX = 0M;
                decimal modelMaxY = 0M;
                Program.Save("\t<id* draftMulAccelFactor='" + draftMulAccelFactor + "' draftTick='" + draftTick + "' driftBoostMulAccelFactor='" + driftBoostMulAccelFactor + "' driftBoostTick='" + driftBoostTick + "' chargeBoostBySpeed='" + chargeBoostBySpeed + "' SpeedSlotCapacity='" + SpeedSlotCapacity + "' ItemSlotCapacity='" + ItemSlotCapacity + "' SpecialSlotCapacity='" + SpecialSlotCapacity + "' UseTransformBooster='" + UseTransformBooster + "' motorcycleType='" + motorcycleType + "' BikeRearWheel='" + BikeRearWheel + "' Mass='" + Mass + "' AirFriction='" + AirFriction + "' DragFactor='" + DragFactor + "' ForwardAccelForce='" + ForwardAccelForce + "' BackwardAccelForce='" + BackwardAccelForce + "' GripBrakeForce='" + GripBrakeForce + "' SlipBrakeForce='" + SlipBrakeForce + "' MaxSteerAngle='" + MaxSteerAngle + "' SteerConstraint='" + SteerConstraint + "' FrontGripFactor='" + FrontGripFactor + "' RearGripFactor='" + RearGripFactor + "' DriftTriggerFactor='" + DriftTriggerFactor + "' DriftTriggerTime='" + DriftTriggerTime + "' DriftSlipFactor='" + DriftSlipFactor + "' DriftEscapeForce='" + DriftEscapeForce + "' CornerDrawFactor='" + CornerDrawFactor + "' DriftLeanFactor='" + DriftLeanFactor + "' SteerLeanFactor='" + SteerLeanFactor + "' DriftMaxGauge='" + DriftMaxGauge + "' NormalBoosterTime='" + NormalBoosterTime + "' ItemBoosterTime='" + ItemBoosterTime + "' TeamBoosterTime='" + TeamBoosterTime + "' AnimalBoosterTime='" + AnimalBoosterTime + "' SuperBoosterTime='" + SuperBoosterTime + "' TransAccelFactor='" + TransAccelFactor + "' BoostAccelFactor='" + BoostAccelFactor + "' StartBoosterTimeItem='" + StartBoosterTimeItem + "' StartBoosterTimeSpeed='" + StartBoosterTimeSpeed + "' StartForwardAccelForceItem='" + StartForwardAccelForceItem + "' StartForwardAccelForceSpeed='" + StartForwardAccelForceSpeed + "' DriftGaguePreservePercent='" + DriftGaguePreservePercent + "' UseExtendedAfterBooster='" + UseExtendedAfterBooster + "' BoostAccelFactorOnlyItem='" + BoostAccelFactorOnlyItem + "' antiCollideBalance='" + antiCollideBalance + "' dualBoosterSetAuto='" + dualBoosterSetAuto + "' dualBoosterTickMin='" + dualBoosterTickMin + "' dualBoosterTickMax='" + dualBoosterTickMax + "' dualMulAccelFactor='" + dualMulAccelFactor + "' dualTransLowSpeed='" + dualTransLowSpeed + "' PartsEngineLock='" + PartsEngineLock + "' PartsWheelLock='" + PartsWheelLock + "' PartsSteeringLock='" + PartsSteeringLock + "' PartsBoosterLock='" + PartsBoosterLock + "' PartsCoatingLock='" + PartsCoatingLock + "' PartsTailLampLock='" + PartsTailLampLock + "' chargeInstAccelGaugeByBoost='" + chargeInstAccelGaugeByBoost + "' chargeInstAccelGaugeByGrip='" + chargeInstAccelGaugeByGrip + "' chargeInstAccelGaugeByWall='" + chargeInstAccelGaugeByWall + "' instAccelFactor='" + instAccelFactor + "' instAccelGaugeCooldownTime='" + instAccelGaugeCooldownTime + "' instAccelGaugeLength='" + instAccelGaugeLength + "' instAccelGaugeMinUsable='" + instAccelGaugeMinUsable + "' instAccelGaugeMinVelBound='" + instAccelGaugeMinVelBound + "' instAccelGaugeMinVelLoss='" + instAccelGaugeMinVelLoss + "' useExtendedAfterBoosterMore='" + useExtendedAfterBoosterMore + "' wallCollGaugeCooldownTime='" + wallCollGaugeCooldownTime + "' wallCollGaugeMaxVelLoss='" + wallCollGaugeMaxVelLoss + "' wallCollGaugeMinVelBound='" + wallCollGaugeMinVelBound + "' wallCollGaugeMinVelLoss='" + wallCollGaugeMinVelLoss + "' modelMaxX='" + modelMaxX + "' modelMaxY='" + modelMaxY + "' />");
            }
        }
    }

    private static void Save(string file)
    {
        File.WriteAllText(@"KartSpec.xml", file);
    }
}
