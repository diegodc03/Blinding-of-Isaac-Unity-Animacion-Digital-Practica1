using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


[System.Serializable]

// Added missing attributes
public class CollectibleItem // Renamed to CollectibleItem
{
    public String name;
    public String description;
    public Sprite image;
}


public class SpawnerItemRoom : MonoBehaviour
{
    public int maxItemsToSpawn = 3;
    public int minItemsToSpawn = 1;
    public int itemsRandomToSpawn = 0;

    public List<CollectibleItem> items; // Updated to use the new class name

    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange; // Corrected typo (from atackSpeedChange)
    public float bulletSizeChange;

    public Transform[] spawnItems;
    private List<int> spawnedZoneUsed = new List<int>();



    public void Start()
    {
        Debug.Log("Estoy en Start de SpawnerItemRoom");


        itemsRandomToSpawn = Random.Range(minItemsToSpawn, maxItemsToSpawn);

        if(itemsRandomToSpawn > spawnItems.Length)
        {
            itemsRandomToSpawn = spawnItems.Length;
        }

        for (int i = 0; i < itemsRandomToSpawn; i++)
        {

            CollectibleItem item = items[Random.Range(0, items.Count)];
            bool existNum = false;
            while (!existNum)
            {
                int num = Random.Range(0, spawnItems.Length);
                if (!spawnedZoneUsed.Contains(num))
                {
                    Transform spawnPoint = spawnItems[num];
                    spawnedZoneUsed.Add(num);
                    existNum = true;

                    if (item != null) // Verificar si el item está asignado
                    {
                        // Acceder al objeto de spawn y cambiar el sprite
                        SpriteRenderer spriteRenderer = spawnPoint.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = item.image;
                        }

                        // Eliminar el collider antiguo y añadir uno nuevo
                        PolygonCollider2D polygonCollider = spawnPoint.GetComponent<PolygonCollider2D>();
                        if (polygonCollider != null)
                        {
                            Destroy(polygonCollider);
                        }

                        BoxCollider2D boxCollider = spawnPoint.GetComponent<BoxCollider2D>();
                        if (boxCollider == null)
                        {
                            boxCollider = spawnPoint.gameObject.AddComponent<BoxCollider2D>();
                        }
                        //boxCollider.isTrigger = true; // Asegurarse de que el collider sea un trigger
                    }
                

                }
            }

            
        }
    }
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Estoy en OnTriggerEnter2D de SpawnerItemRoom");
        if (collision.tag == "Player")
        {
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange); // Updated to match corrected variable name
            GameController.BulletSizeChange(bulletSizeChange);

            Destroy(gameObject);
        }
    }
}
