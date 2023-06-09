using UnityEngine;

//Developer : SangonomiyaSakunovi

public class AvaterInfoSystem : BaseSystem
{
    public AvaterInfoWindow avaterInfoWindow;
    public MainGameWindow mainGameWindow;

    private GameObject avaterShowTablet;
    private GameObject avaterShowCube;

    public static AvaterInfoSystem Instance;

    private float avaterShowCubeCurrentRotation;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void OpenAvaterInfoWindow()
    {
        GameManager.Instance.SetGameMode(GameModeCode.ConfigureItemMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Show);
        audioService.PlayUIAudio(AudioConstant.AttributeAudio);
        ResetAvaterShowRotation();
        avaterShowTablet.SetActive(true);
        mainGameWindow.SetWindowState(false);
        avaterInfoWindow.SetWindowState();
    }

    public void CloseAvaterInfoWindow()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        avaterInfoWindow.SetWindowState(false);
        mainGameWindow.SetWindowState();
        avaterShowTablet.SetActive(false);
        GameManager.Instance.SetGameMode(GameModeCode.GamePlayMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Hide);
    }

    public void InitAvaterShowTablet()
    {
        avaterShowTablet = GameObject.FindGameObjectWithTag("AvaterShowTablet");
        avaterShowCube = avaterShowTablet.transform.Find("PlayerCube").gameObject;
        avaterShowTablet.SetActive(false);
    }

    public void SetAvaterShowCubeCurrentRotation()
    {
        avaterShowCubeCurrentRotation = avaterShowCube.transform.localEulerAngles.y;
    }

    public void SetAvaterShowRotation(float rotation)
    {
        avaterShowCube.transform.localEulerAngles = new Vector3(0, avaterShowCubeCurrentRotation + rotation, 0);
    }

    private void ResetAvaterShowRotation()
    {
        avaterShowCube.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
