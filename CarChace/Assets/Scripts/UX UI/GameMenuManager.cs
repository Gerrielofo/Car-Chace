using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] _gameEndCanvases;


    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.gameEnded += EndGame;

        for (int i = 0; i < _gameEndCanvases.Length; i++)
        {
            _gameEndCanvases[i].SetActive(false);
        }
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
        SceneManager.LoadScene(GameManager.Instance.gameScene);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(GameManager.Instance.mainMenuScene);
    }
}
