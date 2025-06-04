using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletTorpedo : MonoBehaviour
{
    public Vector3 direction = Vector3.up; // Puedes modificar la dirección por defecto
    public float speed = 5f;
    private bool tocado;
    private Puntaje puntaje;
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
   

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            
            
                player.TomarDaño(5);
                Destroy(gameObject);
                tocado=true;
                Puntaje puntaje= FindObjectOfType<Puntaje>();
                if(puntaje!=null)
                {
                    puntaje.Restar(10f);
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