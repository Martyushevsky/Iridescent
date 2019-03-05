using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class UnitStats : NetworkBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SyncVar] private int _curHealth;

        public Stat Damage;
        public Stat Armor; // защита
        public Stat MoveSpeed; // скорость перемещения

        public virtual int CurHealth
        {
            get { return _curHealth; }
            protected set { _curHealth = value; }
        }

        public int MaxHealth => _maxHealth;

        public virtual void TakeDamage(int damage)
        {
            damage -= Armor.GetValue();
            if (damage > 0)
            {
                CurHealth -= damage;
                if (CurHealth <= 0)
                {
                    CurHealth = 0;
                }
            }
        }

        public void AddHealth(int amount)
        {
            _curHealth += amount;
            if (_curHealth > _maxHealth) _curHealth = _maxHealth;
        }

        public void SetHealthRate(float rate)
        {
            CurHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
        }
    }
}