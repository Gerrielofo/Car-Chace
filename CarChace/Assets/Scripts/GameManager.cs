using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int points;

    public bool isPlaying;
    public float timePassed;

    [Header("Scene Names")]
    public string mainMenuScene;
    public string gameScene = "Marnix Map";

    public event GameEnded gameEnded;


    private void Awake()
    {
        if (Instance == this)
        {
            return;
        }
        else if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Had Multiple GameManager's In Scene. Destroyed The One On {gameObject.name}");
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // points = PlayerPrefs.GetInt("Points");
    }

    private void Update()
    {
        if (isPlaying)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= GameOptionsManager.Instance.timeLimitMinutes * 60)
            {
                EndGame();
            }
        }
    }

    public delegate void GameEnded();

    public void EndGame()
    {
        isPlaying = false;
        Time.timeScale = 0;
        if (gameEnded != null)
        {
            gameEnded.Invoke();
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        PlayerPrefs.SetInt("Points", points);
    }

    public bool CanAfford(int amount)
    {
        return points >= amount;
    }

    public void UsePoints(int amount)
    {
        points -= amount;
        PlayerPrefs.SetInt("Points", points);
    }
}
