using UnityEngine;

public class Bulletplayer : MonoBehaviour
{
    public Vector3 direction = Vector3.up; // Puedes modificar la dirección por defecto
    public float speed = 5f;

    [SerializeField] private GameObject particles;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("enemy"))
        {
            Moverenemigo enemy = other.GetComponent<Moverenemigo>();
            if (enemy != null)
            {
            Debug.Log("Llamando a TomarDaño...");
            enemy.TomarDaño(1);
            }
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            
        }
          if (other.CompareTag("finallboss"))
        {
            BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TomarDaño(1);

                }

            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
                Destroy(gameObject);
             
            
        }

        if(other.CompareTag("destroyer"))
        {
            Destroy(gameObject);
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
