using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InicioJugador : MonoBehaviour
{
    void Start()
    {
        int indexJugador= PlayerPrefs.GetInt("JugadorIndex");
         Vector3 posicion = transform.position;

        // Forzamos Z a 0 para asegurarnos de que el personaje esté en el plano visible
        posicion.z = 0;
        Instantiate(GameManager.Instance.personajes[indexJugador].personaje, posicion, Quaternion.identity);
    }

}
