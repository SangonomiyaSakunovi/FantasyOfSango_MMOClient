using SangoMMOCommons.Constants;
using SangoMMOCommons.Enums;
using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class ElementApplication
{
    public ElementTypeCode Type { get; set; }
    public float Gauge { get; set; }
    public float DecayRate { get; set; }
    public float RemainTime { get; set; }

    public ElementApplication(ElementTypeCode element, float gauge)
    {
        Type = element;
        Gauge = gauge;
        RemainTime = ElementConstant.ElementRemainTimeBase + ElementConstant.ElementRemainTimePlus * Gauge;
        DecayRate = ElementConstant.ElementRemainCount * Gauge / RemainTime;
    }
}
