//Developer : SangonomiyaSakunovi
//Discription: The MissionSystem, all the mission behaviour should define here.

public class MissionSystem : BaseSystem
{
    public static MissionSystem Instance = null;
    public DialogWindow dialogWindow;
    public MainGameWindow mainGameWindow;

    public MissionConfig CurrentMissionConfig { get; private set; }

    private string[] dialogAvaterImageArray = null;
    private string[] dialogTextArray = null;
    private string[] dialogAudioArray = null;

    private int dialogIndex = 0;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void RunMission()
    {
        audioService.PlayBGAudio("TestDialogAudio", true);
        GameManager.Instance.SetGameMode(GameModeCode.DialogueMode);
        mainGameWindow.SetWindowState(false);
        CameraController.Instance.LockCursor(false);
        dialogWindow.SetWindowState();
        if (CurrentMissionConfig.npcID != "-1")
        {

        }
        else
        {
            SetDialogInfo(dialogIndex);
        }
    }

    public void AutoFindMissionPath()
    {

    }

    public void SetCurrentMission(string missionId)
    {
        CurrentMissionConfig = resourceService.GetMissionConfig(missionId);
        dialogAvaterImageArray = CurrentMissionConfig.dialogAvaterImageArray.Split('#');
        dialogTextArray = CurrentMissionConfig.dialogTextArray.Split('#');
        dialogAudioArray = CurrentMissionConfig.dialogAudioArray.Split('#');
        dialogIndex = 1;
    }

    public string GetGuidMissionText()
    {
        return null;
    }

    private void SetDialogInfo(int index)
    {
        string[] dialogTextDetailArray = dialogTextArray[index].Split('|');
        dialogWindow.SetDialogAvaterName(dialogTextDetailArray[0]);
        dialogWindow.SetDialogText(dialogTextDetailArray[1].Replace("$name", OnlineAccountCache.Instance.AvaterInfo.Nickname));
        dialogWindow.SetDialogAvaterImage(dialogAvaterImageArray[index]);
        //TODO
        //PlayAudio
    }

    public void SetNextDialog()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        dialogIndex++;
        if (dialogIndex < dialogTextArray.Length)
        {
            SetDialogInfo(dialogIndex);
        }
        else
        {
            dialogWindow.SetWindowState(false);
            mainGameWindow.SetWindowState();
            GameManager.Instance.SetGameMode(GameModeCode.GamePlayMode);
            audioService.PlayBGAudio(AudioConstant.MainGameBG, true);
            //TODO SendRequest to Server
        }
    }
}
