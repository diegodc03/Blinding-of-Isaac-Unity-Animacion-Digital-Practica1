using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    
    Rigidbody2D rigidbody;
    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;

    public float bulletSpeed;

    private float lastfire;

    public float fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        //Coge la referencia de la aplicacion
        rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame  --> se llama una vez por frame
    void Update()
    {
        //// Paara cuando cambiamos el valor
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");

        if ((shootHor != 0 || shootVert != 0) && Time.time > lastfire + fireDelay) {
            Shoot(shootHor, shootVert);
            lastfire = Time.time;
        }
        

        rigidbody.velocity = new Vector3 (horizontal * speed, vertical * speed, 0);
        collectedText.text = "Item Collected: " + collectedAmount;
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;            // Si quiero añadir potenciadores a lo mejor la gravedad tendria que cambiar
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3 (
            (x<0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
            );
    }


}
