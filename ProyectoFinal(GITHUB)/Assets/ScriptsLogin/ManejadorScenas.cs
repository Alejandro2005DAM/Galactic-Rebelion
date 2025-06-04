using UnityEngine;
using TMPro;
using UnityEngine.UI; // IMPORTANTE: Usar TextMeshPro en vez de UnityEngine.UI para campos TMP

public class ManejadorScenas : MonoBehaviour
{
    [Header ("Login")]
    [SerializeField] private TMP_InputField m_loginPasswordInput = null;
    [SerializeField] private TMP_InputField m_loginUserNameInput = null;

    [Header ("Register")]
    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;

    [SerializeField] private TMP_InputField m_userNameInput = null;
    [SerializeField] private TMP_InputField m_emailInput = null;
    [SerializeField] private TMP_InputField m_password = null;
    [SerializeField] private TMP_InputField m_reEnterpassword = null;

    [SerializeField] private TextMeshProUGUI m_text = null;

    private NetworkManager m_networkManager = null;

    private void Awake()
    {
        m_networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

 void Start()
{
    Debug.Log($"Username input: {m_userNameInput}");
    Debug.Log($"Email input: {m_emailInput}");
    Debug.Log($"Password input: {m_password}");
    Debug.Log($"Re-enter password input: {m_reEnterpassword}");
    Debug.Log($"Text label: {m_text}");

    ShowLogin();
}

public void SubmitLogin()
{
    if(m_loginUserNameInput.text == "" || m_loginPasswordInput.text == "")
    {
        m_text.text = "Por favor llena todos los campos";
        return;
    }
    m_text.text = "Procesando....";
    m_networkManager.CheckUser(m_loginUserNameInput.text, m_loginPasswordInput.text, delegate (Response response){
        m_text.text = response.message;
    });
}
public void SumitRegister()
{
    if (m_userNameInput.text == "" || m_emailInput.text == "" || m_password.text == "" || m_reEnterpassword.text == "")
    {
        m_text.text = "Por favor llene todos los campos";
        return;
    }

    if (m_password.text == m_reEnterpassword.text)
    {
        if (m_networkManager != null)
        {
            m_networkManager.CreateUser(m_userNameInput.text, m_emailInput.text, m_password.text, delegate (Response response)
            {
                m_text.text = response.message;
            });
        }
        else
        {
            Debug.LogError("NetworkManager no encontrado. Por favor, añada un GameObject con el componente NetworkManager.");
            m_text.text = "Error: NetworkManager no encontrado";
        }
    }
    else
    {
        m_text.text = "Contraseñas no son iguales, verificar";
    }
}
    public void ShowLogin()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);
    }

    public void ShowRegister()
    {
        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
    }
}

