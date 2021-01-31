using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderEfeitos;

    public static Controller controller;
    private GameObject porta;

    private float volumeEfeitos;
    private float volumeMusica;

    public Vector3 spawn;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        controller = this;
    }
    public void CarregarCena(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void LiberarCaminho()
    {
        porta = GameObject.FindGameObjectWithTag("Porta");
        //porta.GetComponent<Porta>().LiberarCaminho();
    }
    public void getVolume()
    {
        volumeMusica = sliderMusica.value;
        volumeEfeitos = sliderEfeitos.value;
    }
    public void Sair()
    {
        Application.Quit();
    }
}
