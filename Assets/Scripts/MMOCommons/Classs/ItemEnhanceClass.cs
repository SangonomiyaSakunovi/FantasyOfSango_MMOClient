using SangoMMOCommons.Enums;
using System;
using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

[Serializable]
public class ItemEnhanceReq
{
    public ItemTypeCode ItemTypeCode { get; set; }
    public EnhanceTypeCode EnhanceTypeCode { get; set; }
    public string ItemId { get; set; }
    public List<string> ItemModelMaterialList { get; set; }
    public List<string> ItemRawMaterialList { get; set; }
}

[Serializable]
public class ItemEnhanceRsp
{
    public bool IsEnhanceSuccess { get; set; }
    public ItemTypeCode ItemTypeCode { get; set; }
    public EnhanceTypeCode EnhanceTypeCode { get; set; }
}
