using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Vidaplayer : MonoBehaviour
{
     public int vidas = 3;

    private void Start()
    {
        
    }

    public void Daño(int amount)
    {
       vidas-=amount;

       if(vidas <= 0)
       {
        SceneManager.LoadScene("GameOverScene");
       }
    }

}
