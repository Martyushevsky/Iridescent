using UnityEngine.Networking;

namespace Geekbrains
{
    public class StatsManager : NetworkBehaviour
    {
        [SyncVar] public int Damage, Armor, MoveSpeed;
        [SyncVar] public int Level, StatPoints, skillPoints;
        [SyncVar] public float Exp, NextLevelExp;
        public Player Player;

        [Command]
        public void CmdUpgradeStat(int stat)
        {
            if (Player.Progress.RemoveStatPoint())
            {
                switch (stat)
                {
                    case (int)StatType.Damage: Player.Character.Stats.Damage.BaseValue++; break;
                    case (int)StatType.Armor: Player.Character.Stats.Armor.BaseValue++; break;
                    case (int)StatType.MoveSpeed: Player.Character.Stats.MoveSpeed.BaseValue++; break;
                }
            }
        }

        [Command]
        public void CmdUpgradeSkill(int index)
        {
            if (Player.Progress.RemoveSkillPoint())
            {
                UpgradeableSkill skill = Player.Character.unitSkills[index] as UpgradeableSkill;
                if (skill != null)
                {
                    skill.level++;
                }
            }
        }
    }

    public enum StatType { Damage, Armor, MoveSpeed }
}