using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionsManager : MonoBehaviour
{
    public static GameOptionsManager Instance;

    public int timeLimitMinutes;
    public int enemyHealthPrc;

    public int turnType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Found A Second Instance! Destroyed The One On {gameObject.name}");
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }
}
