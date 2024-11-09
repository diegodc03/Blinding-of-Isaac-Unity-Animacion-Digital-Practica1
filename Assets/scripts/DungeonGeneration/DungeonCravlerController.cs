using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direcciones
{
    arriba = 0,
    
    izquierda = 1,

    abajo = 2,

    derecha = 3
};





public class DungeonCravlerController : MonoBehaviour
{

    public static List<Vector2Int> posicionesVisitadas = new List<Vector2Int>();    
    
    private static readonly Dictionary<Direcciones, Vector2Int> direcciones = new Dictionary<Direcciones, Vector2Int>
    {
        {Direcciones.arriba, Vector2Int.up},
        {Direcciones.izquierda, Vector2Int.left},
        {Direcciones.abajo, Vector2Int.down},
        {Direcciones.derecha, Vector2Int.right}
    };

    public static List<Vector2Int> GenerarMazmorra(DungeonGeneratorData dungeonGeneratorData)
    {
        
        List<DungeonCravler> RastreadorDeMazmorra = new List<DungeonCravler>();

        for (int i= 0; i < dungeonGeneratorData.numeroDeRastreadores; i++)
        {
            RastreadorDeMazmorra.Add(new DungeonCravler(Vector2Int.zero));
        }

        int iterador = Random.Range(dungeonGeneratorData.iteradorMinimo, dungeonGeneratorData.iteradorMaximo);

        for(int i = 0; i < iterador; i++)
        {
            foreach (DungeonCravler rastreador in RastreadorDeMazmorra)
            {
                Vector2Int nuevaPosicion = rastreador.Mover(direcciones);
                posicionesVisitadas.Add(nuevaPosicion);
            }
        }

        return posicionesVisitadas;
    }




}
