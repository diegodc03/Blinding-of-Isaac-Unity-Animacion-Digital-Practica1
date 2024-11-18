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
    public ItemsSpawner itemsSpawner; //Asignamos esto en el inspector

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
        if (HabitacionActual != null && !HabitacionActual.passed)
        {
           // Debug.Log("Habitacion actual no pasada");
            // Solo inicia el spawn si es una habitación nueva y no se han generado enemigos aún
            spawnController.CheckAndSpawnEnemies(HabitacionActual);
            itemsSpawner.aniadirItemsALaRoom(HabitacionActual);
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
                habitacionActualizadas = true;
            }   
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }


    IEnumerator CargarUltimaHabitacion()
    {
        ultimaHabitacionCargada = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {

            Room ultimaHabitacion = loadedRooms[loadedRooms.Count - 1];            

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


        
        
        // Se podria mirar lo de que a parte de resetear el spawner, tambien se inicie el spawner de la habitacion para que no se este en el update del roomcontroller

        spawnController.ResetSpawnerState();

        
    }



}
