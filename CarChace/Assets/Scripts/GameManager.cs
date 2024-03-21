using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int points;

    public bool isPlaying;
    public float timePassed;

    public event GameEnded gameEnded;
    [Header("Scene Names")]
    public string mainMenuScene;
    public string gameScene = "Marnix Map";


    [Header("Playprefs Names")]
    public string speedPower = "speedPowerUps";
    public string spikePower = "spikePowerUps";
    public string helicopterPower = "helicopterPowerUps";
    public string reinforcementPower = "reinformentPowerUps";

    ShopManager _shopManager;
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
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _shopManager = FindObjectOfType<ShopManager>();
        points = PlayerPrefs.GetInt("Points");
        AddPoints(1000);
        _shopManager.UpdatePointCount();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneloaded;
    }

    private void OnSceneloaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        if (scene.name == mainMenuScene)
        {
            timePassed = 0f;
        }
    }
    public bool updateCount;
    private void Update()
    {
        if (updateCount)
        {
            _shopManager.UpdatePointCount();
        }
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void AddPoints(int amount)
    {
        points += amount;
        Debug.Log($"Added {amount} points");
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
