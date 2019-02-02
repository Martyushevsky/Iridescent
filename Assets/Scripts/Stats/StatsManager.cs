using UnityEngine.Networking;

namespace Geekbrains
{
    public class StatsManager : NetworkBehaviour
    {
        [SyncVar] public int Damage, Armor, MoveSpeed;
        [SyncVar] public int Level, StatPoints;
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
    }

    public enum StatType { Damage, Armor, MoveSpeed }
}