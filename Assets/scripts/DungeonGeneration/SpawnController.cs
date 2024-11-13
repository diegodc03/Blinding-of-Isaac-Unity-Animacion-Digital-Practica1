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
        if (habitacionActual != null && !enemiesSpawned)
        {
            SpawnearEnemigosEnHabitacionActual();
        }
    }


    public void SpawnearEnemigosEnHabitacionActual()
    {
        if (habitacionActual != null && enemySpawner != null)
        {
            // Llamamos al método de spawn del EnemySpawner solo cuando entras en una nueva habitación
            enemySpawner.StartSpawning();

            // Marcar que ya se generaron los enemigos en esta habitación
            enemiesSpawned = true;
        }


    }



    // Detectamos cuando el jugador entra en la habitación (trigger)
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el jugador entra en la habitación
        if (other.CompareTag("Player"))
        {
            // Cambiar la habitación actual al que corresponde
            // Este código depende de tu lógica para asignar la habitación actual.+
            // eSTO ES COGER LA HABITACION ACTUAL DEL JUGADOR Y PASARLA A LA HABITACION ACTUAL DEL SPAWNCONTROLLER
            // HABRA QUE TENER UN ARRAY DE HABITACIONES Y UNA VARIABLE QUE SEA LA HABITACION ACTUAL PARA QYUE SE PUEDA PONER EN QYUE HABITACION YA HA PELEADO EL JUGAROR
            habitacionActual = other.GetComponent<PlayerController>().currentRoom;    //fodsujdfjosungghhpññkikisgNSIOPOPGGKÑKUJNUJSPIGHKJUNDDNJP

            // Resetear el estado para permitir que se generen enemigos en esta nueva habitación
            enemiesSpawned = false;
        }
    }



}
