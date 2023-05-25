using TMPro;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The Dialog Window.

public class DialogWindow : BaseWindow
{
    public TMP_Text dialogAvaterName;
    public TMP_Text dialogText;
    public Image dialogAvaterImage;
    public Button nextButton;

    protected override void InitWindow()
    {
        base.InitWindow();
    }

    public void SetDialogText(string text)
    {
        SetText(dialogText, text, TextColorCode.WhiteColor);
    }

    public void SetDialogAvaterName(string name)
    {
        SetText(dialogAvaterName, name, TextColorCode.OrangeColor);
    }

    public void SetDialogAvaterImage(string dialogAvaterImg)
    {
        string imagePath = "";
        switch (dialogAvaterImg)
        {
            case DialogConstant.AetherAvaterImageIdle:
                imagePath = DialogConstant.AetherAvaterImageIdlePath;
                break;
            case DialogConstant.PaimonAvaterImageIdle:
                imagePath = DialogConstant.PaimonAvaterImageIdlePath;
                break;
            case DialogConstant.UnknownAvaterImageIdle:
                imagePath = DialogConstant.UnknownAvaterImageIdlePath;
                break;
        }
        SetSprite(dialogAvaterImage, imagePath);
    }

    public void OnNextButtonClick()
    {
        MissionSystem.Instance.SetNextDialog();
    }
}
