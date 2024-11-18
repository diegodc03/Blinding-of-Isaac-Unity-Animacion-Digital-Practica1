using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaFinal : MonoBehaviour
{
    


    public void SalirDeLaAplicacion()
    {
        Debug.Log("Saliendo de la aplicacion");
        Application.Quit();
    }


    public void salirseAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
