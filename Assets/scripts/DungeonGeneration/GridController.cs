using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // Start is called before the first frame update
    public Room room;

    [System.Serializable]

    public struct Grid
    {
        public int filas, columnas;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public List<Vector2> puntosDisponibles = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.columnas = room.Width - 2;
        grid.filas = room.Height - 2;

        GenerateGrid();
    }


    public void GenerateGrid()
    {
        grid.verticalOffset += room.GetComponent<Transform>().localPosition.y;
        grid.horizontalOffset += room.GetComponent<Transform>().localPosition.x;

        for (int y = 0; y < grid.filas; y++)
        {
            for (int x = 0; x < grid.columnas; x++)
            {
                GameObject go = Instantiate(gridTile, transform);
                go.GetComponent<Transform>().position = new Vector2(x - (grid.columnas - grid.horizontalOffset), y - (grid.filas - grid.verticalOffset));
                go.name = "X: " + x + " Y: " + y;
                puntosDisponibles.Add(go.transform.position);
            }
        }   
    }
    
}
