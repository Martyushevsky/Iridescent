using UnityEngine.Networking;

namespace Geekbrains
{
	public class StatsManager : NetworkBehaviour
	{
		[SyncVar] public int Damage, Armor, MoveSpeed;
	}
}