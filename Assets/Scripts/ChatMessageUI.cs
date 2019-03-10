using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
    public class ChatMessageUI : MonoBehaviour
    {

        [SerializeField] Text authorText;
        [SerializeField] Text messageText;
        [SerializeField] Button privateButton;

        ChatMessage msg;

        public void SetChatMessage(ChatMessage message)
        {
            msg = message;
            authorText.text = message.author;
            messageText.text = message.message;
            privateButton.onClick.AddListener(SetPrivateMessage);
        }

        public void SetPrivateMessage()
        {
            ChatUI.instance.SetPrivateMessage(msg);
        }
    }
}