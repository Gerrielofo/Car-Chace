using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] float _startHealth;
    [SerializeField] float _startSpeed;
    [SerializeField] float _damageMultiplier;
    [SerializeField] float _maxPointGain;

    float _health;
    float _speed;
    float _timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        _health = _startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _timeAlive += Time.deltaTime;
    }


    public void TakeDamage(float playerSpeed)
    {
        _health -= playerSpeed * _damageMultiplier;

        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        float pointMultiplier = _maxPointGain / _timeAlive * 10;
        int pointsToGain = (int)pointMultiplier;

        GameManager.Instance.AddPoints(pointsToGain);
    }
}
