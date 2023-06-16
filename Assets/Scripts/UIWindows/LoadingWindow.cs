using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi

public class LoadingWindow : BaseWindow
{
    public TMP_Text tips;
    public Image loadingProgressFG;
    public Image loadingProgressPoint;
    public TMP_Text loadingProgressText;

    public float loadingProgressFGWidth;

    private float loadingProgressPointYPos = -444.731f;

    protected override void InitWindow()
    {
        base.InitWindow();
        loadingProgressFGWidth = loadingProgressFG.GetComponent<RectTransform>().sizeDelta.x;
        SetText(tips, "¸ÐÐ»ÄúµÄ²âÊÔ", TextColorCode.OrangeColor);
        SetText(loadingProgressText, "0%", TextColorCode.OrangeColor);
        loadingProgressFG.fillAmount = 0;
        loadingProgressPoint.transform.localPosition = new Vector3(-loadingProgressFGWidth / 2, loadingProgressPointYPos, 0);
    }

    public void SetLoadingProgress(float loadingProgress)
    {
        SetText(loadingProgressText, (int)(loadingProgress * 100) + "%", TextColorCode.OrangeColor);
        loadingProgressFG.fillAmount = loadingProgress;
        float positionLoadingProgressPoint = loadingProgress * loadingProgressFGWidth - loadingProgressFGWidth / 2;
        loadingProgressPoint.transform.localPosition = new Vector3(positionLoadingProgressPoint, loadingProgressPointYPos, 0);
    }
}
