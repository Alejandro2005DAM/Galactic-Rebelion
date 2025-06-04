using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Puntaje : MonoBehaviour
{
    public static Puntaje Instancia;
    private float puntos;
    private TextMeshProUGUI text;
    private bool jugadorTocado = false;

    void Awake()
    {
          if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    IEnumerator SumarCadaSegundo()
    {
        while (!jugadorTocado)
        {
            puntos += 1;
            text.text = puntos.ToString("0");
            yield return new WaitForSeconds(0.25f);
        }
    }

    

     public void Sumar(float cant)
    {
        puntos += cant;
        text.text = puntos.ToString("0");
    }
      public void Restar(float cant)
    {
       
        
        if(puntos>cant){
            puntos-=cant;
        }else
        {
            puntos=0;
        }
        text.text=puntos.ToString("0");
    }
    public int puntuacion()
    {
        return Mathf.RoundToInt(puntos);
    }
    public void guardarPuntuacion()
    {
        PlayerPrefs.SetInt("PuntajeFinal", puntuacion());
        PlayerPrefs.Save();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
     text=FindObjectOfType<TextMeshProUGUI>();
     if(scene.name == "GameScene")
     {
        puntos=0;
        guardarPuntuacion();
     }else
     {
         puntos= PlayerPrefs.GetInt("PuntajeFinal",0);

     }
     StartCoroutine(SumarCadaSegundo()); 


    }
     void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void pararSumar()
    {
        StopCoroutine(SumarCadaSegundo());
    }
}

