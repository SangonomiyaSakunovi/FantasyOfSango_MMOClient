using SangoCommon.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The Enemy Hp Item.

public class EnemyHpUIItem : BaseWindow
{
    private int HPFull;

    public Image hpFG;

    #region ElementType
    public Image element1Pyro;
    public Image element1Hydro;
    public Image element1Cryo;
    #endregion

    public TMP_Text levelText;
    public Image focusEnemyImg;
    public Animation focusEnemyAni;

    public void InitItem(int hpFull)
    {
        HPFull = hpFull;
    }


    public void SetHPFG(int hp)
    {
        float fillAmout = (float)hp / HPFull;
        hpFG.fillAmount = fillAmout;
    }

    public void SetElement1Image(ElementTypeCode elementType)
    {
        if (elementType == ElementTypeCode.Hydro)
        {
            SetActive(element1Hydro);
        }
        else if (elementType == ElementTypeCode.Pyro)
        {
            SetActive(element1Pyro);
        }
        else if (elementType == ElementTypeCode.Cryo)
        {
            SetActive(element1Cryo);
        }
    }

    public void RemoveElement1Image(ElementTypeCode elementType)
    {
        if (elementType == ElementTypeCode.Hydro)
        {
            SetActive(element1Hydro, false);
        }
        else if (elementType == ElementTypeCode.Pyro)
        {
            SetActive(element1Pyro, false);
        }
        else if (elementType == ElementTypeCode.Cryo)
        {
            SetActive(element1Cryo, false);
        }
    }

    public void SetLevelText(int level)
    {
        SetText(levelText, "Lv. " + level);
    }

    public void SetIsFocusEnemy(bool bo = true)
    {
        if (bo)
        {
            SetActive(focusEnemyImg);
        }
        else
        {
            SetActive(focusEnemyImg, false);
        }
    }
}
