using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int points;

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
    }



    public bool CanAfford(int amount)
    {
        return points >= amount;
    }

    public void UsePoints(int amount)
    {
        points -= amount;
    }
}
