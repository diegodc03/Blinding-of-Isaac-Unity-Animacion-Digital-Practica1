using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaFinal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



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
