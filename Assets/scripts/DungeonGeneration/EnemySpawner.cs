using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Prefab del enemigo que se va a instanciar
    public GameObject enemyPrefab;
    // Lista de puntos de spawn
    public Transform[] spawnPoints;
    // Tiempo entre spawns
    public float spawnInterval = 3f;

    // Cantidad de enemigos a spawnear
    public int maxEnemies = 50;
    public int minEnemies = 5;

    public int maxEnemiesRandom;

    // Contador de enemigos actuales
    private int currentEnemyCount = 0;

    // Para saber si la corrutina está en ejecución
    private bool isSpawning = false;



    


    public void StartSpawning()
    {
        if (!isSpawning)  // Verifica si no se está generando ya enemigos
        {
            StartCoroutine(SpawnEnemies());
            isSpawning = true;
        }
    }


    IEnumerator SpawnEnemies()
    {
        // Selecciona un numero aleatorio de enemigos a spawnear
        maxEnemiesRandom = Random.Range(minEnemies, maxEnemies);

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
