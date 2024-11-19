using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Prefab del enemigo que se va a instanciar
    public GameObject enemyPrefab;
    public GameObject enemyPrefab1;
    //public GameObject enemyPrefab2;
    // Lista de puntos de spawn
   


    // Para saber si la corrutina est� en ejecuci�n
    private bool isSpawning = false;

    public int currentEnemyCount = 0;


   


    public void StartSpawning(float spawnInterval, int maxEnemies, int minEnemies, int maxEnemiesRandom, Transform[] spawnPoints)
    {

        Debug.Log("Enemigos a spawnear en startSpawning"+maxEnemiesRandom);
        if (!isSpawning)  // Verifica si no se est� generando ya enemigos
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies(spawnInterval, maxEnemies, minEnemies, maxEnemiesRandom, spawnPoints));
            isSpawning = false;
            
        }
    }

    


    IEnumerator SpawnEnemies(float spawnInterval, int maxEnemies, int minEnemies, int maxEnemiesRandom, Transform[] spawnPoints)
    {
        Debug.Log("Enemigos a spawnear en spawnEnemies"+maxEnemiesRandom);
        Debug.Log("spawnPoints"+spawnPoints.Length);    
        if(spawnPoints.Length==0)
        {
            Debug.Log("No hay puntos de spawn");
            yield break;
        }
        // Mientras no se haya alcanzado el limite de enemigos
        while (currentEnemyCount < maxEnemiesRandom)
        {
            // Espera el tiempo de intervalo
            yield return new WaitForSeconds(spawnInterval);

            // Selecciona un punto de spawn aleatorio
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instancia el enemigo en el punto aleatorio
            if(currentEnemyCount%2==0)
            {
                
                Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);
                
            }
            else
            {
                Instantiate(enemyPrefab1, randomSpawnPoint.position, Quaternion.identity);
            }
            //Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            // Aumenta el contador de enemigos
            currentEnemyCount++;
        }
        currentEnemyCount = 0;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
