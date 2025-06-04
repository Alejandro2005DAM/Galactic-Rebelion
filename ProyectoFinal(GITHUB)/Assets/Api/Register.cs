using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    [Header("Campos de Registro")]
    [SerializeField] private TMP_InputField m_userNameInput = null;
    [SerializeField] private TMP_InputField m_emailInput = null;
    [SerializeField] private TMP_InputField m_passwordInput = null;
    [SerializeField] private TMP_InputField m_reEnterPasswordInput = null;
    [SerializeField] private TextMeshProUGUI infoText = null;

    private RegisterNetwork m_networkManager = null;

    private void Awake()
    {
        m_networkManager = Object.FindFirstObjectByType<RegisterNetwork>();
    }

    public void RegisterButton()
    {
        string username = m_userNameInput.text;
        string email = m_emailInput.text;
        string password = m_passwordInput.text;
        string rePassword = m_reEnterPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
        {
            infoText.text = "Por favor completa todos los campos.";
            return;
        }
        if(password.Length < 8)
        {
            infoText.text = "Conraseña demasiado corta, debe tener al menos 8 caracteres.";
            return;
        }
        if(!email.Contains("@gmail.com") && !email.Contains("@hotmail.com") && !email.Contains("@outlook.com"))
        {
            infoText.text = "Correo inválido, debe ser de Gmail, Hotmail o Outlook.";
            return;
        }

        
        if(password != rePassword)
        {
            infoText.text = "Las contraseñas no coinciden.";
            return; 
        }

        infoText.text = "Registrando...";

        m_networkManager.CreateUser(username, email, password, delegate (Response res)
        {
            infoText.text = res.message;

            if (res.done)
            {
                Debug.Log("Registro exitoso");
                // Guardar el nombre del usuario si se desea
                PlayerPrefs.SetString("username", username);
                SceneManager.LoadScene("LoginScene1"); 
            }
            else
            {
                Debug.LogWarning("Registro fallido: " + res.message);
            }
        });
    }


    public void BackToLogin()
    {
        SceneManager.LoadScene("LoginScene1");
    }
}
