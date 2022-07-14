using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class COMController : MonoBehaviour
{
    public float WaitingTime;
    public float ReadyTime;
    public int SelfNumber;
    public Vector2Int NextMove;

    private void Awake()
    {
    }

    void Start()
    {
        GameController.COM = this;
        WaitingTime = 0;
        ReadyTime = 0;
        NextMove = new(0, 0);
        SelfNumber = 2;
    }

    public void SetPlayerNumber(int num)
    {
        SelfNumber = num;
    }

    void Update()
    {
        SequenceProcess(new()
        {
            () => SequenceProcess(new()
            {
                ShouldMove,
                Delay
            }),
            () => SelectProcess(new()
            {
                () => CheckCanWin(Target.COM),
                () => CheckCanWin(Target.Player),
                FindBestMove
            }),
            DoMove
        });
    }

    private bool SelectProcess(List<Func<bool>> funcs)
    {
        foreach (var func in funcs)
            if (func())
                return true;
        return false;
    }

    private bool SequenceProcess(List<Func<bool>> funcs)
    {
        foreach (var func in funcs)
            if (!func())
                return false;
        return true;
    }

    private bool ShouldMove()
    {
        if (GameController.GameOver || GameController.Mode == GameMode.PVP)
            return false;
        if (GameController.CurrentPlayer == SelfNumber)
            return true;
        return false;
    }

    private bool Delay()
    {
        if (ReadyTime == 0)
        {
            Debug.Log("start delay");
            ReadyTime = Random.Range(1, 2);
        }
        if (WaitingTime > ReadyTime)
        {
            ReadyTime = 0;
            WaitingTime = 0;
            return true;
        }
        WaitingTime += Time.deltaTime;
        return false;
    }

    private bool CheckCanWin(Target target)
    {
        var map = GameController.Map.MapData;
        int num = 0;
        if (target == Target.COM)
            num = SelfNumber;
        else
            num = 3 - SelfNumber;
        for (int i = 1; i <= 3; i++)
            for (int j = 1; j <= 3; j++)
            {
                if (map[i][j] != 0)
                    continue;
                if (CheckWin(map, i, j, num))
                {
                    NextMove = new(i, j);
                    return true;
                }
            }
        Debug.Log($"cant win {target}");
        return false;
    }

    private bool CheckWin(int[][] rawMap, int x, int y, int num)
    {
        int[][] map = new int[4][];
        for (int i = 0; i < 4; i++)
            map[i] = rawMap[i].Clone() as int[];
        map[x][y] = num;
        for (int i = 1; i <= 3; i++)
        {
            if (map[i][1] == map[i][2] && map[i][2] == map[i][3] && map[i][1] != 0)
                return true;
            if (map[1][i] == map[2][i] && map[2][i] == map[3][i] && map[1][i] != 0)
                return true;
        }

        if (map[1][1] == map[2][2] && map[2][2] == map[3][3] && map[2][2] != 0)
            return true;
        if (map[1][3] == map[2][2] && map[2][2] == map[3][1] && map[2][2] != 0)
            return true;

        return false;
    }

    private bool FindBestMove()
    {
        var map = GameController.Map.MapData;
        List<Vector2Int> nodes = new();

        // check middle
        if (map[2][2] == 0)
        {
            NextMove = new(2, 2);
            return true;
        }

        nodes.Clear();
        // check cornor
        int[] x = new int[4] { 1, 1, 3, 3 };
        int[] y = new int[4] { 1, 3, 1, 3 };
        for (int i = 0; i < 4; i++)
            if (map[x[i]][y[i]] == 0)
                nodes.Add(new(x[i], y[i]));
        if (nodes.Count != 0)
        {
            int index = Random.Range(0, nodes.Count);
            NextMove = nodes[index];
            return true;
        }

        nodes.Clear();
        // check border
        x = new int[4] { 2, 1, 3, 2 };
        y = new int[4] { 1, 2, 2, 3 };
        for (int i = 0; i < 4; i++)
            if (map[x[i]][y[i]] == 0)
                nodes.Add(new(x[i], y[i]));
        if (nodes.Count != 0)
        {
            int index = Random.Range(0, nodes.Count);
            NextMove = nodes[index];
            return true;
        }

        return false;
    }

    private bool DoMove()
    {
        GameController.Map.Move(NextMove);
        return true;
    }
}

public enum Target
{
    COM,
    Player
}