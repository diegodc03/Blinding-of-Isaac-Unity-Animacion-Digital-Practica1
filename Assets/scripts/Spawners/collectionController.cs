using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[System.Serializable]

// Added missing attributes
public class CollectibleItem // Renamed to CollectibleItem
{
    public String name;
    public String description;
    public Sprite image;
}

public class CollectionController : MonoBehaviour // Corrected typo in the class name
{

    private PlayerController playerController;

    public CollectibleItem item; // Updated to use the new class name
    

    private float healthChange;
    private float moveSpeedChange = 0;

    private float bulletSpeed = 0; // Corrected typo (from atackSpeedChange)
    private float fireDelayChange = 0.5f; // Corrected typo (from fireDelayChange)



    public void Start()
    {
      
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if(item.name == "BulletSpeed" && playerController != null)
        {
            bulletSpeed = Random.Range(10f, 20f);
            fireDelayChange = Random.Range(0.1f, 0.5f);

        }
        else if(item.name == "Boot" && playerController != null)
        {
            moveSpeedChange = Random.Range(5f, 15f);
    
        }
        else
        {
            healthChange = Random.Range(1, 4);

        }
        
     


        if (item != null) // Verificar si el item está asignado
        {
            GetComponent<SpriteRenderer>().sprite = item.image;
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.collectedAmount++;

            GameController.HealPlayer(healthChange);
            //GameController.MoveSpeedChange(moveSpeedChange);
            //GameController.FireRateChange(playerController.fireDelay); // Updated to match corrected variable name
            
            playerController.speed += moveSpeedChange;
            playerController.bulletSpeed += bulletSpeed;
            playerController.fireDelay = fireDelayChange;
            

            Destroy(gameObject);
        }
    }
}

