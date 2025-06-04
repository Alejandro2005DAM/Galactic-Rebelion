using UnityEngine;
using System;
using System.Collections;

public class RegisterNetwork : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  public void CreateUser(string userName, string email, string pass, Action<Response> callback)
    {
        StartCoroutine(CO_CreateUser(userName, email, pass, callback));
    }

    private IEnumerator CO_CreateUser(string userName, string email, string pass, Action<Response> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("email", email);
        form.AddField("pass", pass);

        string url = "http://localhost/game/createUser.php";
        Debug.Log($"Enviando petición a: {url}");
        Debug.Log($"Datos: userName={userName}, email={email}, pass=[oculto]");

        WWW w = new WWW(url, form);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError($"Error de conexión: {w.error}");
            Response errorResponse = new Response
            {
                done = false,
                message = $"Error de conexión: {w.error}"
            };
            callback(errorResponse);
            yield break;
        }

        Debug.Log($"Respuesta del servidor: '{w.text}'");

        // Verificar si la respuesta está vacía
        if (string.IsNullOrEmpty(w.text))
        {
            Debug.LogError("Respuesta vacía del servidor");
            Response emptyResponse = new Response
            {
                done = false,
                message = "El servidor no devolvió ninguna respuesta"
            };
            callback(emptyResponse);
            yield break;
        }

        try
        {
            // Intentar parsear la respuesta como JSON
            Response parsedResponse = JsonUtility.FromJson<Response>(w.text);
            Debug.Log($"Respuesta parseada exitosamente: done={parsedResponse.done}, message={parsedResponse.message}");
            callback(parsedResponse);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al parsear JSON: {e.Message}");
            Debug.LogError($"Contenido recibido: '{w.text}'");
            
            // Crear una respuesta con el error
            Response errorResponse = new Response
            {
                done = false,
                message = "Error al procesar la respuesta del servidor"
            };
            callback(errorResponse);
        }
    }
}
