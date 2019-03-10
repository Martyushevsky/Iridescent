using UnityEngine;

namespace Geekbrains
{
    [System.Serializable]
    public class UnitSkills
    {
        [SerializeField] private Skill[] _skills;

        public Skill this[int index]
        {
            get { return _skills[index]; }
            set { _skills[index] = value; }
        }

        public int Count { get { return _skills.Length; } }

        UserData data;

        public void Load(UserData data)
        {
            this.data = data;
            for (int i = 0; i < _skills.Length; i++)
            {
                UpgradeableSkill skill = _skills[i] as UpgradeableSkill;
                if (i >= data.skills.Count) data.skills.Add(skill.level);
                else skill.level = data.skills[i];
                skill.onSetLevel += ChangeLevel;
            }
        }

        void ChangeLevel(UpgradeableSkill skill, int newLevel)
        {
            for (int i = 0; i < _skills.Length; i++)
            {
                if (_skills[i] == skill)
                {
                    data.skills[i] = newLevel;
                    break;
                }
            }
        }

        public bool inCast
        {
            get
            {
                foreach (Skill skill in _skills)
                    if (skill.castDelay > 0) return true;
                return false;
            }
        }
    }
}