using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed; 
    private Rigidbody2D rig; 
    private Vector2 control;
    
    private Joystick joy; 
    private BarraDeVida barra; 
    private TextMeshProUGUI textointroduccion;
    [SerializeField] private float vida;
    [SerializeField] private float maxvida;

    [SerializeField] private GameObject particles;

    private Generadorobjetos generadorobjetos;
    private bool iswall;
    [SerializeField] public AudioClip gameOverSound;
    private AudioSource audioSource;
    public float minX = -2.76f;
    public float maxX = 2.65f;
    public float minY = -3.72f;
    public float maxY = 4.75f;
    private Puntaje puntaje;
    private float tiemposobrevivido=0;
    [SerializeField] private float tiempopasar=60;
    private bool yapaso=false;
    private GameObject personajeInstanciado;

    
    private void Awake()
    {
        textointroduccion = GameObject.Find("IntroTexto").GetComponent<TextMeshProUGUI>();
        if (textointroduccion == null)
        {
            Debug.LogError("No se encontró el objeto de texto de introducción.");
        }
        rig = GetComponent<Rigidbody2D>();

        // Buscar automáticamente el Joystick y la barra de vida
        joy = Object.FindFirstObjectByType<Joystick>();
        if (joy == null)
        {
            Debug.LogError(" No se encontró ningún Joystick en la escena.");
        }

        barra = Object.FindFirstObjectByType<BarraDeVida>();
        if (barra == null)
        {
            Debug.LogError("No se encontró la barra de vida en la escena.");
        }
        puntaje = Object.FindFirstObjectByType<Puntaje>();
        if (puntaje == null)
        {
            Debug.LogError("No se encontró el sistema de puntuación.");
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("Se ha añadido un AudioSource al jugador porque no se encontró uno existente.");
        }
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        vida = maxvida;

        if (barra != null)
        {
            barra.Iniciarbarravida(vida);
        }
        MostrarTexto();
    }

    private void Update()
    {
        if (joy != null)
        {
            Vector2 mover = new Vector2(joy.Horizontal, joy.Vertical);
            control = mover.normalized * speed;

            if (iswall)
            {
                Debug.Log("Está tocando la pared.");
            }
        }

        if(!yapaso)
        {
            tiemposobrevivido+=Time.deltaTime;
            if(tiemposobrevivido > tiempopasar)
            {
                siguienteFase();
            }
        }
    }

    private void FixedUpdate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float horizontalInput = spriteRenderer.bounds.size.x / 2;
        float verticalInput = spriteRenderer.bounds.size.y / 2;

        Vector2 newPos = rig.position + control * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX + horizontalInput, maxX - horizontalInput);
        newPos.y = Mathf.Clamp(newPos.y, minY + verticalInput, maxY - verticalInput);
        rig.MovePosition(newPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
            iswall = true;
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;

        if (barra != null)
        {
            barra.Cambiarvidactual(vida);
        }

        if (vida <= 0)
        {

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Puntaje puntaje= Object.FindFirstObjectByType<Puntaje>();
            if (puntaje != null)
            {
                PlayerPrefs.SetInt("PuntajeFinal", puntaje.puntuacion());
                puntaje.pararSumar();
            }
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);

            }
            
            if(gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }
            StartCoroutine(esperar());
        }
    }
    
    private IEnumerator esperar()
    {
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("GameOverScene");
    }
    private void MostrarTexto()
    {
        string escena = SceneManager.GetActiveScene().name;
        if (escena == "GameScene")
        {
            textointroduccion.text = "Sobrevive el tiempo necesario para pasar a la siguiente fase";
            textointroduccion.gameObject.SetActive(true);
            StartCoroutine(QuitarTexto());
        }
        else if (escena == "GameScene 1")
        {
            textointroduccion.text = "Sobrevive el tiempo necesario para pasar a la siguiente fase";
            textointroduccion.gameObject.SetActive(true);
            StartCoroutine(QuitarTexto());
        }
        else if (escena == "GameScene 2")
        {
            textointroduccion.text = "Sobrevive el tiempo necesario para pasar a la siguiente fase";
            textointroduccion.gameObject.SetActive(true);
            StartCoroutine(QuitarTexto());
        }

    }
    private IEnumerator QuitarTexto()
    {
        yield return new WaitForSeconds(1.5f);
        textointroduccion.gameObject.SetActive(false);
    }
    private void siguienteFase()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (puntaje != null)
        {
            puntaje.guardarPuntuacion();
        }
        if (escena == "GameScene")
        {
            SceneManager.LoadScene("GameScene 1");
        }
        else if (escena == "GameScene 1")
        {
            SceneManager.LoadScene("GameScene 2");
        }
        else if (escena == "GameScene 2")
        {
            SceneManager.LoadScene("EndScene");
        }
        

    }
}
