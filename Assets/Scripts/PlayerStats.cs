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

		public override void OnStartServer()
		{
			base.OnStartServer();
			Damage.OnStatChanged += DamageChanged;
			Armor.OnStatChanged += ArmorChanged;
			MoveSpeed.OnStatChanged += MoveSpeedChanged;
		}

		private void DamageChanged(int value)
		{
			if (_manager != null) _manager.Damage = value;
		}

		private void ArmorChanged(int value)
		{
			if (_manager != null) _manager.Armor = value;
		}

		private void MoveSpeedChanged(int value)
		{
			if (_manager != null) _manager.MoveSpeed = value;
		}
	}
}