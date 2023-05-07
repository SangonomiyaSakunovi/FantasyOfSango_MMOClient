//Developer : SangonomiyaSakunovi
//Discription: The Game manager.

public class GameManager : BaseManager
{
    public static GameManager Instance;

    public GameModeCode GameMode;

    public override void InitManager()
    {
        base.InitManager();
        Instance = this;
    }

}
