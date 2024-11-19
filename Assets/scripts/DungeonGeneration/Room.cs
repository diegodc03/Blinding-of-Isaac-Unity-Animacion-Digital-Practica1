using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int X;
    public int Y;

    public Boolean passed = false;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    private bool actualizarPuertas = false;

    public bool isFinalRoom = false;

    public Transform[] spawnPoints; 


    //public ItemsSpawner itemsSpawner; //Asignamos esto en el inspector
    public bool isSpawningItems = false;

    public SpawnController spawnController; //Asignamos esto en el inspector
    public EnemyController enemyController; //Asignamos esto en el inspector

    public GameObject topWallCollider;
    public GameObject bottomWallCollider;
    public GameObject leftWallCollider;
    public GameObject rightWallCollider;



    public int numberOfObjectsRandomPerRoom;

    // Agregamos las puertas a una lista para facilitar su gestión
   

    //Constructor
    public Room(int x, int y)
    {
        X = x;
        Y = y;
        
    }

    public List<Door> doors = new List<Door>();

    public List<Door> activatedDoors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        // Inicializamos la lista de puertas y las asignamos
        GetSpawnPoints();

        if (RoomController.instance == null)
        {
            Debug.Log("RoomController not found");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
            }   
        }
       
        RoomController.instance.RegisterRoom(this);
    }


    public void GetSpawnPoints()
    {
        Debug.Log("Buscando puntos de spawn items en abitacion actual");
        // Buscar los puntos de spawn en la habitación actual
        // Buscar los puntos de spawn en la habitación actual
        this.spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoints")
            .Where(go => go.transform.IsChildOf(transform)) // Asegurarse de que están dentro de esta habitación
            .Select(go => go.transform)
            .ToArray();

        Debug.Log("Spawn points en esta habitación: " + spawnPoints.Length);
    }


    //Habria que buscar el metodo para que si no hay una habitacion conectada, se elimine la puerta
    // pero si hay una habitacion conectada, se mantenga la puerta, si se ha quitado la puerta, se vuelva a introducir
    public void EliminarPuertasNoConectadas()
    {


        foreach (Door d in doors)
        {
            switch(d.doorType)
            {
              
                case Door.DoorType.top:
                    if (!RoomController.instance.DoesRoomExist(X, Y + 1))
                    {
                        topWallCollider.SetActive(true);
                        d.gameObject.SetActive(false);
                    }
                    else
                    {
                        activatedDoors.Add(d);
                    }
                    break;
                case Door.DoorType.bottom:
                    if (!RoomController.instance.DoesRoomExist(X, Y - 1))
                    {
                        bottomWallCollider.SetActive(true);
                        d.gameObject.SetActive(false);
                    }
                    else
                    {
                        activatedDoors.Add(d);
                    }
                    break;
                case Door.DoorType.left:
                    if (!RoomController.instance.DoesRoomExist(X - 1, Y))
                    {
                        leftWallCollider.SetActive(true);
                        d.gameObject.SetActive(false);
                    }
                    else
                    {
                        activatedDoors.Add(d);
                    }
                    break;
                case Door.DoorType.right:
                    if (!RoomController.instance.DoesRoomExist(X + 1, Y))
                    {
                        rightWallCollider.SetActive(true);
                        d.gameObject.SetActive(false);
                    }
                    else
                    {
                        activatedDoors.Add(d);
                    }
                    break;
            }
        }
    }

    // X e Y son las coordenadas de la sala a la que se va a desconectar de la puerta
    public void eliminarAcopleDePuerta(int X, int Y)
    {
        if(RoomController.instance.DoesRoomExist(X, Y))
        {
            foreach (Door d in doors)
            {
                // Si la puerta conecta con las coordenadas dadas, desactivarla
                if (d.connectedRoomX == X && d.connectedRoomY == Y )
                {
                    d.gameObject.SetActive(false);
                }
            }
        }
    }


    //Queremos que unicamente haya una entrada a la sala final para que así sea obligatorio pasar por todas las salas, si en algun momento estan muchas salas juntas
    //, se eliminan las puertas de las salas que no esten conectadas a la sala final menos una.
    public void EliminarPuertasUltimaSala()
    {
        //Establecesmos un booleano para que si en algun momento se activa, no se vuelva a activar y se salga del bucle ya que las demas puertas tienen que estar eliminadas
        bool puertaEncontrada = false;

        foreach (Door d in doors)
        {
            //Debug.Log("Eliminando puertas no conectadas de la ultima sala dentro del foreach y la variable puerta encontrada" + puertaEncontrada);
            switch (d.doorType)
            {
                case Door.DoorType.top:
                    if ((!RoomController.instance.DoesRoomExist(X, Y + 1) || puertaEncontrada) )
                    {
                        
                        d.gameObject.SetActive(false);
                        topWallCollider.SetActive(true);
                    }
                    else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.bottom:
                    if ((!RoomController.instance.DoesRoomExist(X, Y - 1) || puertaEncontrada) )
                    {
                       
                        d.gameObject.SetActive(false);
                        bottomWallCollider.SetActive(true);

                    }
                    else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.left:
                    if ((!RoomController.instance.DoesRoomExist(X - 1, Y) || puertaEncontrada) )
                    {
                        
                        d.gameObject.SetActive(false);
                        leftWallCollider.SetActive(true);
                    }
                    else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.right:
                    if ((!RoomController.instance.DoesRoomExist(X + 1, Y) || puertaEncontrada))
                    {
                        
                        d.gameObject.SetActive(false);
                        rightWallCollider.SetActive(true);
                    }
                    else{
                        puertaEncontrada = true;
                    }
                    break;
            }
        }
        //actualizarPuertas = true;
    }



    // Update is called once per frame
    void Update()
    {
        
        if(name.Contains("End") && !actualizarPuertas)
        {
            EliminarPuertasUltimaSala();
            actualizarPuertas = true;
        }
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetRoomCentre(), new Vector3(Width, Height, 0));
    }
    


    public Vector3 GetRoomCentre()
    {
        return new Vector3(X * Width, Y * Height);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Estoy en com,probacion ultima sala" + isFinalRoom);
        // Si es la habitacion final, llamar a la funcion de la habitacion final donde se esperara 5 segundos y se cargara la escena de menu principal
        if (isFinalRoom && collision.tag == "Player")
        {
            Debug.Log("Habitación final alcanzada. Esperando 5 segundos...");
            StartCoroutine(FinalRoom());
        }


        if (collision.tag == "Player" && !isSpawningItems)
        {
            RoomController.instance.OnPlayerEnterRoom(this);
            Debug.Log("Player entered room on Rool Class" + X + ", " + Y);

            
            if(enemyController != null)
            {
                enemyController.OnPlayerEnterRoom();
            }

        }
    }


    private IEnumerator FinalRoom()
    {
        Debug.Log("Habitación final alcanzada. Esperando 5 segundos...");

        // Espera 5 segundos
        yield return new WaitForSeconds(5f);

        // Cargar la escena del menú principal
        SceneManager.LoadScene("MenuPrincipal"); // Reemplaza "MainMenu" con el nombre exacto de tu escena
    }




    //En esta funcion vamos a hacer que si los enemigos han entrado en la sala, se cierren las puertas
    public void activarPuertas()
    {
        if(passed == false)
        {
            //Actiamos las puertas
            foreach(Door puerta in doors)
            {
                if(puerta.gameObject == true)
                {
    
                    puerta.CerrarPuerta();
                }
            }
        }
    }


    //En estas funciones vamos a hacer que si se han eliminado todos los enemigos de la sala, se abran las puertas
    public void desactivarPuertas()
    {
        
        if(passed == true)
        {
            foreach(Door puerta in doors)
            {

                if(puerta.gameObject == true)
                {
 
                    puerta.AbrirPuerta();
                }
            }
        }
    }




}
