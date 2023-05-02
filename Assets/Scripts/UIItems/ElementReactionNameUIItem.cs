using SangoCommon.Enums;
using TMPro;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The ElementReaction Item.

public class ElementReactionNameUIItem : BaseWindow
{
    public TMP_Text elementReactionNameText;
    public Animation elementReactionNameAni;

    public void PlayElementReactionName(ElementReactionCode elementReaction)
    {
        SetColor(elementReactionNameText, elementReaction);
        elementReactionNameAni.Play();
    }

    private void SetColor(TMP_Text text, ElementReactionCode elementReaction)
    {
        float Nor = 255f;
        if (elementReaction == ElementReactionCode.Vaporize)
        {
            SetText(elementReactionNameText, "Õô·¢");
            text.color = new Color(254 / Nor, 197 / Nor, 82 / Nor);
        }
        else if (elementReaction == ElementReactionCode.Melt)
        {
            SetText(elementReactionNameText, "ÈÚ»¯");
            text.color = new Color(255 / Nor, 201 / Nor, 100 / Nor);
        }
        else if (elementReaction == ElementReactionCode.Frozen)
        {
            SetText(elementReactionNameText, "¶³½á");
            text.color = new Color(144 / Nor, 246 / Nor, 255 / Nor);
        }
    }
}
