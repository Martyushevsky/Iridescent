using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
    public class SkillTreeItem : MonoBehaviour
    {

        [SerializeField] Image icon;
        [SerializeField] Text levelText;
        [SerializeField] GameObject holder;

        public void SetSkill(UpgradeableSkill skill)
        {
            if (skill != null)
            {
                icon.sprite = skill.icon;
                skill.onSetLevel += ChangeLevel;
                ChangeLevel(skill, skill.level);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetUpgradable(bool active)
        {
            holder.SetActive(active);
        }

        void ChangeLevel(UpgradeableSkill skill, int newLevel)
        {
            levelText.text = newLevel.ToString();
        }
    }
}