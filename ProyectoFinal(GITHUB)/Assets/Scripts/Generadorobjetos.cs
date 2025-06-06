using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Generadorobjetos : MonoBehaviour
{
    public GameObject[] objetos;  // Prefabs de enemigos
    public float timeSpawn = 2f;
    public float Spawnrate = 3;
    [SerializeField] public GameObject pilot;
    private float velocidad = 1f; // Velocidad de los enemigos

    private float velocidadboss = 1f; // Velocidad del jefe
    public Transform right;
    public Transform left;
    public Transform [] bosswaypoints;
    public List<GameObject> f1;
    public List<GameObject> f2;
    public List<GameObject> f3;
    public GameObject finalboss; // Referencia a la nave
    private GameObject currentboss;
    public float timespawnboss = 10;
    [HideInInspector] public bool alive=false;

    
       
    void Start()
    {
        alive = false;
          string dificultad= PlayerPrefs.GetString("Dificultad", "Facil");
        switch (dificultad)
        {

            case "Facil":
                timeSpawn = 2.5f;
                Spawnrate = 1.5f;
                timespawnboss = 20f;
                velocidad = 4f; // Ajustar velocidad de enemigos en modo fácil
                velocidadboss = 3f; // Ajustar velocidad del jefe en modo fácil
                break;
            case "Medio":
                timeSpawn = 2.5f;
                Spawnrate = 1f;
                timespawnboss = 15f;
                velocidad = 5f; // Ajustar velocidad de enemigos en modo medio
                velocidadboss = 4f; // Ajustar velocidad del jefe en modo medio
                break;
            case "Dificil":
                timeSpawn = 2.5f;
                Spawnrate = 0.6f;
                timespawnboss = 10f;
                velocidad = 6f; // Ajustar velocidad de enemigos en modo difícil
                velocidadboss = 5f; // Ajustar velocidad del jefe en modo difícil
                break;
            default:
                Debug.LogWarning("Dificultad no reconocida, usando valores por defecto.");
                break;
        }
        InvokeRepeating("Spawnenemies", timeSpawn, Spawnrate);
        Invoke("Spawnboss",timespawnboss);
    }

    public void Spawnenemies()
    {
        // Crear enemigos en posiciones aleatorias
        Vector3 pos = new Vector3(Random.Range(left.position.x, right.position.x), transform.position.y, 0f);
        int randomIndex = Random.Range(0, objetos.Length);
        GameObject enemie = Instantiate(objetos[randomIndex], pos, gameObject.transform.rotation);

        // Asignar listas si el enemigo las necesita
        Moverenemigo mover = enemie.GetComponent<Moverenemigo>();
        if (mover != null)
        {
            mover.f1 = f1;
            mover.f2 = f2;
            mover.f3 = f3;
            mover.speed = velocidad; 
        }

        // Comprobar si tiene el componente Shooter
        ShooterEnemigo shooterEnemigo = enemie.GetComponent<ShooterEnemigo>();
        if (shooterEnemigo != null)
        {
            // Crear FirePoint automáticamente para el enemigo
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(enemie.transform);
            firePointObj.transform.localPosition = new Vector3(0f, -1f, 0f); // Hacia abajo
            shooterEnemigo.firePoint = firePointObj.transform;
            
            Debug.Log($"FirePoint asignado a Shooter en {enemie.name}");
        }
        
        
        
        if (shooterEnemigo == null)
        {
            Debug.LogWarning($"El enemigo {enemie.name} no tiene ningún componente de disparo");
        }
    }


    public void Spawnboss()
    {
     
          StartCoroutine(PiltotandBoss());
    }
    private IEnumerator PiltotandBoss()
    {
        CancelInvoke("Spawnenemies");
        if (pilot != null)
        {
            Vector3 startPos = new Vector3(right.position.x + 2f, 0f, 0f); //posición de inicio
            Vector3 endPos = new Vector3(0f, 0f, 0f);// posición final
            pilot.transform.position = startPos; // Posición inicial del piloto
            float duration = 0.25f; // Duración del movimiento
            float elapsedTime = 0f;
            pilot.SetActive(true);
            while (elapsedTime < duration)
            {
                pilot.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Esperar un frame
            }
            pilot.transform.position = endPos;
            yield return new WaitForSeconds(1.5f); // Esperar un segundo antes de la animación de escala
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                pilot.transform.position = Vector3.Lerp(endPos, startPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Esperar un frame
            }
            pilot.transform.position = startPos; // Volver a la posición inicial
            pilot.SetActive(false); // Desactivar el piloto después de la animación
        }
        
        if (alive) yield break; 

        Vector3 pos = new Vector3(-0.2f,4.5f,0f);
        currentboss = Instantiate(finalboss, pos, Quaternion.identity);
        alive = true;

        BossController bosscontroller= currentboss.GetComponent<BossController>();
        if(bosscontroller != null)
            {
            bosscontroller.waypoints=bosswaypoints;
            bosscontroller.speed = velocidadboss; // Asignar velocidad del jefe
            bosscontroller.generador=this;

            }
        Shooter shooter = currentboss.GetComponent<Shooter>();
        if (shooter != null)
            {
        GameObject firePointObj = new GameObject("BossFirePoint");
        firePointObj.transform.SetParent(currentboss.transform);
        firePointObj.transform.localPosition = new Vector3(0f, -1.5f, 0f); // Un poco más abajo que el boss
        shooter.firePoint = firePointObj.transform;
        Debug.Log("Boss Shooter configurado.");
        }
        else
        {
        Debug.LogWarning("El jefe no tiene un componente Shooter, ¡no podrá disparar!");
        }
    }

    public void Defeated()
    {
        alive = false;
        currentboss = null;
        InvokeRepeating("Spawnenemies", timeSpawn, Spawnrate);
        Invoke("Spawnboss", timespawnboss);

    }

}
