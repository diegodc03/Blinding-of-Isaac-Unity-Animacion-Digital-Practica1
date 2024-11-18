using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;






public class ItemsSpawner : MonoBehaviour
{
    public List<CollectibleItem> items = new List<CollectibleItem>();

    private int itemsSpawned= 0;
    private List<int> spawnedZoneUsed = new List<int>();

    public int maxItemsToSpawn = 2;
    public int minItemsToSpawn = 1;
    public int ItemsRandomToSpawn = 0;



    // Referencia al CollecionController para controlar el spawn
    public CollectionController ColletionController;



    public void aniadirItemsALaRoom(Room habitacionActual)
    {
        Debug.Log("Estoy en aniadirItemsALaRoom");
        // Si la habitación es la inicial, no se spawnearán items
        if (SceneManager.GetActiveScene().name == "BasementStart" || (habitacionActual.X == 0 && habitacionActual.Y == 0))
        {
            return;
        }

        
        // Si la habitación no ha sido pasada, se spawnearán items
        if (!habitacionActual.passed && habitacionActual != null && !habitacionActual.isSpawningItems)
        {
            
            ItemsRandomToSpawn = Random.Range(minItemsToSpawn, maxItemsToSpawn);
            Debug.Log("Items a spawnear en aniadirItemsALaRoom" + ItemsRandomToSpawn);
            StartCoroutine(SpawnItems(1f, ItemsRandomToSpawn, habitacionActual.spawnItems, habitacionActual.passed));

        }
    }  
    

    IEnumerator SpawnItems(float spawnInterval, int itemsRandomToSpawn, Transform[] spawnItems, bool passed)
    {
        Debug.Log("Items a spawnear en spawnItems" + itemsRandomToSpawn);
        Debug.Log("spawnPoints" + spawnItems.Length);
        

        // Mientras no se haya alcanzado el limite de items
        while (itemsSpawned < itemsRandomToSpawn && !passed)
        {
            // Espera el tiempo de intervalo
            yield return new WaitForSeconds(spawnInterval);

            bool spawnedZone = false;
            while(spawnedZone == false)
            {
                // Selecciona un punto de spawn aleatorio
                int num = Random.Range(0, spawnItems.Length);
                spawnedZoneUsed.Add(num);
                if (!spawnedZoneUsed.Contains(num))
                {
                    // Selecciona un item aleatorio de la lista
                    int randomIndex = Random.Range(0, items.Count);
                    CollectibleItem randomItem = items[randomIndex];


                    spawnedZone = true;
                    Transform randomSpawnPoint = spawnItems[num];

                    // Instancia el item en el punto aleatorio
                    GameObject collectibleObject = Instantiate(ColletionController.gameObject, randomSpawnPoint.position, Quaternion.identity);

                    // Asigna el item al CollectionController
                    CollectionController controller = collectibleObject.GetComponent<CollectionController>();
                    if (controller != null)
                    {
                        controller.item = randomItem; // Asignamos el CollectibleItem al CollectionController
                        controller.Start(); // Llamar al Start() para que se inicialice
                    }


                    // Aumenta el contador de items
                    itemsSpawned++;
                }
            }
            
            


            
        }
        itemsSpawned = 0;
    }

}
