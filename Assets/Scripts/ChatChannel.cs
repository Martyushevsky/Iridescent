using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class ChatChannel : NetworkBehaviour
    {
        public new string name;

        public virtual void SendFromChanel(ChatMessage message)
        {
            RpcSendFromChanel(message);
        }

        [ClientRpc]
        protected void RpcSendFromChanel(ChatMessage message)
        {
            PlayerChat.instance.ReciveChatMessage(message);
        }

        public virtual void SendFromPlayerChat(string text)
        {
            ChatMessage msg = new ChatMessage(PlayerChat.instance.netId, NetworkInstanceId.Invalid, PlayerChat.instance.name, text);
            PlayerChat.instance.CmdSendFromChannel(gameObject, msg);
        }
    }

    [System.Serializable]
    public struct ChatMessage
    {
        public NetworkInstanceId senderId;
        public NetworkInstanceId reciverId;
        public string author;
        public string message;

        public ChatMessage(NetworkInstanceId sender, NetworkInstanceId receiver, string author, string message)
        {
            senderId = sender;
            reciverId = receiver;
            this.author = author;
            this.message = message;
        }
    }
}