using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float speed = 5;
    
    Rigidbody2D rigidbody1;

    public Text collectedText;
    public static int collectedAmount = 0;

    public GameObject bulletPrefab;

    public float bulletSpeed = 10;

    private float lastfire;

    public float fireDelay = 5;

    //Hace referencia al animator del jugaodor
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Coge la referencia de la aplicacion
        rigidbody1 = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame  --> se llama una vez por frame
    void Update()
    {
        //// Paara cuando cambiamos el valor
        //fireDelay = GameController.FireRate;
        //speed = GameController.MoveSpeed;

        
        //Coge el valor de los ejes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal == 0 && vertical != 0)
        {
            animator.SetFloat("movimiento", 1* vertical * speed);
        }else if(vertical == 0 && horizontal != 0)
        {
         
            animator.SetFloat("movimiento", 1*horizontal * speed);
        }
        else if(horizontal != 0 && vertical != 0)
        {

            animator.SetFloat("movimiento", vertical * horizontal * speed);
        }

        if(vertical > 0 || horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if(vertical<0 || horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        
       

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");

        if ((shootHor != 0 || shootVert != 0) && Time.time > lastfire + fireDelay) {
            
            Shoot(shootHor, shootVert);
            lastfire = Time.time;
        }
        

        rigidbody1.velocity = new Vector3 (horizontal * speed, vertical * speed, 0);
        collectedText.text = "Item Collected: " + collectedAmount;
    }


    void Awake()
    {
        instance = this;
    }



    void Shoot(float x, float y)
    {
        if(x == 0 && y == 0)
        {
            return;
        }

        // Calcular la dirección normalizada
        Vector2 direction = new Vector2(x, y).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;            // Si quiero añadir potenciadores a lo mejor la gravedad tendria que cambiar
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3 (
            (x<0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
            );

        // Rotar la bala hacia la dirección de disparo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convertir a grados
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void ResetPlayerData()
    {
        bulletSpeed = 10;
        fireDelay = 0.5f;
        speed = 10.0f;
    }


 

}
