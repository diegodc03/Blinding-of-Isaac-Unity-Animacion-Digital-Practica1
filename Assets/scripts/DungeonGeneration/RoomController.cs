using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class RoomInfo
{
    public string name;

    public int X;

    public int Y;
}
public class RoomController : MonoBehaviour
{

    public Room HabitacionActual;

    public SpawnController spawnController; //Asignamos esto en el inspector3
    //public ItemsSpawner itemsSpawner; //Asignamos esto en el inspector

    private List<EnemyController> enemiesInRoom = new List<EnemyController>();

    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;


    bool ultimaHabitacionCargada = false;

    bool habitacionActualizadas = false;


    void Awake()
    {
        instance = this;
    }



    public void LoadRoom(string name, int x, int y)
    {
        //Si ya existe no se tiene que volver a crear
        if(DoesRoomExist(x, y))
        {
            return;
        }
        RoomInfo roomInfo = new RoomInfo(); ;
        roomInfo.name = name;
        roomInfo.X = x;
        roomInfo.Y = y;

        loadRoomQueue.Enqueue(roomInfo);
    }


    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {

        // Si no existe la habitacion se crea
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {

            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + " " + room.Y;


            // Configurar las puertas de la habitación
            foreach (Door door in room.doors)
            {
                switch (door.doorType)
                {
                    case Door.DoorType.top:
                        door.connectedRoomX = room.X;
                        door.connectedRoomY = room.Y + 1;
                        break;
                    case Door.DoorType.bottom:
                        door.connectedRoomX = room.X;
                        door.connectedRoomY = room.Y - 1;
                        break;
                    case Door.DoorType.left:
                        door.connectedRoomX = room.X - 1;
                        door.connectedRoomY = room.Y;
                        break;
                    case Door.DoorType.right:
                        door.connectedRoomX = room.X + 1;
                        door.connectedRoomY = room.Y;
                        break;
                }
            }

            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CamaraController.instance.habitacionActual = room;
            }

            loadedRooms.Add(room);

            // Quitamos la eliminacion de puertas no conectadas
            //room.EliminarPuertasNoConectadas();

        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }


        public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }



   

    // Update is called once per frame
    void Update()
    {
        UpdateRoomQueue();

        ///Debug.Log("Habitacion actual: " + HabitacionActual);
        if (HabitacionActual != null && !HabitacionActual.passed && !HabitacionActual.isSpawningItems)
        {
           // Debug.Log("Habitacion actual no pasada");
            // Solo inicia el spawn si es una habitación nueva y no se han generado enemigos aún
            spawnController.CheckAndSpawnEnemies(HabitacionActual);
            
        }
        
    }


    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
        }

        if(loadRoomQueue.Count == 0)
        {
            if (!ultimaHabitacionCargada)
            {
                StartCoroutine(CargarUltimaHabitacion());
            }else if(ultimaHabitacionCargada && !habitacionActualizadas)
            {
                foreach(Room room in loadedRooms)
                {    
                    room.EliminarPuertasNoConectadas();
                    
                }

                // Tenemos que comprobar que la inicial no esta junto a la final, ya yhemos comprobado la final
                Room habitacionInicial = loadedRooms[0];
                Room habitacionFinal = loadedRooms[loadedRooms.Count - 1];
                habitacionFinal.isFinalRoom = true;
                comprobarHabitacionFinalJuntoInicial(habitacionInicial, habitacionFinal);


                habitacionActualizadas = true;
            }   
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }


    void comprobarHabitacionFinalJuntoInicial(Room habitacionInicial, Room habitacionFinal)
    {
        int xfin = habitacionFinal.X;
        int yfin = habitacionFinal.Y;

        int xini = habitacionInicial.X;
        int yini = habitacionInicial.Y;

        if ((xini + 1 == xfin && yini == yfin) || // Derecha
            (xini - 1 == xfin && yini == yfin) || // Izquierda
            (xini == xfin && yini + 1 == yfin) || // Arriba
            (xini == xfin && yini - 1 == yfin))
        {

            //Comprobar que habitacion final solo tiene una actica
            habitacionFinal.isFinalRoom = true;
            // Cerrar las puertas correspondientes
            habitacionInicial.eliminarAcopleDePuerta(xfin, yfin);
            habitacionFinal.eliminarAcopleDePuerta(xini, yini);
        }
    }

    IEnumerator CargarUltimaHabitacion()
    {
        ultimaHabitacionCargada = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {

            Room ultimaHabitacion = loadedRooms[loadedRooms.Count - 1];

            //Con esto podemos saber si es la ultima habitacion
            

            Room tempRoom = new Room(ultimaHabitacion.X, ultimaHabitacion.Y);
            Destroy(ultimaHabitacion.gameObject);
                
            var habitacionAEliminar = loadedRooms.Single(item => item.X == tempRoom.X && item.Y == tempRoom.Y);
            loadedRooms.Remove(habitacionAEliminar);
            LoadRoom("End", tempRoom.X, tempRoom.Y); 
        }
        else
        {
            ultimaHabitacionCargada = false;
        }
    }   




    public void OnPlayerEnterRoom(Room room)
    {
        CamaraController.instance.habitacionActual = room;
        HabitacionActual = room;

        // Llamar a PlayerController para reiniciar sus datos
        PlayerController.instance.ResetPlayerData();  // Asumiendo que tienes una función para resetear los datos del jugador

        // Si la habitación no ha sido pasada, reiniciar el spawner
        if (!room.passed)
        {
            spawnController.ResetSpawnerState();
        }

    }


   



}
