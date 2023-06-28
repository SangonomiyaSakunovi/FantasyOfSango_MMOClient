using TMPro;
using UnityEngine.UI;

//Developer: SangonomiyaSakunovi

public class HotFixWindow : BaseWindow
{
    public TMP_Text hotFixInfoText;
    public Button confirmButton;
    public Button cancelButton;

    protected override void InitWindow()
    {
        base.InitWindow();
    }

    public void OnConfirmButtonClick()
    {
        HotFixSystem.Instance.RunHotFix();
    }

    public void OnCancelButtonClick()
    {
        //TODO
    }

    public void SetHotFixInfoText(long totalDownloadBytes)
    {
        string text = "当前需要下载更新" + totalDownloadBytes + "左右，\n是否继续？\n（建议在Wifi环境下进行）";
        SetText(hotFixInfoText, text);
    }
}
