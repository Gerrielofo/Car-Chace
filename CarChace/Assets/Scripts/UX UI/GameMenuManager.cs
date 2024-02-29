using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _gameEndCanvas;

    [SerializeField] Transform _playerCam;
    [SerializeField] Vector3 _placementOffset;


    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.gameEnded += EndGame;
    }

    public void EndGame()
    {
        _gameEndCanvas.SetActive(true);
        // _gameEndCanvas.transform.LookAt(_playerCam);
        // _gameEndCanvas.transform.position = _playerCam.position + _placementOffset;
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
