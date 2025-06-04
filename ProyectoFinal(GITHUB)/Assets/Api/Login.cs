using UnityEngine;
using TMPro;
using UnityEngine.UI; // IMPORTANTE: Usar TextMeshPro en vez de UnityEngine.UI para campos TMP
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_loginPasswordInput = null;
    [SerializeField] private TMP_InputField m_loginUserNameInput = null;
    [SerializeField] private TextMeshProUGUI info;
  
    private LoginNetwork m_networkManager = null;

    private void Awake()
    {
        m_networkManager = Object.FindFirstObjectByType<LoginNetwork>();
    }
    public void loginbutton()
    {
        if (string.IsNullOrEmpty(m_loginUserNameInput.text) || string.IsNullOrEmpty(m_loginPasswordInput.text))
        {
            info.text="Por favor completa los campos";
            return;
        }
        info.text="Conectando...";
        m_networkManager.CheckUser(m_loginUserNameInput.text, m_loginPasswordInput.text, delegate (Response res)
        {
            if (res.done)
            {
                info.text = res.message;
                Debug.Log("Login exitoso");

                // Aquí puedes guardar el usuario, cambiar de escena, etc.
                PlayerPrefs.SetString("username", m_loginUserNameInput.text);
                 SceneManager.LoadScene("MenuScene"); // Por ejemplo
            }
            else
            {
                info.text = res.message;
                Debug.LogWarning("Login fallido: " + res.message);
            }
        });
    }

    public void registerbutton()
    {
        SceneManager.LoadScene("LoginScene2");
    }

    public void exitbutton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); 
        #endif
    }
}    
 

