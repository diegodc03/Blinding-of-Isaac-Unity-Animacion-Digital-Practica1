using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonCravler : MonoBehaviour
{
    
    public Vector2Int Posicion { get; set; }

    public DungeonCravler(Vector2Int posicionInicial)
    {
        Posicion = posicionInicial;
    }

    public Vector2Int Mover(Dictionary<Direcciones, Vector2Int> direccionesDeMovimientoEnElMapa)
    {
        Direcciones direccion = (Direcciones)Random.Range(0, 4);
        Posicion += direccionesDeMovimientoEnElMapa[direccion];
        return Posicion;
    }   


}
