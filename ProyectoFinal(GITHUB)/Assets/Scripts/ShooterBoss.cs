using UnityEngine;
using System.Collections;
public class ShooterBoss : MonoBehaviour
{
    public GameObject Shoot; // Prefab del disparo
    public Transform firePoint;
    public float fireRate = 0.5f;

    private float minFireRate = 0.25f; // Tiempo mínimo entre disparos
    private float maxFireRate = 1f; // Tiempo máximo entre disparos

    void Start()
    {
        Debug.Log($"Iniciando ShooterEnemigo en {gameObject.name}");

        // Crear firePoint directamente como hijo del enemigo
        GameObject firePointObj = new GameObject("BossFirePoint");
        firePointObj.transform.parent = transform;
        firePointObj.transform.localPosition = new Vector3(0f, firePoint.localPosition.y, 0f);
        firePoint = firePointObj.transform;

        Debug.Log($"FirePoint creado en {gameObject.name}");
        Debug.Log($"ShooterEnemigo iniciado en {gameObject.name}. Disparará cada {fireRate} segundos");

        // Comienza a disparar inmediatamente
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
    public void StartShooting()
    {

        StartCoroutine(FrecuenciaDisparo());
    }
    private IEnumerator FrecuenciaDisparo ()
    {
        while (true)
        {
            ShootBullet();
            float randomDelay = Random.Range(minFireRate, maxFireRate);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}