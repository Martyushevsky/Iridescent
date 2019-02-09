using UnityEngine;
using UnityEngine.Networking;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).serverMode)
            menuPanel.SetActive(false);
    }

    public void Disconnect()
    {
        if (NetworkManager.singleton.IsClientConnected())
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
