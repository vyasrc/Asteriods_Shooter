using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawing = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawing == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 3.7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
                 
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while(_stopSpawing == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 3.7f, 0);
            int randomPowerup = Random.Range(0, 2);
            Instantiate(powerups[randomPowerup], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawing = true;
    }
}


