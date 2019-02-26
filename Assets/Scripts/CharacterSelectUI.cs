using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelectUI : MonoBehaviour
{
    #region Singleton
    public static CharacterSelectUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of CharacterSelectUI found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject selectPanel;

    public void OpenPanel()
    {
        loginPanel.SetActive(false);
        selectPanel.SetActive(true);
    }

    public void SelectCharacter(NetworkIdentity characterIdentity)
    {
        CharacterSelect.instance.SelectCharacter(characterIdentity.assetId);
    }
}