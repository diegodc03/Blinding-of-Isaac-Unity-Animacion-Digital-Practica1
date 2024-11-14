using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Habitacion actual donde se encuentra el personaje
    public Room habitacionActual;

    // Referencia al EnemySpawner para controlar el spawn
    public EnemySpawner enemySpawner;

    private bool enemiesSpawned = false;


    public float spawnInterval = 3f;

    // Cantidad de enemigos a spawnear
    public int maxEnemies = 50;
    public int minEnemies = 5;

    public int maxEnemiesRandom;

    
    // Contador de enemigos actuales
    private int enemigosEliminados = 0;


    private bool puertasActivadas = false;



    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que enemySpawner está asignado en el Inspector
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner no está asignado en el SpawnController");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
        
        // Solo spawnear enemigos si estamos en una nueva habitación y no se han generado ya
        if (habitacionActual != null && !enemiesSpawned && habitacionActual.passed == false)
        {
            if(habitacionActual != null && habitacionActual.passed == false)
            {
                if (!puertasActivadas)
                {
                    habitacionActual.activarPuertas();
                    puertasActivadas = true;
                }
                
                SpawnearEnemigosEnHabitacionActual();
            }
            
        }
    }


    public void SpawnearEnemigosEnHabitacionActual()
    {   
        
        if (habitacionActual != null && enemySpawner != null)
        {
            // Llamamos al método de spawn del EnemySpawner solo cuando entras en una nueva habitación

            // Selecciona un numero aleatorio de enemigos a spawnear
            this.maxEnemiesRandom = Random.Range(minEnemies, maxEnemies);
            Debug.Log("Elemigos a spawnear: " + maxEnemiesRandom);


            enemySpawner.StartSpawning(spawnInterval, maxEnemies, minEnemies, maxEnemiesRandom);

            // Marcar que ya se generaron los enemigos en esta habitación
            enemiesSpawned = true;
        }


    }


    //Funcion que se llama desde Enter Room
    public void OnPlayerEnterRoom(Room room)
    {
        habitacionActual = room; // Actualiza la habitación actual
        enemiesSpawned = false; // Resetea el estado de enemigos generados
        
    }


    public void DecrementarEnemigosRestantes()
    {
        enemigosEliminados++;
        Debug.Log("Enemigos Totales: " + this.maxEnemiesRandom);
        Debug.Log("Enemigos restantes: " + enemigosEliminados);
        // Comprobar si todos los enemigos han sido derrotados
        if (enemigosEliminados >= this.maxEnemiesRandom)
        {

            Debug.Log("Estoy en la habitacion" + habitacionActual.X + ", " + habitacionActual.Y);

            Debug.Log("Todos los enemigos han sido derrotados. Puedes avanzar.");
            // Aquí puedes activar algo en la habitación, como abrir una puerta
            habitacionActual.passed = true;
            enemigosEliminados = 0;
            habitacionActual.desactivarPuertas();
        }
    }


}
