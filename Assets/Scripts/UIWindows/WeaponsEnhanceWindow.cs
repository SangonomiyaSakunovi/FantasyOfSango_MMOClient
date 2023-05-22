using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The WeaponsEnhanceWindow.

public class WeaponsEnhanceWindow : BaseWindow
{
    public Transform buttonsTransform;

    private Image[] buttonBGImageArray = new Image[3];
    private int currentButtonIndex;

    protected override void InitWindow()
    {
        base.InitWindow();
        RegistClickEvents();
        OnItemClick(0);
    }

    private void RegistClickEvents()
    {
        for (int i = 0; i < buttonsTransform.childCount; i++)
        {
            Image image = buttonsTransform.GetChild(i).GetComponent<Image>();
            OnClick(image.gameObject, (object args) =>
            {
                OnItemClick((int)args);
                audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
            }, i);
            buttonBGImageArray[i] = image;
        }
    }

    private void OnItemClick(int index)
    {
        currentButtonIndex = index;
        for (int i = 0; i < buttonBGImageArray.Length; i++)
        {
            Transform trans = buttonBGImageArray[i].transform;
            if (i == currentButtonIndex)
            {
                SetSprite(buttonBGImageArray[i], ButtonConstant.EnhanceSelectedImagePath);
            }
            else
            {
                SetSprite(buttonBGImageArray[i], ButtonConstant.EnhanceUnSelectedImagePath);
            }
        }
    }
}
