using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject LoginObj;
    public TMP_InputField UserName;
    public GameObject ScoreObj;
    public TextMeshProUGUI Player1Score, Player2Score;
    public TextMeshProUGUI Player1NameText, Player2NameText;
    //[HideInInspector]
    public int score1, score2;
    public GameObject ModeSelectObj;
    public GameObject OrderSelectObj;
    public TextMeshProUGUI OrderText;
    public TextMeshProUGUI GameResultText;
    public GameObject GameOverObj;
    public TextMeshProUGUI ResultText;

    void Start()
    {
        GameController.UIController = this;
        LoginObj.SetActive(true);
        ScoreObj.SetActive(false);
        ModeSelectObj.SetActive(false);
        OrderSelectObj.SetActive(false);
        GameOverObj.SetActive(false);

    }

    void Update()
    {

    }

    public void AddScore(int result)
    {
        if (result == 1)
            score1++;
        else if (result == 2)
            score2++;
        Player1Score.text = score1.ToString();
        Player2Score.text = score2.ToString();
        GameOver(result);
    }

    public void GameStart()
    {
        if (string.IsNullOrEmpty(UserName.text))
            return;
        GameController.Player1Name = UserName.text;
        LoginObj.SetActive(false);
        ModeSelectObj.SetActive(true);

    }

    public void SelectGameMode(int mode)
    {
        GameController.SetGameMode((GameMode)mode);
        ModeSelectObj.SetActive(false);
        OrderSelectObj.SetActive(true);

        score1 = score2 = 0;
        Player1Score.text = score1.ToString();
        Player2Score.text = score2.ToString();
        Player1NameText.text = GameController.Player1Name;
        Player2NameText.text = GameController.Player2Name;
    }

    public void SelectOrder(int order)
    {
        GameController.SetCurrentPlayer(order);
        OrderSelectObj.SetActive(false);
        ScoreObj.SetActive(true);
        GameController.GameOver = false;
    }

    public void GameOver(int result)
    {
        GameOverObj.SetActive(true);
        if (result == 0)
            GameResultText.text = "平局";
        else if (result == 1)
            GameResultText.text = $"{GameController.Player1Name} 获胜";
        else if (result == 2)
            GameResultText.text = $"{GameController.Player2Name} 获胜";
    }

    public void Restart()
    {
        GameController.Restart();
        GameOverObj.SetActive(false);
        OrderSelectObj.SetActive(true);
    }

    public void ResetGame()
    {
        GameController.Restart();
        GameOverObj.SetActive(false);
        ModeSelectObj.SetActive(true);
        ScoreObj.SetActive(false);
    }
}
