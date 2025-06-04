using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RankingManager : MonoBehaviour
{
    [Header("Textos del Ranking (Ordenados del 1 al 10)")]
    [SerializeField] private TextMeshProUGUI[] rankingTexts = new TextMeshProUGUI[10];

    [Header("Configuración")]
    [SerializeField] private string formatoTexto = "{0}. {1} - {2} pts";
    [SerializeField] private string textoVacio = "{0}. ---";
    [SerializeField] private GameObject botonVolver;
    private NetworkManager networkManager;

    void Start()
    {
        // Buscar el NetworkManager en la escena
        networkManager = FindObjectOfType<NetworkManager>();

        if (networkManager == null)
        {
            Debug.LogError("No se encontró NetworkManager en la escena!");
            MostrarError("Error: NetworkManager no encontrado");
            return;
        }

        // Cargar el ranking automáticamente al inicio
        CargarRanking();
    }

    /// <summary>
    /// Método público para cargar el ranking (puedes llamarlo desde un botón)
    /// </summary>
    public void CargarRanking()
    {
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager no está disponible");
            return;
        }

        // Mostrar "Cargando..." en todos los textos
        MostrarCargando();

        // Solicitar las top 10 puntuaciones
        networkManager.GetTopScores(OnTopScoresReceived, 10);
    }

    /// <summary>
    /// Callback que se ejecuta cuando se reciben las puntuaciones del servidor
    /// </summary>
    private void OnTopScoresReceived(NetworkManager.TopScoresResponse response)
    {
        if (!response.done)
        {
            Debug.LogError("Error al obtener puntuaciones: " + response.message);
            MostrarError("Error al cargar ranking");
            return;
        }

        // Limpiar todos los textos primero
        LimpiarRanking();

        // Si no hay puntuaciones
        if (response.scores == null || response.scores.Length == 0)
        {
            Debug.Log("No hay puntuaciones registradas");
            MostrarRankingVacio();
            return;
        }

        // Mostrar las puntuaciones obtenidas
        for (int i = 0; i < response.scores.Length && i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                string textoRanking = string.Format(formatoTexto,
                    i + 1,
                    response.scores[i].userName,
                    response.scores[i].score);

                rankingTexts[i].text = textoRanking;
                rankingTexts[i].color = Color.white;
            }
        }

        // Llenar las posiciones restantes con texto vacío
        for (int i = response.scores.Length; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = string.Format(textoVacio, i + 1);
                rankingTexts[i].color = Color.gray;
            }
        }

        Debug.Log($"Ranking actualizado con {response.scores.Length} puntuaciones");
    }

    /// <summary>
    /// Muestra "Cargando..." en todos los textos
    /// </summary>
    private void MostrarCargando()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = $"{i + 1}. Cargando...";
                rankingTexts[i].color = Color.yellow;
            }
        }
    }

    /// <summary>
    /// Muestra un mensaje de error en todos los textos
    /// </summary>
    private void MostrarError(string mensaje)
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = $"{i + 1}. {mensaje}";
                rankingTexts[i].color = Color.red;
            }
        }
    }

    /// <summary>
    /// Limpia todos los textos del ranking
    /// </summary>
    private void LimpiarRanking()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = "";
                rankingTexts[i].color = Color.white;
            }
        }
    }

    /// <summary>
    /// Muestra el ranking vacío con guiones
    /// </summary>
    private void MostrarRankingVacio()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = string.Format(textoVacio, i + 1);
                rankingTexts[i].color = Color.gray;
            }
        }
    }

    /// <summary>
    /// Método para refrescar el ranking (útil para botones)
    /// </summary>
    public void RefrescarRanking()
    {
        CargarRanking();
    }
    public void VolverAlMenu()
    {
     SceneManager.LoadScene("MenuScene");
    }
}