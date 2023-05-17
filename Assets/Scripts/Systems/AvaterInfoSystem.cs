//Developer : SangonomiyaSakunovi
//Discription: The AvaterInfo Sytem.

using UnityEngine;

public class AvaterInfoSystem : BaseSystem
{
    public AvaterInfoWindow avaterInfoWindow;
    public MainGameWindow mainGameWindow;

    private Transform avaterShowCameraTrans;

    private Transform playerTrans;
    private Vector3 offsetPosition;

    public static AvaterInfoSystem Instance;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void OpenAvaterInfoWindow()
    {
        audioService.PlayUIAudio(AudioConstant.AttributeAudio);        
        avaterShowCameraTrans.position = offsetPosition + playerTrans.position;
        avaterShowCameraTrans.LookAt(playerTrans.position);
        avaterShowCameraTrans.gameObject.SetActive(true);

        mainGameWindow.SetWindowState(false);
        avaterInfoWindow.SetWindowState();
    }

    public void CloseAvaterInfoWindow()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        avaterInfoWindow.SetWindowState(false);
        mainGameWindow.SetWindowState();
    }

    public void SetPlayerTrans(Transform transform)
    {
        playerTrans = transform;
    }

    public void SetAvaterShowCameraTrans(Transform transform)
    {
        avaterShowCameraTrans = transform;
    }

    public void InitAvaterShowCamera()
    {
        offsetPosition = playerTrans.forward * 3;
        avaterShowCameraTrans.gameObject.SetActive(false);
    }
}
