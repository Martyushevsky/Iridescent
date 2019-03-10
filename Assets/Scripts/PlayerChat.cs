using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class PlayerChat : NetworkBehaviour
    {
        #region Singleton
        public static PlayerChat instance;
        public override void OnStartClient()
        {
            if (instance != null)
            {
                Debug.LogError("More than one instance of PlayerChat found!");
                return;
            }
            instance = this;
        }
        #endregion

        public List<ChatChannel> channels = new List<ChatChannel>();

        public delegate void ChangeChannelsDelegate();
        public event ChangeChannelsDelegate onChangeChannels;

        public delegate void ReciveMessageDelegate(ChatMessage message);
        public event ReciveMessageDelegate onReciveMessage;

        public void RegisterChannel(ChatChannel channel)
        {
            channels.Add(channel);
            if (onChangeChannels != null) onChangeChannels.Invoke();
        }

        [Command]
        public void CmdSendFromChannel(GameObject channelGO, ChatMessage message)
        {
            message.author = AccountManager.GetAccount(connectionToClient).login;
            channelGO.GetComponent<ChatChannel>().SendFromChanel(message);
        }

        public void ReciveChatMessage(ChatMessage message)
        {
            onReciveMessage.Invoke(message);
        }
    }
}