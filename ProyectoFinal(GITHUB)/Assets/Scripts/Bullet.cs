using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction = Vector3.up; // Puedes modificar la dirección por defecto
    public float speed = 5f;
    private Puntaje puntaje;
    private bool tocado;

    [SerializeField] private GameObject particles;

    void Start()
    {
        string dificultad = PlayerPrefs.GetString("Dificultad", "Facil");
        switch (dificultad)
        {
            case "Facil":
                speed = 5f; // Velocidad de la bala en modo fácil
                break;
            case "Medio":
                speed = 7f; // Velocidad de la bala en modo medio
                break;
            case "Dificil":
                speed = 9f; // Velocidad de la bala en modo difícil
                break;
            default:
                Debug.LogWarning("Dificultad no reconocida, usando velocidad por defecto.");
                break;
        }
        puntaje = Object.FindFirstObjectByType<Puntaje>();
        if (puntaje == null)
        {
            Debug.LogError("No se encontró el sistema de puntuación.");
        }
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();


            player.TomarDaño(1);
            Destroy(gameObject);
            tocado = true;
            Puntaje puntaje = FindFirstObjectByType<Puntaje>();
            if (puntaje != null)
            {
                puntaje.Restar(5f);
            }
                if(particles != null)
                {
                    Instantiate(particles, transform.position, Quaternion.identity);
                }
        }

        if(other.CompareTag("destroyer"))
        {
            Destroy(gameObject);
            tocado=true;

        }
    }
   /*  private void OnCollisionEnter2D(Collision2D collision)
    {
    if (collision.gameObject.CompareTag("destroyer"))
    {
        Destroy(gameObject);
    }
    if (collision.gameObject.CompareTag("Player"))
    {}
    }
    */
}

