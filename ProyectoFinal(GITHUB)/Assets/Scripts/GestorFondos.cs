using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBackground : MonoBehaviour
{
    [SerializeField] private GameObject background;
    private Vector2 posBackground = new Vector2(0, 40.9f);
    private Transform posLastBackground;

    private void Start()
    {
        posLastBackground = GetComponentInChildren<Transform>();
    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 vectorLastBackground = posLastBackground.position;
            GameObject newBackground = Instantiate(background, vectorLastBackground + posBackground, Quaternion.identity);
            newBackground.transform.parent = this.transform.parent;
            posLastBackground = newBackground.transform;
            newBackground.gameObject.name = "Fondo";
        }
      else if (collision.gameObject.CompareTag("destructor"))
        {
            Destroy(gameObject);
        }
    }
}