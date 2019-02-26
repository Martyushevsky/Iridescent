using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelect : MonoBehaviour
{
    #region Singleton
    public static CharacterSelect instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        manager.serverRegisterHandler += RegisterServerHandler;
        manager.clientRegisterHandler += RegisterClientHandler;
    }
    #endregion

    [SerializeField] MyNetworkManager manager;

    void RegisterServerHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnSelectCharacter);
    }
    void RegisterClientHandler(NetworkClient client)
    {
        client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnOpenSelectUI);
    }

    void OnSelectCharacter(NetworkMessage netMsg)
    {
        NetworkHash128 hash = netMsg.reader.ReadNetworkHash128();
        if (hash.IsValid())
        {
            UserAccount account = AccountManager.GetAccount(netMsg.conn);
            account.data.characterHash = hash;
            manager.AccountEnter(account);
        }
    }

    void OnOpenSelectUI(NetworkMessage netMsg)
    {
        CharacterSelectUI.instance.OpenPanel();
    }

    public void SelectCharacter(NetworkHash128 characterHash)
    {
        if (characterHash.IsValid())
        {
            manager.client.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new HashMessage(characterHash));
        }
    }
}

public class HashMessage : MessageBase
{
    public NetworkHash128 hash;

    public HashMessage()
    {
    }
    public HashMessage(NetworkHash128 hash)
    {
        this.hash = hash;
    }

    public override void Deserialize(NetworkReader reader)
    {
        hash = reader.ReadNetworkHash128();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(hash);
    }
}