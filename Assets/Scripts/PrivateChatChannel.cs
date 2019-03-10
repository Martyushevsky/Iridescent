using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class PrivateChatChannel : ChatChannel
    {
        NetworkInstanceId reciver;

        public void SetReciverMessage(ChatMessage message)
        {
            reciver = message.senderId;
            name = message.author;
        }

        public override void SendFromPlayerChat(string text)
        {
            ChatMessage msg = new ChatMessage(PlayerChat.instance.netId, reciver, PlayerChat.instance.name, text);
            PlayerChat.instance.CmdSendFromChannel(gameObject, msg);
        }

        public override void SendFromChanel(ChatMessage message)
        {
            TargetSendFromChanel(NetworkServer.objects[message.reciverId].connectionToClient, message);
        }

        [TargetRpc]
        protected void TargetSendFromChanel(NetworkConnection target, ChatMessage message)
        {
            PlayerChat.instance.ReciveChatMessage(message);
        }
    }
}