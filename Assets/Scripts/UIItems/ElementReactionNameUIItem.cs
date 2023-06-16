using SangoMMOCommons.Enums;
using TMPro;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class ElementReactionNameUIItem : BaseWindow
{
    public TMP_Text elementReactionNameText;
    public Animation elementReactionNameAni;

    public void PlayElementReactionName(ElementReactionCode elementReaction)
    {
        SetElementReactionName(elementReactionNameText, elementReaction);
        elementReactionNameAni.Play();
    }

    private void SetElementReactionName(TMP_Text text, ElementReactionCode elementReaction)
    {
        if (elementReaction == ElementReactionCode.Vaporize)
        {
            SetText(elementReactionNameText, "����", TextColorCode.VaporizeColor);
        }
        else if (elementReaction == ElementReactionCode.Melt)
        {
            SetText(elementReactionNameText, "�ڻ�", TextColorCode.MeltColor);
        }
        else if (elementReaction == ElementReactionCode.Frozen)
        {
            SetText(elementReactionNameText, "����", TextColorCode.FrozenColor);
        }
    }
}
