using System.Collections.Generic;
using System.Xml.Serialization;

//Developer : SangonomiyaSakunovi
//Discription: The ABConfig, not done.

[System.Serializable]
public class AssetBundleConfig
{
    [XmlElement("ABList")]
    public List<ABBase> ABList { get; set; }
}

[System.Serializable]
public class ABBase
{
    [XmlAttribute("Path")]
    public string Path { get; set; }
    [XmlAttribute("Crc")]
    public uint Crc { get; set; }
    [XmlAttribute("ABName")]
    public string ABName { get; set; }
    [XmlAttribute("AssetName")]
    public string AssetName { get; set; }
    [XmlElement("ABDependList")]
    public List<string> ABDependList { get; set; }
}
