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
        SetColor(attackNumText, elementReaction);
        SetText(attackNumText, attackNum);
        Vector3 pos = new Vector3(1600, 500, 0);
        attackNumText.transform.position = new Vector3(Random.Range(pos.x - 200, pos.x + 200), Random.Range(pos.y - 200, pos.y + 200), 0);
        attackNumAni.Play();
    }

    private void SetColor(TMP_Text text, ElementReactionCode elementReaction)
    {
        float Nor = 255f;
        if (elementReaction == ElementReactionCode.Vaporize)
        {
            text.color = new Color(253 / Nor, 196 / Nor, 84 / Nor);
        }
    }
}
