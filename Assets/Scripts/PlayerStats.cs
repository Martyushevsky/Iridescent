namespace Geekbrains
{
    public class PlayerStats : UnitStats
    {
        private StatsManager _manager;
        public StatsManager Manager
        {
            set
            {
                _manager = value;
                _manager.Damage = Damage.GetValue();
                _manager.Armor = Armor.GetValue();
                _manager.MoveSpeed = MoveSpeed.GetValue();
            }
        }

        private UserData data;

        public void Load(UserData data)
        {
            this.data = data;
            CurHealth = data.curHealth;
            if (data.statDamage > 0) Damage.BaseValue = data.statDamage;
            if (data.statArmor > 0) Armor.BaseValue = data.statArmor;
            if (data.statMoveSpeed > 0) MoveSpeed.BaseValue = data.statMoveSpeed;
        }

        public override int CurHealth
        {
            get { return base.CurHealth; }
            protected set
            {
                base.CurHealth = value;
                data.curHealth = CurHealth;
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Damage.OnStatChanged += DamageChanged;
            Armor.OnStatChanged += ArmorChanged;
            MoveSpeed.OnStatChanged += MoveSpeedChanged;
        }

        private void DamageChanged(int value)
        {
            if (Damage.BaseValue != data.statDamage)
                data.statDamage = Damage.BaseValue;

            if (_manager != null) _manager.Damage = value;
        }

        private void ArmorChanged(int value)
        {
            if (Armor.BaseValue != data.statArmor)
                data.statArmor = Armor.BaseValue;

            if (_manager != null) _manager.Armor = value;
        }

        private void MoveSpeedChanged(int value)
        {
            if (MoveSpeed.BaseValue != data.statMoveSpeed)
                data.statMoveSpeed = MoveSpeed.BaseValue;

            if (_manager != null) _manager.MoveSpeed = value;
        }
    }
}