using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforcements : MonoBehaviour
{
    [SerializeField] GameObject _reinforcementPrefab;
    [SerializeField] Transform _spawnTransform;
    [SerializeField] float _spawnDelay;

    public void SpawnReinforcements(float amount, float duration)
    {
        StartCoroutine(SpawnCar(_spawnDelay, amount, duration));
    }

    IEnumerator SpawnCar(float spawnDelay, float amount, float duration)
    {
        yield return new WaitForSeconds(spawnDelay);
        GameObject car = Instantiate(_reinforcementPrefab, _spawnTransform.position, _spawnTransform.rotation);
        car.GetComponent<PoliceAgentFollow>().maxLifeSpan = duration;

        if (amount >= 1)
        {
            StartCoroutine(SpawnCar(spawnDelay, amount - 1, duration));
        }
    }
}
