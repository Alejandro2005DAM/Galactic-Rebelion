using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Menupausa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject botonReiniciar;
    [SerializeField] private GameObject botonMenu;
    [SerializeField] private GameObject introtexto;
    [SerializeField] private GameObject barra;
    [SerializeField] private AudioSource audioSource;
    private Joystick joy;
    public void Awake()
    {
        joy = FindFirstObjectByType<Joystick>();
        if (joy == null)
        {
            Debug.LogError("No se encontró ningún Joystick en la escena.");
        }
    }
    public void pause()
    {


        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
        barra.SetActive(false);
        introtexto.SetActive(false);
        audioSource.Pause();
        joy.enabled = false; // Desactiva el joystick al pausar el juego

    }

    public void resume()
    {

        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
        barra.SetActive(true);
        audioSource.UnPause();
        joy.enabled = true; // Reactiva el joystick al reanudar el juego
    }

    public void backmenu(){
         Time.timeScale = 1f;
         SceneManager.LoadScene("MenuScene");
       
    }

    public void restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
  

}
