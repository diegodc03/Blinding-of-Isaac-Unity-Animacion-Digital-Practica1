using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemASpawnear : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject itemPotion;
    public GameObject itemBoot;
    public GameObject itemBulletSpeed;


    void Start()
    {
        int random = Random.Range(0, 6);
        if(random == 0)
        {

            itemPotion.SetActive(true);
        }if(random == 1)
        {
            itemBoot.SetActive(true);
        }else if(random == 2)
        {
            itemBulletSpeed.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
