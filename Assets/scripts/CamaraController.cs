using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    //at the start of the game, the camera will be in the room 0 0

    public Room habitacionActual;

    public int cameraSpeedWhenChange;

    public static CamaraController instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actualizarPosicionCamara();
    }



    private void Awake()
    {
        instance = this;
    }


    private void actualizarPosicionCamara()
    {
        if(habitacionActual == null)
        {
            return;
        }

        Vector3 posicionRequerida = GetPosicionCamara();

        // lo que hace esto es coger y guardar en transform.position la posicion actual de la camara y la posicion actual de la camara
        // pasa de transfrom.postiion, a posicionActual, con una velocidad de cameraSpeedWhenChange * Time.deltaTime
        transform.position = Vector3.MoveTowards(transform.position, posicionRequerida, cameraSpeedWhenChange * Time.deltaTime);

    }

    private Vector3 GetPosicionCamara()
    {
        if(habitacionActual == null)
        {
            return Vector3.zero;
        }

        Vector3 posicion = habitacionActual.GetRoomCentre();
        posicion.z = transform.position.z;

        return posicion; 
    }   


    public bool IsChangingRoom()
    {
        return transform.position.Equals(GetPosicionCamara()) == false;
    }


}
