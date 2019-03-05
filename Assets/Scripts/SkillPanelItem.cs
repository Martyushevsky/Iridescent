using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
    public class SkillPanelItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] GameObject holder;
        [SerializeField] Text timerText;

        public void SetSkill(Skill skill)
        {
            if (skill != null)
            {
                icon.sprite = skill.icon;
                holder.SetActive(false);
                timerText.gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetHolder(bool active)
        {
            holder.SetActive(active);
        }

        public void SetCastTime(float time)
        {
            timerText.text = ((int)time).ToString();
            timerText.gameObject.SetActive(time > 0);
        }
    }
}