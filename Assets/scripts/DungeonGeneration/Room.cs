using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int X;
    public int Y;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    private bool actualizarPuertas = false;

    //Constructor
    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }


    public List<Door> doors = new List<Door>();



    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null)
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
                        d.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.bottom:
                    if (!RoomController.instance.DoesRoomExist(X, Y - 1))
                    {
                        d.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.left:
                    if (!RoomController.instance.DoesRoomExist(X - 1, Y))
                    {
                        d.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.right:
                    if (!RoomController.instance.DoesRoomExist(X + 1, Y))
                    {
                        d.gameObject.SetActive(false);
                    }
                    break;
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
                    if (!RoomController.instance.DoesRoomExist(X, Y + 1) || puertaEncontrada)
                    {
                        d.gameObject.SetActive(false);
                    }else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.bottom:
                    if (!RoomController.instance.DoesRoomExist(X, Y - 1) || puertaEncontrada)
                    {        
                        d.gameObject.SetActive(false);
                    }else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.left:
                    if (!RoomController.instance.DoesRoomExist(X - 1, Y) || puertaEncontrada)
                    {
                        d.gameObject.SetActive(false);
                    }else{
                        puertaEncontrada = true;
                    }
                    break;
                case Door.DoorType.right:
                    if (!RoomController.instance.DoesRoomExist(X + 1, Y) || puertaEncontrada)
                    {
                        //Debug.Log("Eliminando puertas 1");
                        d.gameObject.SetActive(false);
                    }else{
                        puertaEncontrada = true;
                    }
                    break;
            }
        }
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
        if(collision.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }

}
