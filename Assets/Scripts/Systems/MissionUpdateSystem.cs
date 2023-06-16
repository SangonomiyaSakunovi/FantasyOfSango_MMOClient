using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

public class MissionUpdateSystem : BaseSystem
{
    public static MissionUpdateSystem Instance = null;
    
    public DialogWindow dialogWindow;
    public MainGameWindow mainGameWindow;

    public MissionConfig CurrentMissionConfig { get; private set; }

    private MissionUpdateRequest missionUpdateRequest;
    private MissionUpdateReq missionUpdateReq;
    private string currentMissionId;

    private string[] dialogAvaterImageArray = null;
    private string[] dialogTextArray = null;
    private string[] dialogAudioArray = null;

    private int dialogIndex = 0;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        missionUpdateRequest = GetComponent<MissionUpdateRequest>();
        missionUpdateReq = new MissionUpdateReq();
    }

    public void RunMission()
    {
        audioService.PlayBGAudio("TestDialogAudio", true);
        GameManager.Instance.SetGameMode(GameModeCode.DialogueMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Show);
        mainGameWindow.SetWindowState(false);
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
        currentMissionId = missionId;
    }

    private void OnMissionComplete()
    {
        missionUpdateReq.MissionId = currentMissionId;
        missionUpdateReq.MissionTypeCode = MissionTypeCode.Main;
        missionUpdateReq.MissionUpdateTypeCode = MissionUpdateTypeCode.Complete;
        missionUpdateRequest.SetMissionUpdateReq(missionUpdateReq);
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
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        dialogIndex++;
        if (dialogIndex < dialogTextArray.Length)
        {
            SetDialogInfo(dialogIndex);
        }
        else
        {
            dialogWindow.SetWindowState(false);
            mainGameWindow.SetWindowState();            
            audioService.PlayBGAudio(AudioConstant.MainGameBG, true);
            OnMissionComplete();
            missionUpdateRequest.DefaultRequest();
            GameManager.Instance.SetGameMode(GameModeCode.GamePlayMode);
        }
    }

    public void OnMissionCompleteResponse(MissionUpdateRsp missionUpdateRsp)
    {
        if (missionUpdateRsp.MissionUpdateTypeCode == MissionUpdateTypeCode.Complete)
        {
            UpdateMissionCompleteRewards(missionUpdateRsp);
        }
    }

    private void UpdateMissionCompleteRewards(MissionUpdateRsp missionUpdateRsp)
    {
        //TODO
        SangoRoot.AddMessage("成功完成当前任务，奖励已经发放", TextColorCode.GreenColor);
        MissionConfig missionConfig = resourceService.GetMissionConfig(missionUpdateRsp.MissionId);
        OnlineAccountCache.Instance.ItemInfo.Coin += missionConfig.coinRewards;
    }
}
