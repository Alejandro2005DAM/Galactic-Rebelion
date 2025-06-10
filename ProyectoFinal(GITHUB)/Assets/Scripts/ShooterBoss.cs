using UnityEngine;
using System.Collections;
public class ShooterBoss : MonoBehaviour
{
    public GameObject Shoot; // Prefab del disparo
    public Transform firePoint;
    public float fireRate = 0.5f;

    private float minFireRate = 0.25f; // Tiempo mínimo entre disparos
    private float maxFireRate = 1f; // Tiempo máximo entre disparos
    private Coroutine shootingCoroutine;
  
    void ShootBullet()
    {
        if (Shoot == null || firePoint == null)
        {
            Debug.LogError($"Error en configuración de disparo en {gameObject.name}");
            return;
        }

        // Usa Quaternion.Euler para definir explícitamente la rotación (0, 0, 180 apunta hacia abajo)
        Instantiate(Shoot, firePoint.position, Quaternion.Euler(0, 0, 180));
    }
    public void StartShooting()
    {
        if (shootingCoroutine == null)
        {
        shootingCoroutine = StartCoroutine(FrecuenciaDisparo());
            
        }
    }
    private IEnumerator FrecuenciaDisparo()
    {
        while (true)
        {
            ShootBullet();
            float randomDelay = Random.Range(minFireRate, maxFireRate);
            yield return new WaitForSeconds(randomDelay);
        }
    }
    public void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
            
        }
    }
}
