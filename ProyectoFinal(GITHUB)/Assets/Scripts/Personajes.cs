using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Personaje")]
public class Personajes : ScriptableObject
{
    public GameObject personaje;
    public Sprite imagen;
    public string nombre;

}
