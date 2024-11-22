using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{


    // Referencia al EnemySpawner para controlar el spawn
    public EnemySpawner enemySpawner;


    private bool enemiesSpawned = false;

    // Tiempo entre los spawns
    public float spawnInterval = 1f;

    // Cantidad de enemigos a spawnear
    public int maxEnemies = 20;
    public int minEnemies = 5;
    public int maxEnemiesRandom=0;
    
    // Contador de enemigos actuales
    private int enemigosEliminados = 0;

    private bool puertasActivadas = false;


    



    // Verifica si es necesario spawnear enemigos
    public void CheckAndSpawnEnemies(Room habitacionActual)
    {

           ///Debug.Log("Estoy en la habitacion" + habitacionActual.X + ", " + habitacionActual.Y);
        if(SceneManager.GetActiveScene().name == "BasementStart" || (habitacionActual.X == 0 && habitacionActual.Y == 0) || SceneManager.GetActiveScene().name == "BasementEnd")
        {

            return;
        }
        
        if (!enemiesSpawned && habitacionActual != null)
        {
            // Activar puertas si es necesario
            if (!puertasActivadas )
            {
      
                habitacionActual.activarPuertas();
                puertasActivadas = true;
            }
            SpawnearEnemigos(habitacionActual);
        }
    }


    // Llama al método de spawn cuando es necesario
    private void SpawnearEnemigos(Room habitacionActual)
    {

        // Establece un número aleatorio de enemigos a spawnear
        maxEnemiesRandom = Random.Range(minEnemies, maxEnemies);
        if(spawnInterval == 0)
        {
            spawnInterval = Random.Range(0.8f, 3f);
        }
        
        enemigosEliminados = 0;
        habitacionActual.GetSpawnPoints();

        // Llama al método de spawn del EnemySpawner
        enemySpawner.StartSpawning(spawnInterval, maxEnemies, minEnemies, maxEnemiesRandom, habitacionActual.spawnPoints);

        // Marca que los enemigos ya fueron spawneados
        enemiesSpawned = true;
        habitacionActual.passed = true;
        
    }


    // Resetea los estados de spawn al entrar en una nueva habitación
    public void ResetSpawnerState()
    {
        enemiesSpawned = false;
        enemigosEliminados = 0;
        maxEnemiesRandom = 0;
        puertasActivadas = false;
    }


  


    public void DecrementarEnemigosRestantes(Room habitacionActual)
    {
        this.enemigosEliminados = this.enemigosEliminados + 1;

        // Comprobar si todos los enemigos han sido derrotados
        if (this.enemigosEliminados >= this.maxEnemiesRandom )
        {
            // Aquí puedes activar algo en la habitación, como abrir una puerta
            habitacionActual.passed = true;
            this.enemigosEliminados = 0;
  
            habitacionActual.desactivarPuertas();
     
        }  
    }
}
