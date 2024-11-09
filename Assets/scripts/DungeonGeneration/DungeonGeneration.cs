using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public DungeonGeneratorData DungeonGeneratorData;

    private List<Vector2Int> habitacionesDeLaMazmorra;
    
    private void Start()
    {
        habitacionesDeLaMazmorra = DungeonCravlerController.GenerarMazmorra(DungeonGeneratorData);
        GenerarHabitaciones(habitacionesDeLaMazmorra);
    }


    private void GenerarHabitaciones(IEnumerable<Vector2Int> habitaciones)
    {
        RoomController.instance.LoadRoom("Start",0,0);
        foreach (Vector2Int habitacion in habitaciones)
        {
            /*
            if(habitacion == habitacionesDeLaMazmorra[habitacionesDeLaMazmorra.Count - 1] && !(habitacion == Vector2Int.zero))
            {
                RoomController.instance.LoadRoom("End", habitacion.x, habitacion.y);
            }
            else
            {
                RoomController.instance.LoadRoom("Empty", habitacion.x, habitacion.y);
            }
            */
            
            RoomController.instance.LoadRoom("Empty", habitacion.x, habitacion.y);
        }
    }

}
