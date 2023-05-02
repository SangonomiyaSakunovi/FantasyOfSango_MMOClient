using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The Loading Window.

public class LoadingWindow : BaseWindow
{
    public TMP_Text tips;
    public Image loadingProgressFG;
    public Image loadingProgressPoint;
    public TMP_Text loadingProgressText;

    public float loadingProgressFGWidth;

    protected override void InitWindow()
    {
        base.InitWindow();
        loadingProgressFGWidth = loadingProgressFG.GetComponent<RectTransform>().sizeDelta.x;
        SetText(tips, "¸ÐÐ»ÄúµÄ²âÊÔ");
        SetText(loadingProgressText, "0%");
        loadingProgressFG.fillAmount = 0;
        loadingProgressPoint.transform.localPosition = new Vector3(-loadingProgressFGWidth / 2, -444.731f, 0);
    }

    public void SetLoadingProgress(float loadingProgress)
    {
        SetText(loadingProgressText, (int)(loadingProgress * 100) + "%");
        loadingProgressFG.fillAmount = loadingProgress;

        float positionLoadingProgressPoint = loadingProgress * loadingProgressFGWidth - loadingProgressFGWidth / 2;
        loadingProgressPoint.transform.localPosition = new Vector3(positionLoadingProgressPoint, -444.731f, 0);
    }
}
