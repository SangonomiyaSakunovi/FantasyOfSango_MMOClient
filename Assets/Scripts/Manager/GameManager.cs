//Developer : SangonomiyaSakunovi
//Discription: The Game manager.

public class GameManager : BaseManager
{
    public static GameManager Instance;

    public GameModeCode GameMode { get; private set; }

    public override void InitManager()
    {
        base.InitManager();
        Instance = this;
    }

    public void SetGameMode(GameModeCode gameMode)
    {
        GameMode = gameMode;
    }
}
