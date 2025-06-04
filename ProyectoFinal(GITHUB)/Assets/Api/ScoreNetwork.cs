using UnityEngine;
using System;
using System.Collections;
public class ScoreNetwork : MonoBehaviour
{
   public void EnviarPartida(string userName, int puntos, Action<bool, string> callback)
    {
        StartCoroutine(EnviarPartidaCoroutine(userName, puntos, callback));
    }

    private IEnumerator EnviarPartidaCoroutine(string userName, int puntos, Action<bool, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("puntos", puntos);
      
        // Enviar solicitud POST usando WWW (obsoleto)
        WWW www = new WWW("http://localhost/game/createMatch.php", form);
        
        // Esperar a que termine la solicitud
        yield return www;

        // Verificar si la solicitud fue exitosa
        if (!string.IsNullOrEmpty(www.error))
        {
            callback(false, www.error);  // Enviar error al callback
        }
        else
        {
            callback(true, www.text);  // Enviar respuesta al callback
        }
        
    }
}
