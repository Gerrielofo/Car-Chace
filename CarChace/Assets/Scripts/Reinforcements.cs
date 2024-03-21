using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforcements : MonoBehaviour
{
    [SerializeField] GameObject _reinforcementPrefab;
    [SerializeField] Transform _spawnTransform;
    [SerializeField] float _spawnDelay;

    float amountToSpawn;

    public void SpawnReinforcements(float amount, float duration)
    {
        amountToSpawn = amount;
        StartCoroutine(SpawnCar(_spawnDelay, duration));
    }

    IEnumerator SpawnCar(float spawnDelay, float duration)
    {

        yield return new WaitForSeconds(spawnDelay);
        GameObject car = Instantiate(_reinforcementPrefab, _spawnTransform.position, _spawnTransform.rotation);
        car.GetComponent<PoliceAgentFollow>().maxLifeSpan = duration;
        amountToSpawn--;
        if (amountToSpawn > 0)
        {
            StartCoroutine(SpawnCar(_spawnDelay, duration));
        }
    }
}
