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


    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Room HabitacionActual;

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
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomQueue();
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
            }else if(ultimaHabitacionCargada && habitacionActualizadas)
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
            if (!habitacionActualizadas)
            {
                foreach (Room room in loadedRooms)
                {
                    Room ultimaHabitacion = loadedRooms[loadedRooms.Count - 1];
                    Room tempRoom = new Room(ultimaHabitacion.X, ultimaHabitacion.Y);
                    Destroy(ultimaHabitacion.gameObject);

                    var habitacionAEliminar = loadedRooms.FirstOrDefault(item => item.X == tempRoom.X && item.Y == tempRoom.Y);
                    if(habitacionAEliminar != null)
                    {
                        loadedRooms.Remove(habitacionAEliminar);
                        LoadRoom("End", tempRoom.X, tempRoom.Y);
                    }
                    

                }
                habitacionActualizadas = true;
            }
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
    }



}
