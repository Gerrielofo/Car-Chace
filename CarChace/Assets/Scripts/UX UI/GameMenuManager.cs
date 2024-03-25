using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] _gameEndCanvases;
    [SerializeField] TMP_Text[] _timeSurvivedText;


    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.gameEnded += EndGame;

        for (int i = 0; i < _gameEndCanvases.Length; i++)
        {
            _gameEndCanvases[i].SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _timeSurvivedText.Length; i++)
        {
            _timeSurvivedText[i].text = $"{GetTimeText()}";
        }
    }

    public string GetTimeText()
    {
        float totalTime = GameManager.Instance.timePassed;
        int seconds = (int)totalTime % 60;
        int minutes = (int)totalTime / 60;

        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void EndGame()
    {
        for (int i = 0; i < _gameEndCanvases.Length; i++)
        {
            _gameEndCanvases[i].SetActive(true);
        }
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(GameManager.Instance.gameScene);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(GameManager.Instance.mainMenuScene);
    }
}
