using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image[] chaves;
    [SerializeField] private Sprite chavePega;
    public static UI ui;

    private void Start()
    {
        ui = this;
    }
    public void unPause()
    {
        Time.timeScale = 1;
        FindObjectOfType<Player>().paused = false;       
    }
    private void Update()
    {
        Pause();
    }
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            FindObjectOfType<Player>().paused = true; 
        }
    }
    public void PegarChave()
    {
        if (chaves[0].sprite != chavePega)
        {
            chaves[0].sprite = chavePega;
        }
        else if (chaves[1].sprite != chavePega)
        {
            chaves[1].sprite = chavePega;
        }
        else
        {
            chaves[2].sprite = chavePega;

            Controller.controller.LiberarCaminho();
        }
    }
}
