using UnityEngine;
using UnityEngine.UI;

//Developer: SangonomiyaSakunovi

public class HotFixWindow : MonoBehaviour
{
    public Button confirmButton;
    public Button cancelButton;
    public Text hotFixInfoText;
    public Text Tips;
    public Image loadingProgressFG;
    public Image loadingProgressPoint;
    public Text loadingProgressText;

    private float loadingProgressFGWidth;

    private float loadingProgressPointYPos = 88.89987f;

    protected void InitWindow()
    {
        loadingProgressFGWidth = loadingProgressFG.GetComponent<RectTransform>().sizeDelta.x;
        loadingProgressText.text = "0%";
        loadingProgressFG.fillAmount = 0;
        loadingProgressPoint.transform.localPosition = new Vector3(-loadingProgressFGWidth / 2, loadingProgressPointYPos, 0);
    }

    public void OnConfirmButtonClick()
    {
        HotFixService.Instance.RunHotFix();
    }

    public void OnCancelButtonClick()
    {
        //TODO
    }

    public void SetHotFixInfoText(long totalDownloadBytes)
    {
        long totalUploadMB = totalDownloadBytes / 1048576;
        string text = "当前需要下载更新" + totalDownloadBytes + "MB左右，\n是否继续？\n（建议在Wifi环境下进行）";
        hotFixInfoText.text = text;
    }

    public void SetTips(string text)
    {
        Tips.text = text;
    }

    public void SetLoadingProgress(float loadingProgress)
    {
        loadingProgressText.text = (int)(loadingProgress * 100) + "%";
        loadingProgressFG.fillAmount = loadingProgress;
        float positionLoadingProgressPoint = loadingProgress * loadingProgressFGWidth - loadingProgressFGWidth / 2;
        loadingProgressPoint.transform.localPosition = new Vector3(positionLoadingProgressPoint, loadingProgressPointYPos, 0);
    }
}
