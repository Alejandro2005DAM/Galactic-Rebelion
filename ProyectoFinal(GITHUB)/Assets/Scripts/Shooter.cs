using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject Shoot;        // Prefab del disparo
    public Transform firePoint;     // Punto desde donde se dispara
    public float fireRate = 0.5f;   // Tiempo entre disparos (en segundos)

    public AudioClip shootsound;

    private AudioSource audiosource;

    void Start()
    {
        Debug.Log($"Iniciando Shooter en {gameObject.name}");
        audiosource = GetComponent<AudioSource>();
        if (audiosource == null)
        {
            audiosource = gameObject.AddComponent<AudioSource>();
        }

        if (firePoint == null)
        {
            Debug.LogWarning($"¡No hay firePoint asignado en {gameObject.name}!");
        }

        if (Shoot == null)
        {
            Debug.LogWarning($"¡No hay prefab de disparo asignado en {gameObject.name}!");
        }

        InvokeRepeating(nameof(ShootBullet), 0f, fireRate);
    }

    void ShootBullet()
    {
        Debug.Log($"Intentando disparar desde Shooter en {gameObject.name}");

        if (Shoot == null)
        {
            Debug.LogError($"¡No hay prefab de disparo asignado en {gameObject.name}!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError($"¡FirePoint es nulo en {gameObject.name}!");
            return;
        }

        if (shootsound != null)
        {
            audiosource.PlayOneShot(shootsound);
        }
        else
        {
            Debug.Log("No hay disparo");
        }

        Instantiate(Shoot, firePoint.position, firePoint.rotation);
        Debug.Log($"Bala disparada desde Shooter en {gameObject.name}");
    }
    
}