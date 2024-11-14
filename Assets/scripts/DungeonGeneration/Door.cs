using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;






public class Door : MonoBehaviour
{

    public GameObject puertaCerrada; // Imagen de puerta cerrada con Collider
    public GameObject puertaAbierta;  // Imagen de puerta abierta sin Collider


    public enum DoorType
    {
        top,
        bottom,
        left,
        right
    };

   

    public DoorType doorType;



    // Método para cerrar la puerta(activar la imagen de puerta cerrada y su collider)
    public void CerrarPuerta()
    {
        puertaCerrada.SetActive(true);
        puertaAbierta.SetActive(false);
    }


    // Método para abrir la puerta (activar la imagen de puerta abierta y desactivar el collider)
    public void AbrirPuerta()
    {
        puertaCerrada.SetActive(false);
        puertaAbierta.SetActive(true);
    }


}
