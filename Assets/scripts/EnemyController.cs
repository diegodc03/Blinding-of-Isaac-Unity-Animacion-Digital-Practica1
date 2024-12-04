using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyState
{
    Wander, 
    Follow,
    Die, 
    Atack
};


public enum EnemyType
{
    Melee,
    Ranged
};



public class EnemyController : MonoBehaviour
{

    GameObject player;

    //para poder llamar a este
    private SpawnController spawnController;

    private Room currentRoom;   // Referencia a la habitación en la que se encuentra el enemigo


    public enemyState currState = enemyState.Follow;

    public EnemyType enemyType;

    public float range = 50;

    public float speed = 2;

    public float coolDown = 1;

    private bool chooseDir = false;

    private bool coolDownAtack = false;

    private Vector3 randomDir;

    public float atackRange = 5;

    public GameObject bulletPrefab;

    // Referencia al RoomController (para saber en qué habitación está el enemigo)
    private RoomController roomController;


    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        spawnController = FindObjectOfType<SpawnController>();
        currentRoom = FindObjectOfType<RoomController>().HabitacionActual;  // Asigna la habitación actual del RoomController

        speed = Random.Range(2f, 5.5f);
        coolDown = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            switch (currState)
            {
                
                case (enemyState.Follow):
                    Follow();
                    break;
                case (enemyState.Die):
                    break;
                case (enemyState.Atack):
                    Atack();
                    break;

            }

            if (IsPlayerInRange(range) && currState != enemyState.Die)
            {
                currState = enemyState.Follow;
            }
            


            if (Vector3.Distance(transform.position, player.transform.position) <= atackRange)
            {
                currState = enemyState.Atack;
            }
        }

    }




    public void OnPlayerEnterRoom()
    {

  
        spawnController = FindObjectOfType<SpawnController>();
        currentRoom = FindObjectOfType<RoomController>().HabitacionActual;  // Actualiza la habitación cuando el jugador entra

        // Cambiamos la velocidad dependiendo de la habitaciónse usara un random
        
    }


    private bool IsPlayerInRange(float range)
    {
        if(player != null)
        {
            // posicion del elemento, posicion del jugador
            return Vector3.Distance(transform.position, player.transform.position) <= range;
        }
        else
        {
            return false;
        }
        
    }


    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currState = enemyState.Follow;
        }

    }

    

    void Follow()
    {
        if (player != null)
        {
            // Lo movemos de la posicion actual a la posicion del jugador , y como esto se hace por frame, se ira moviendo poco a poco en direccion al jugador
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void Atack()
    {


        if(!coolDownAtack)
        {
            //GameController.DamagePlayer(1);
            //StartCoroutine(CoolDown());
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<bulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<bulletController>().isEnemyBullet = true;


                    // Obtener la dirección hacia el jugador
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    // Rotar la bala hacia la dirección de disparo
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convertir a grados
                    bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

                    StartCoroutine(CoolDown());
                    break;



                    

            }
        }
    }


    private IEnumerator CoolDown()
    {
        coolDownAtack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAtack = false;
    }


    public void Death()
    { 
        spawnController.DecrementarEnemigosRestantes(currentRoom);
        Destroy(gameObject);
    }

}
