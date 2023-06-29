//Developer: SangonomiyaSakunovi

public class LoadingSystem : BaseSystem
{
    public static LoadingSystem Instance;

    public LoadingWindow loadingWindow;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void OpenLoadingWindow()
    {
        loadingWindow.SetWindowState();
    }

    public void SetLoadingProgress(float loadingProgress)
    {
        loadingWindow.SetLoadingProgress(loadingProgress);
    }

    public void SetTips(string tips)
    {
        loadingWindow.SetTips(tips);
    }

    public void CloseLoadingWindow()
    {
        loadingWindow.SetWindowState(false);
    }
}
