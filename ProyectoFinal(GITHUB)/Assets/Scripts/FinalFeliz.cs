using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class FinalFeliz : MonoBehaviour
{
       [SerializeField] private GameObject botonSaltar;


       public void saltarCinematica()
       {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScene");
       }

}
