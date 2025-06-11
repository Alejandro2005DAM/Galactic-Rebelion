using UnityEngine;

public class ShooterEnemigo : MonoBehaviour
{
    public GameObject Shoot; // Prefab del disparo
    public Transform firePoint;
    public float fireRate = 1f;

    void Start()
    {
        Debug.Log($"Iniciando ShooterEnemigo en {gameObject.name}");
         if (Shoot == null)
        {
            Debug.LogWarning($"¡No hay prefab de disparo asignado en {gameObject.name}!");
        }
        // Comienza a disparar inmediatamente
        InvokeRepeating("ShootBullet", 0.1f, fireRate);
    }

    void ShootBullet()
    {
        if (Shoot == null || firePoint == null)
        {
            Debug.LogError($"Error en configuración de disparo en {gameObject.name}");
            return;
        }

        // Usa Quaternion.Euler para definir explícitamente la rotación (0, 0, 180 apunta hacia abajo)
        GameObject bullet = Instantiate(Shoot, firePoint.position, Quaternion.Euler(0, 0, 180));
        Debug.Log($"Bala enemiga disparada en posición {bullet.transform.position}");
    }

    public void StopShooting()
    {
        CancelInvoke("ShootBullet");
    }
}
