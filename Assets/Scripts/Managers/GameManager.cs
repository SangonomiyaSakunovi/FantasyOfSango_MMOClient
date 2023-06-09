//Developer : SangonomiyaSakunovi
//Discription: The Game manager.

using UnityEngine;

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

    public void SetCursorShowType(CursorShowTypeCode cursorShowType)
    {
        if (cursorShowType == CursorShowTypeCode.Hide)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
