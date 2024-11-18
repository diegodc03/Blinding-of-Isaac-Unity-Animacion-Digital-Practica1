using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]


public class CollectionController : MonoBehaviour // Corrected typo in the class name
{
    public CollectibleItem collectibleItem; // Updated to use the new class name
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange; // Corrected typo (from atackSpeedChange)
    public float bulletSizeChange;

    public void StartItem(CollectibleItem item)
    {
        // Inicializamos el sprite y el collider en el objeto
        collectibleItem = item; // Guardamos el item asignado
        GetComponent<SpriteRenderer>().sprite = item.image;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
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
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange); // Updated to match corrected variable name
            GameController.BulletSizeChange(bulletSizeChange);

            Destroy(gameObject);
        }
    }
}
