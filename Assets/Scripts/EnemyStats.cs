namespace Geekbrains
{
    public class EnemyStats : UnitStats
    {
        public override void OnStartServer()
        {
            CurHealth = MaxHealth;
        }
    }
}