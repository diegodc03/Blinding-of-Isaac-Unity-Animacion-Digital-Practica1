using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private static float health = 6.0f;

    private static int maxHealth = 6;

    private static float moveSpeed = 5.0f;

    private static float fireRate = 1f;

    private static float bulletSize = 0.5f;




    /// <summary>
    /// ////////////////////////////////GETTERS AND SETTERS
    /// </summary>
    public static float Health
    {
        get => health;
        set => health = value;
    }

    public static int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public static float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public static float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    public static float BulletSize
    {
        get => bulletSize;
        set => bulletSize = value;
    }




    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el objeto se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya hay una instancia, destruye la nueva
        }
    }




    // Update is called once per frame
    void Update()
    {
        //healthText.text = "Health:" + health;
    }



    public static void DamagePlayer(int damage)
    {
        health -= damage;
        if (Health <= 0)
        {
            KillPlayer();
        }

    }


    public static void HealPlayer(float healAmount)
    {
        Health = Mathf.Min(maxHealth, Health + healAmount);
    }


    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }


    public static void FireRateChange(float rate)
    {
        fireRate += rate;
    }
   


    public static void KillPlayer()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        SceneManager.LoadScene("MenuPrincipal");
    }


    public void ResetGameData()
    {
        health = 6.0f;
        moveSpeed = 5.0f;
        fireRate = 1f;
        bulletSize = 0.5f;
    }


}
