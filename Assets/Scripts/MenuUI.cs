using UnityEngine;
using UnityEngine.Networking;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject skillsPanel;

    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).serverMode)
        {
            menuPanel.SetActive(false);
            healthBar.SetActive(false);
            skillsPanel.SetActive(false);
        }
    }

    public void Disconnect()
    {
        if (NetworkManager.singleton.IsClientConnected())
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
