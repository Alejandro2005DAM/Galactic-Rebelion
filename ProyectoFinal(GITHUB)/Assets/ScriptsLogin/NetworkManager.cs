using System;
using System.Collections;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
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

    // Método para probar la conexión al servidor
    public void TestConnection(Action<bool, string> callback)
    {
        StartCoroutine(CO_TestConnection(callback));
    }

    private IEnumerator CO_TestConnection(Action<bool, string> callback)
    {
        string url = "http://localhost/game/createUser.php";
        Debug.Log($"Probando conexión a: {url}");

        WWW w = new WWW(url);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError($"Error al probar conexión: {w.error}");
            callback(false, w.error);
        }
        else
        {
            Debug.Log($"Conexión exitosa, respuesta: {w.text}");
            callback(true, w.text);
        }
    }
    public void CheckUser(string userName, string pass, Action<Response> response)
    {
        StartCoroutine(CO_CheckUser(userName, pass, response));
    }

    private IEnumerator CO_CheckUser(string userName, string pass, Action<Response> response)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("pass", pass);

        WWW w = new WWW("http://localhost/game/checkUser.php", form);
        yield return w;
        Debug.Log(w.text);
        response(JsonUtility.FromJson<Response>(w.text));
    }


    public void SaveScore(string userName, int score, Action<Response> callback)
    {
        StartCoroutine(CO_SaveScore(userName, score, callback));
    }

    private IEnumerator CO_SaveScore(string userName, int score, Action<Response> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("score", score);

        WWW w = new WWW("http://localhost/game/saveScore.php", form);
        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError("Error: " + w.error);
            callback(new Response { done = false, message = w.error });
            yield break;
        }

        try
        {
            Response response = JsonUtility.FromJson<Response>(w.text);
            callback(response);
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing response: " + e.Message);
            callback(new Response { done = false, message = "Error parsing server response" });
        }
    }
public void GetTopScores(Action<TopScoresResponse> callback, int limit = 10)
{
    StartCoroutine(CO_GetTopScores(callback, limit));
}

private IEnumerator CO_GetTopScores(Action<TopScoresResponse> callback, int limit)
{
    WWWForm form = new WWWForm();
    form.AddField("limit", limit);

    WWW w = new WWW("http://localhost/game/getTopScores.php", form);
    yield return w;

    Debug.Log("=== RESPUESTA CRUDA DEL SERVIDOR ===");
    Debug.Log($"Error: '{w.error}'");
    Debug.Log($"Text: '{w.text}'");

    if (!string.IsNullOrEmpty(w.error))
    {
        Debug.LogError("Error de conexión: " + w.error);
        callback(new TopScoresResponse { done = false, message = w.error });
        yield break;
    }

    if (string.IsNullOrEmpty(w.text))
    {
        Debug.LogError("Respuesta vacía del servidor");
        callback(new TopScoresResponse { done = false, message = "Respuesta vacía del servidor" });
        yield break;
    }

    try
    {
        Debug.Log("=== INTENTANDO PARSEAR JSON ===");
        Debug.Log($"JSON a parsear: '{w.text}'");
        
        TopScoresResponse response = JsonUtility.FromJson<TopScoresResponse>(w.text);
        
        Debug.Log("=== JSON PARSEADO EXITOSAMENTE ===");
        Debug.Log($"Response.done: {response.done}");
        Debug.Log($"Response.message: '{response.message}'");
        Debug.Log($"Response.scores es null: {response.scores == null}");
        
        if (response.scores != null)
        {
            Debug.Log($"Número de scores: {response.scores.Length}");
            for (int i = 0; i < response.scores.Length; i++)
            {
                Debug.Log($"Score[{i}]: userName='{response.scores[i].userName}', score={response.scores[i].score}, date='{response.scores[i].date}'");
            }
        }
        
        callback(response);
    }
    catch (Exception e)
    {
        Debug.LogError("=== ERROR AL PARSEAR JSON ===");
        Debug.LogError("Error: " + e.Message);
        Debug.LogError("StackTrace: " + e.StackTrace);
        Debug.LogError("JSON que falló: '" + w.text + "'");
        
        callback(new TopScoresResponse { done = false, message = "Error parsing server response: " + e.Message });
    }
}

    [System.Serializable]
    public class TopScoresResponse : Response
    {
        public ScoreData[] scores;
    }

    [System.Serializable]
    public class ScoreData
    {
        public string userName;
        public int score;
        public string date;
    }



}
