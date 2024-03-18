using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    CarAgentFollow _carAI;
    [Header("Enemy Settings")]
    [SerializeField] float _startHealth;
    [SerializeField] float _startSpeed;
    [SerializeField] float _damageMultiplier = 5f;
    [SerializeField] float _maxPointGain;
    [SerializeField] float _damageInterval;

    [Header("UI")]
    [SerializeField] Transform _canvas;
    [SerializeField] Slider _healthSlider;
    [SerializeField] float _healthShowTime = 3f;
    float _healthTimer;

    [Header("Info")]
    [SerializeField] float _health;
    [SerializeField] float _currentSpeed;
    [SerializeField] bool _isAlive;
    [SerializeField] float _timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        _carAI = GetComponent<CarAgentFollow>();
        _health = _startHealth;
        _healthSlider.maxValue = _startHealth;
        _healthSlider.value = _health;
        _carAI.isAlive = true;
        _isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        _timeAlive += Time.deltaTime;
        if (_healthTimer > 0)
        {
            _healthTimer -= Time.deltaTime;
            _canvas.LookAt(Camera.main.transform);
        }
        else if (_canvas.gameObject.activeSelf)
        {
            _canvas.gameObject.SetActive(false);
        }

        _currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
    }


    public void TakeDamage(float playerSpeed)
    {
        if (!_isAlive)
        {
            return;
        }
        Debug.Log($"Hittting car for {playerSpeed * _damageMultiplier}");
        _canvas.gameObject.SetActive(true);
        _healthTimer = _healthShowTime;

        _health -= playerSpeed * _damageMultiplier;
        _healthSlider.value = _health;
        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _isAlive = false;
        float pointMultiplier = _maxPointGain / _timeAlive * 10;
        int pointsToGain = (int)pointMultiplier;

        _carAI.isAlive = false;
        _canvas.gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPoints(pointsToGain);
        }

        GetComponent<CarCrash>().crash = true;
        Destroy(gameObject, 5f);
    }
}
