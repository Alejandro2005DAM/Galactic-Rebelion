using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class MenuPrincipal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int index;
    [SerializeField] private Image imagen;
    [SerializeField] private TextMeshProUGUI nombre;
    private string dificultad = "Facil"; // Dificultad por defecto
    private AudioSource audioSource;
    public AudioClip levelsound, startsound; // Sonido del clic del botón
    private GameManager gameManager;
    [SerializeField] private GameObject botonStart;
    [SerializeField] private GameObject botonSalir;
    [SerializeField] private GameObject botonLogout;

    [SerializeField] private GameObject botonRanking;
    [SerializeField] private TextMeshProUGUI userNameDisplay;

    private void Start()
    {
        userNameDisplay.text = PlayerPrefs.GetString("username", "Invitado");
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (gameManager == null)
        {
            Debug.LogError(" GameManager.Instance es null. ¿Está en la escena?");
            return;
        }

        if (gameManager.personajes == null || gameManager.personajes.Count == 0)
        {
            Debug.LogError(" La lista de personajes está vacía o no asignada.");
            return;
        }

        index = PlayerPrefs.GetInt("JugadorIndex", 0);

        if (index > gameManager.personajes.Count - 1)
        {
            index = 0;
        }

        CambiarPantalla();
    }
    private void CambiarPantalla()
    {
        if (gameManager == null || gameManager.personajes == null || gameManager.personajes.Count == 0)
        {
            Debug.LogWarning(" No se puede cambiar la pantalla. Verifica GameManager y personajes.");
            return;
        }

        if (index < 0 || index >= gameManager.personajes.Count)
        {
            Debug.LogWarning(" Índice fuera de rango, se reinicia a 0.");
            index = 0;
        }

        PlayerPrefs.SetInt("JugadorIndex", index);
        imagen.sprite = gameManager.personajes[index].imagen;
        nombre.text = gameManager.personajes[index].nombre;

    }
    public void Ranking()
    {
       StartCoroutine(LoadRankingScene());
    }
    private System.Collections.IEnumerator LoadRankingScene()
    {
        PlayClickSound();
        float waitTime = levelsound != null ? levelsound.length-1f : 0.5f; // Tiempo de espera basado en la duración del sonido
        yield return new WaitForSeconds(waitTime); // Espera el tiempo calculado antes de cargar la escena
        SceneManager.LoadScene("RankingScene");
    }
    public void startgame()
    {
        StartCoroutine(StartWithSound());
    }
    private System.Collections.IEnumerator StartWithSound()
    {
        PlayStartSound();
        float waitTime = startsound != null ? startsound.length-1f : 0.5f; // Tiempo de espera basado en la duración del sonido
        yield return new WaitForSeconds(waitTime);
        PlayerPrefs.SetString("Dificultad", dificultad);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene("IntroScene");
    }
    public void logout()
    {
        SceneManager.LoadScene("LoginScene1");
    }

    public void Sigiente()
    {
        if (index == gameManager.personajes.Count - 1)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }
        CambiarPantalla();
    }
    public void Anterior()
    {
        if (index == 0)
        {
            index = gameManager.personajes.Count - 1;
        }
        else
        {
            index -= 1;
        }
        CambiarPantalla();
    }
    public void exit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
    }
    public void Facil()
    {
        PlayClickSound();
        dificultad = "Facil";
        Debug.Log("Dificultad establecida: " + dificultad);
    }
    public void Medio()
    {
        PlayClickSound();
        dificultad = "Medio";
        Debug.Log("Dificultad establecida: " + dificultad);
    }
    public void Dificil()
    {
        PlayClickSound();
        dificultad = "Dificil";
        Debug.Log("Dificultad establecida: " + dificultad);
    }

    public void PlayClickSound()
    {
        if (audioSource != null && levelsound != null)
        {
            audioSource.PlayOneShot(levelsound);
        }
        else
        {
            Debug.LogWarning("AudioSource o clickSound no están configurados correctamente.");
        }
    }
    public void PlayStartSound()
    {
        if (audioSource != null && startsound != null)
        {
            audioSource.PlayOneShot(startsound);
        }
        else
        {
            Debug.LogWarning("AudioSource o startSound no están configurados correctamente.");
        }
    }
}