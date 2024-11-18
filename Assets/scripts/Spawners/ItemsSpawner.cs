using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;



[CreateAssetMenu(fileName = "New Collectible Item", menuName = "Collectible Item")]

public class CollectibleItem : ScriptableObject // Renamed to CollectibleItem
{
    public String name;
    public String description;
    public Sprite image;
}


public class ItemsSpawner : MonoBehaviour
{
    public List<int> itemSpawned = new List<int>();

    public int maxItemsToSpawn = 2;
    public int minItemsToSpawn = 1;
    public int ItemsRandomToSpawn = 0;

    public List<CollectibleItem> itemsToSpawn = new List<CollectibleItem>();


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
        if (!habitacionActual.passed && habitacionActual != null)
        {
            Debug.Log("he entrado sisisis");
            ItemsRandomToSpawn = Random.Range(minItemsToSpawn, maxItemsToSpawn);

            for(int i=0; i<ItemsRandomToSpawn; i++)
            {
                bool itemSpawnedBool = false;
                while (!itemSpawnedBool)
                {

                    // Seleccionamos un item aleatorio de la lista de items
                    CollectibleItem item = itemsToSpawn[Random.Range(0, itemsToSpawn.Count)];

                    int spawnElement = Random.Range(0, habitacionActual.spawnItems.Length);
                    if (habitacionActual.spawnItems.Length == 0)
                    {
                        Debug.Log("No hay spawnItems en la habitación");
                        
                    }

                    if (itemSpawned.Contains(spawnElement))
                    {
                        Debug.Log("Ya se ha spawnado un item en esta posición");
                        
                    }
                    else
                    {

                        itemSpawned.Add(spawnElement);
                        itemSpawnedBool = true;
                        Transform spawnPosition = habitacionActual.spawnItems[spawnElement];


                        // Creamos un nuevo item
                        // Instanciamos el prefab con el script CollectionController
                        GameObject collectibleObject = Instantiate(ColletionController.gameObject, spawnPosition.position, Quaternion.identity);

                        CollectionController controller = collectibleObject.GetComponent<CollectionController>();
                        if (controller != null)
                        {
                            controller.collectibleItem = item;
                            controller.StartItem(item);
                        }
                    }
                }
            }
        }
    }   
}
