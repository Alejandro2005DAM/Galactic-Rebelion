using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuGameOver : MonoBehaviour
{
    [SerializeField] private GameObject botonReiniciar;
    [SerializeField] private GameObject botonMenu;
    [SerializeField] private TextMeshProUGUI texto;
    private int puntos;
    private string usuario;
    private ScoreNetwork m_scorenetwork = null;

    public void Awake()
    {
        m_scorenetwork = Object.FindFirstObjectByType<ScoreNetwork>();

    }
    public void Start()
    {
        puntos= PlayerPrefs.GetInt("PuntajeFinal",0);
        usuario= PlayerPrefs.GetString("username","default");
        m_scorenetwork.EnviarPartida(usuario, puntos, (success, responseMessage) =>
        {
            if (success)
            {
                Debug.Log("Partida enviada correctamente: " + responseMessage);
            }
            else
            {
                Debug.LogError("Error al enviar la partida: " + responseMessage);
            }
        });
        texto.text=" FINAL SCORE : "+ puntos.ToString(); 
    }
     public void restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

     public void backmenu(){
         Time.timeScale = 1f;
         SceneManager.LoadScene("MenuScene");
       
    }


}
