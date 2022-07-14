using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController control;
    public static Map Map;
    public static UIController UIController;
    public static COMController COM;
    public static int CurrentPlayer = -1;
    public static bool GameOver = true;

    // 0 - µ¥ÈË; 1 - Ë«ÈË
    public static GameMode Mode = GameMode.PVP;
    public static string Player1Name = "Player1", Player2Name = "Player2";

    public static int Player1Score = 0, Player2Score = 0;
    public static int Player1 = 1, Player2 = 2;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SetCurrentPlayer(1);
    }

    void Awake()
    {
        if (control == null)
            control = this;
        else if (control != this)
            Destroy(gameObject);
    }

    public static void SetGameMode(GameMode mode)
    {
        Mode = mode;
        if (Mode == GameMode.PVP)
            Player2Name = "Player2";
        else
            Player2Name = "COM";
    }

    public static void Restart()
    {
        Map.Restart();
    }

    public static void TurnEnd()
    {
        int result = TryCheckWin();
        if (result != 0)
        {
            GameOver = true;
            UIController.AddScore(result);
        }
        CurrentPlayer = 3 - CurrentPlayer;
        for (int i = 1; i <= 3; i++)
            for (int j = 1; j <= 3; j++)
                if (Map.MapData[i][j] == 0)
                    return;
        GameOver = true;
        UIController.AddScore(0);
    }

    public static void SetCurrentPlayer(int player)
    {
        CurrentPlayer = player;
    }

    private static int TryCheckWin()
    {
        var map = Map.MapData;

        for (int i = 1; i <= 3; i++)
        {
            if (map[i][1] == map[i][2] && map[i][2] == map[i][3] && map[i][1] != 0)
                return map[i][1];
            if (map[1][i] == map[2][i] && map[2][i] == map[3][i] && map[1][i] != 0)
                return map[1][i];
        }

        if (map[1][1] == map[2][2] && map[2][2] == map[3][3] && map[1][1] != 0)
            return map[1][1];
        if (map[1][3] == map[2][2] && map[2][2] == map[3][1] && map[1][3] != 0)
            return map[1][3];

        return 0;
    }
}

public enum GameMode
{
    PVE,
    PVP
}