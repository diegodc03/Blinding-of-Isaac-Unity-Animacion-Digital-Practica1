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

    public enemyState currState = enemyState.Wander;

    public EnemyType enemyType;

    public float range;

    public float speed;

    public float coolDown;

    private bool chooseDir = false;

    private bool dead = false;

    private bool coolDownAtack = false;

    private Vector3 randomDir;

    public float atackRange;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnController = FindObjectOfType<SpawnController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (enemyState.Wander):
                Wander();
                break;
            case (enemyState.Follow):
                Follow();
                break;
            case (enemyState.Die):
                break;
            case (enemyState.Atack):
                Atack();
                break;

        }

        if (IsPlayerInRange(range) && currState != enemyState.Die) {
            currState = enemyState.Follow;
        } else if (!IsPlayerInRange(range) && currState != enemyState.Die) {
            currState = enemyState.Wander;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= atackRange)
        {
            currState = enemyState.Atack;
        }

    }

    private bool IsPlayerInRange(float range)
    {
        // posicion del elemento, posicion del jugador
        return Vector3.Distance(transform.position, player.transform.position) <= range;
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
        // Lo movemos de la posicion actual a la posicion del jugador , y como esto se hace por frame, se ira moviendo poco a poco en direccion al jugador
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        Debug.Log("Enemigo muerto");
        spawnController.DecrementarEnemigosRestantes();
        Destroy(gameObject);
    }

}
