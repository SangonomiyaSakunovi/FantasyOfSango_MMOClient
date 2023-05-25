using SangoCommon.Enums;
using TMPro;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Attack Num Item.

public class AttackNumUIItem : BaseWindow
{
    public TMP_Text attackNumText;
    public Animation attackNumAni;

    public void PlayAttackNum(ElementReactionCode elementReaction, int attackNum, Vector3 attackPosition)
    {
        SetText(attackNumText, attackNum, TextColorCode.VaporizeColor);
        Vector3 pos = new Vector3(1600, 500, 0);
        attackNumText.transform.position = new Vector3(Random.Range(pos.x - 200, pos.x + 200), Random.Range(pos.y - 200, pos.y + 200), 0);
        attackNumAni.Play();
    }
}
