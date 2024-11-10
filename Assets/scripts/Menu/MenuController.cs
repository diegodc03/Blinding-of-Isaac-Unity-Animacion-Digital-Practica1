using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [Header("Configuracion de la aplicacion")]
    public Slider volumenJuego;
    public Toggle mute;

    public AudioMixer audioMixer;

   


    private void Awake()
    {
        volumenJuego.onValueChanged.AddListener(delegate { CambiarVolumen(); });
    }

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


    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }



    public void CambiarVolumen()
    {
        audioMixer.SetFloat("Musica", volumenJuego.value);
    }


    public void Mute()
    {
        if (mute.isOn)
        {
            audioMixer.SetFloat("Musica", -80);
        }
        else
        {
            audioMixer.SetFloat("Musica", volumenJuego.value);
        }
    }


}
