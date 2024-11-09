using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public String name;
    public String description;
    public Sprite itemImage;
}



public class CollecionController : MonoBehaviour
{

    public Item item;
    public float healthChange;
    public float speedChange;
    public float moveSpeedChange;
    public float atackSpeedChange;
    public float bulletSizeChange;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
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
            GameController.FireRateChange(atackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);

            Destroy(gameObject);
        }
    }

}
