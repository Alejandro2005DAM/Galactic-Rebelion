using UnityEngine;
using System.Collections.Generic;
public class Moverenemigo : MonoBehaviour
{
    public float speed = 2;

    [HideInInspector] public List<GameObject> f1;
    [HideInInspector] public List<GameObject> f2;
    [HideInInspector] public List<GameObject> f3;
    public AudioClip boom;
    private bool isDying = false;

    private AudioSource audiosource;
    private int ind = 0;
    private List<Transform> ruta = new List<Transform>();
     [SerializeField] private float vida;
    [SerializeField] private float maxvida;

    public  GameObject particles;
    private Puntaje puntaje;
    private void Awake()
    {
        puntaje = Object.FindFirstObjectByType<Puntaje>();
    }

    void Start()
    {
    audiosource = GetComponent<AudioSource>();
    if(audiosource == null)
    {
     audiosource = gameObject.AddComponent<AudioSource>();
    }
    vida=maxvida;

    if (f1.Count > 0) ruta.Add(f1[Random.Range(0, f1.Count)].transform);
    if (f2.Count > 0) ruta.Add(f2[Random.Range(0, f2.Count)].transform);
    if (f3.Count > 0) ruta.Add(f3[Random.Range(0, f3.Count)].transform);
        
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
        if (other.CompareTag("destroyer"))
        {
            Destroy(gameObject);
        }
    }
 

    void Update()
    {
        if(ind>=ruta.Count) return;
          Transform target = ruta[ind];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            ind++;
        }
    }
     public void TomarDaño(float daño)
    {
        vida-=daño;
        if(vida<=0 && !isDying){
            isDying = true;

            
            if (puntaje != null)
            {
                puntaje.Sumar(10f);
            }
              if(particles!=null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            MonoBehaviour shooter= GetComponent<ShooterEnemigo>();
            if(shooter != null)
                {
                shooter.enabled = false; // Desactivar el script ShooterEnemigo
                }
            MonoBehaviour bala= GetComponent<Bullet>();
            if(bala != null)
                {
                bala.enabled = false; // Desactivar el script Bullet
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
                Destroy(gameObject, boom.length - 7.25f);
            }
            else
            {
                // Si no hay sonido, destruir inmediatamente
                Destroy(gameObject);
            }
            
        }

    }
}
