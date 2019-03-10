using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class UpgradeableSkill : Skill
    {
        public delegate void SetLevelDelegate(UpgradeableSkill skill, int newLevel);
        public event SetLevelDelegate onSetLevel;

        [SyncVar(hook = "LevelHook")] int _level = 1;
        public virtual int level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (onSetLevel != null) onSetLevel.Invoke(this, level);
            }
        }

        void LevelHook(int newLevel)
        {
            level = newLevel;
        }
    }
}