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
    public void pause()
    {


        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
        barra.SetActive(false);
        introtexto.SetActive(false);

    }

    public void resume(){

        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
        barra.SetActive(true);
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
