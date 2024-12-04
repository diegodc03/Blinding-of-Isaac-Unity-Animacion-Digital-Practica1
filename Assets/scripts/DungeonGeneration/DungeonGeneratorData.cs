using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DungeonGeneratorData : MonoBehaviour
{
    public int numeroDeRastreadores = 1;
    public int iteradorMinimo;
    public int iteradorMaximo;

    private void Start()
    {
        if (numeroDeRastreadores == 0)
        {
            numeroDeRastreadores = 1;
        }

        if (iteradorMinimo < 4 )
        {
            iteradorMinimo = 4;
        }

    }
}
