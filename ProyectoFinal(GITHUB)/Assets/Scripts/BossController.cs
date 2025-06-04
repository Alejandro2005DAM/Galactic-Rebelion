using UnityEngine;
public class BossController : MonoBehaviour
{
    public float speeddown = 1f;
    
    private bool inmune = true;
   [HideInInspector] public Transform[] waypoints;
   private SpriteRenderer spriteRenderer;
    [SerializeField] private float vida;
    [SerializeField] private float maxvida;
    public AudioClip boom;
    private bool isDying = false;
    private AudioSource audiosource;
    private Color originalColor;
    public float speed = 2f;
    private Puntaje puntaje;
    private int currentWaypoint = 0;
    private Vector3 targetPosition;
    private bool isarrived = false;
    public Generadorobjetos generador;
    private GameObject shoot;
    public GameObject particles;

    private ShooterBoss shooterBoss;

  
    private void Awake()
    {
        puntaje = Object.FindFirstObjectByType<Puntaje>();
    }
    void Start()
    {
        shooterBoss = GetComponent<ShooterBoss>();
        if (shooterBoss == null)
        {
            shooterBoss = gameObject.AddComponent<ShooterBoss>();
        }
       
        audiosource = GetComponent<AudioSource>();
        if (audiosource == null)
        {
            audiosource = gameObject.AddComponent<AudioSource>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        vida = maxvida;
        targetPosition= new Vector3(transform.position.x, 3.2f, transform.position.z);

    }
    void Update()
    {
        if(!isarrived){
            
            Vector3 currentPosition = transform.position;
            targetPosition = new Vector3(currentPosition.x, 1.8f, currentPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speeddown * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                if(shooterBoss != null)
                {
                    shooterBoss.StartShooting();
                }
                isarrived = true;
                inmune = false; // Desactivar inmunidad al llegar al waypoint
            }
            return;
        }
         if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            int waypoint;
            do
            {
                waypoint = Random.Range(0, waypoints.Length);
            } while (waypoint == currentWaypoint && waypoints.Length > 1);
            currentWaypoint = waypoint;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TomarDaño(1f);
            }
        }
    }
     public void TomarDaño(float daño)
    {
        if(inmune) return;

        vida -=daño;
        if(spriteRenderer != null)
        {
            float porcentaje = vida / maxvida;
            spriteRenderer.color = Color.Lerp(Color.red,originalColor, porcentaje);
        }
        if(vida<=0 && !isDying){
            isDying = true;
            
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);


            }
            if (puntaje != null)
            {
                puntaje.Sumar(20f);
            }
            if (generador != null)
            {
                generador.Defeated();
            }
             if (boom != null)
            {
                audiosource.PlayOneShot(boom);

                // Mantener solo el componente AudioSource y desactivar el resto
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
                Collider collider = GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;

                }

                // Destruir el objeto después de que termine el sonido
                Destroy(gameObject, 0.5f); 
            }
            else
            {
                // Si no hay sonido, destruir inmediatamente
                Destroy(gameObject);
            }
        }

    }

  
}
