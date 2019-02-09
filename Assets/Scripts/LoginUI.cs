using UnityEngine;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    [SerializeField] GameObject loginPanel;

    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).serverMode)
            loginPanel.SetActive(false);
    }

    public void Login()
    {
        NetworkManager.singleton.StartClient();
    }
}