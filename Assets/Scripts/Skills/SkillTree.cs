using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
    public class SkillTree : MonoBehaviour
    {
        #region Singleton
        public static SkillTree instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one instance of SkillTree found!");
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] SkillTreeItem[] items;
        [SerializeField] Text skillPointsText;

        StatsManager manager;
        int curSkillPoints;

        void Update()
        {
            if (manager != null)
            {
                CheckManagerChanges();
            }
        }

        public void SetManager(StatsManager statsManager)
        {
            manager = statsManager;
            CheckManagerChanges();
        }

        public void SetCharacter(Character character)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetSkill(i < character.unitSkills.Count ? character.unitSkills[i] as UpgradeableSkill : null);
            }
            if (manager != null) CheckManagerChanges();
        }

        private void CheckManagerChanges()
        {
            if (curSkillPoints != manager.skillPoints)
            {
                curSkillPoints = manager.skillPoints;
                skillPointsText.text = curSkillPoints.ToString();
                if (curSkillPoints > 0) SetUpgradableSkills(true);
                else SetUpgradableSkills(false);
            }
        }

        void SetUpgradableSkills(bool active)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetUpgradable(active);
            }
        }

        public void UpgradeSkill(SkillTreeItem skillItem)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == skillItem)
                {
                    manager.CmdUpgradeSkill(i);
                    break;
                }
            }
        }
    }
}