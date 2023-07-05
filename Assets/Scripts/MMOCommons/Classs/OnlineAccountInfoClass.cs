using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class SyncPlayerTransformRsp
{
    public bool SyncPlayerTransformResult { get; set; }
    public bool PredictPlayerTransform { get; set; }
    public TransformOnline TransformOnline { get; set; }
}

[Serializable]
public class TransformOnline
{
    public string Account { get; set; }
    public Vector3Position Vector3Position { get; set; }
    public QuaternionRotation QuaternionRotation { get; set; }
}

[Serializable]
public class NewAccountOnline
{
    public string Account { get; set; }
    public AvaterCode AvaterCode { get; set; }
    public Vector3Position Vector3Position { get; set; }
    public QuaternionRotation QuaternionRotation { get; set; }
}
