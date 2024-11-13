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
        // Aseg�rate de que enemySpawner est� asignado en el Inspector
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner no est� asignado en el SpawnController");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Solo spawnear enemigos si estamos en una nueva habitaci�n y no se han generado ya
        if (habitacionActual != null && !enemiesSpawned)
        {
            SpawnearEnemigosEnHabitacionActual();
        }
    }


    public void SpawnearEnemigosEnHabitacionActual()
    {
        if (habitacionActual != null && enemySpawner != null)
        {
            // Llamamos al m�todo de spawn del EnemySpawner solo cuando entras en una nueva habitaci�n
            enemySpawner.StartSpawning();

            // Marcar que ya se generaron los enemigos en esta habitaci�n
            enemiesSpawned = true;
        }


    }



    // Detectamos cuando el jugador entra en la habitaci�n (trigger)
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el jugador entra en la habitaci�n
        if (other.CompareTag("Player"))
        {
            // Cambiar la habitaci�n actual al que corresponde
            // Este c�digo depende de tu l�gica para asignar la habitaci�n actual.+
            // eSTO ES COGER LA HABITACION ACTUAL DEL JUGADOR Y PASARLA A LA HABITACION ACTUAL DEL SPAWNCONTROLLER
            // HABRA QUE TENER UN ARRAY DE HABITACIONES Y UNA VARIABLE QUE SEA LA HABITACION ACTUAL PARA QYUE SE PUEDA PONER EN QYUE HABITACION YA HA PELEADO EL JUGAROR
            habitacionActual = other.GetComponent<PlayerController>().currentRoom;    //fodsujdfjosungghhp��kikisgNSIOPOPGGK�KUJNUJSPIGHKJUNDDNJP

            // Resetear el estado para permitir que se generen enemigos en esta nueva habitaci�n
            enemiesSpawned = false;
        }
    }



}
