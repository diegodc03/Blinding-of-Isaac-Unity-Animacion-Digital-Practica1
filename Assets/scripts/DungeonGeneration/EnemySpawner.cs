using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Prefab del enemigo que se va a instanciar
    public GameObject enemyPrefab;
    // Lista de puntos de spawn
    public Transform[] spawnPoints;


    // Para saber si la corrutina está en ejecución
    private bool isSpawning = false;

    public int currentEnemyCount = 0;


    


    public void StartSpawning(float spawnInterval, int maxEnemies, int minEnemies, int maxEnemiesRandom)
    {

        Debug.Log("Enemigos a spawnear en startSpawning"+maxEnemiesRandom);
        if (!isSpawning)  // Verifica si no se está generando ya enemigos
        {
            StartCoroutine(SpawnEnemies(spawnInterval, maxEnemies, minEnemies, maxEnemiesRandom));
            isSpawning = true;
        }
    }

    


    IEnumerator SpawnEnemies(float spawnInterval, int maxEnemies, int minEnemies, int maxEnemiesRandom)
    {
        Debug.Log("Enemigos a spawnear en spawnEnemies"+maxEnemiesRandom);

        // Mientras no se haya alcanzado el limite de enemigos
        while (currentEnemyCount < maxEnemiesRandom)
        {
            // Espera el tiempo de intervalo
            yield return new WaitForSeconds(spawnInterval);

            // Selecciona un punto de spawn aleatorio
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instancia el enemigo en el punto aleatorio
            Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            // Aumenta el contador de enemigos
            currentEnemyCount++;
        }
    }   



    // Update is called once per frame
    void Update()
    {
        
    }
}
