using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _gameEndCanvases;
    [SerializeField] TMP_Text[] _timeSurvivedText;


    private void Start()
    {
        _gameEndCanvases.Clear();
        _gameEndCanvases = GameObject.FindGameObjectsWithTag("EndScreen").ToList();

        for (int i = 0; i < _gameEndCanvases.Count; i++)
        {
            _gameEndCanvases[i].SetActive(false);
        }
        Time.timeScale = 1;

        if (GameManager.Instance)
        {
            GameManager.Instance.isPlaying = true;
            GameManager.Instance.timePassed = 0;
        }

    }

    private void OnEnable()
    {
        if (GameManager.Instance)
            GameManager.Instance.gameEnded += EndGame;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.gameEnded -= EndGame;
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
        if (!GameManager.Instance)
            return "XX:XX";
        float totalTime = GameManager.Instance.timePassed;
        int seconds = (int)totalTime % 60;
        int minutes = (int)totalTime / 60;

        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void EndGame()
    {
        for (int i = 0; i < _gameEndCanvases.Count; i++)
        {
            _gameEndCanvases[i].SetActive(true);
        }
    }

    public void Retry()
    {
        if (GameManager.Instance)
        {
            SceneManager.LoadScene(GameManager.Instance.gameScene);
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void BackToMainMenu()
    {
        if (GameManager.Instance)
        {
            SceneManager.LoadScene(GameManager.Instance.mainMenuScene);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
