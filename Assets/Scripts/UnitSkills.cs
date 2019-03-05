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