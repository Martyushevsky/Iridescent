using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    // Панели
    [SerializeField] GameObject curPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject loadingPanel;

    // Поля ввода
    [SerializeField] InputField loginLogin;
    [SerializeField] InputField loginPass;
    [SerializeField] InputField registerLogin;
    [SerializeField] InputField registerPass;
    [SerializeField] InputField registerConfirm;

    MyNetworkManager mgr;

    private void Start()
    {
        mgr = NetworkManager.singleton as MyNetworkManager;
        if (mgr.serverMode)
        {
            loginPanel.SetActive(false);
        }
        else
        {
            // привязка обработчиков ответа с сервера к событиям менеджера
            mgr.loginResponseDelegate = LoginResponse;
            mgr.registerResponseDelegate = RegisterResponse;
        }
    }

    public void LoginResponse(string response)
    {
        switch (response)
        {
            case "UserError":
                Debug.Log("Error: Username not Found");
                break;
            case "PassError":
                Debug.Log("Error: Password Incorrect");
                break;
            default:
                Debug.Log("Error: Unknown Error. Please try again later.");
                break;
        }
        loadingPanel.SetActive(false);
        curPanel.SetActive(true);
        ClearInputs();
    }

    public void RegisterResponse(string response)
    {
        switch (response)
        {
            case "Success":
                Debug.Log("User registered");
                break;
            case "UserError":
                Debug.Log("Error: Username Already Taken");
                break;
            default:
                Debug.Log("Error: Unknown Error. Please try again later.");
                break;
        }
        loadingPanel.SetActive(false);
        curPanel.SetActive(true);
        ClearInputs();
    }

    void ClearInputs()
    {
        loginLogin.text = "";
        loginPass.text = "";
        registerLogin.text = "";
        registerPass.text = "";
        registerConfirm.text = "";
    }

    public void SetPanel(GameObject panel)
    {
        curPanel.SetActive(false);
        curPanel = panel;
        curPanel.SetActive(true);
        //ClearInputs();
    }

    public void Login()
    {
        mgr.Login(loginLogin.text, loginPass.text);
        curPanel.SetActive(false);
        loadingPanel.SetActive(true);
    }

    public void Register()
    {
        if (registerPass.text != "" && registerPass.text == registerConfirm.text)
        {
            mgr.Register(registerLogin.text, registerPass.text);
            curPanel.SetActive(false);
            loadingPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Error: Password Incorrect");
            ClearInputs();
        }
    }
}