using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class NetUnitSetup : NetworkBehaviour
	{
		[SerializeField] private MonoBehaviour[] _disableBehaviours;

		private void Awake()
		{
			if (hasAuthority) return;
			foreach (var behaviour in _disableBehaviours)
			{
				behaviour.enabled = false;
			}
		}

		public override void OnStartAuthority()
		{
			foreach (var behaviour in _disableBehaviours)
			{
				behaviour.enabled = true;
			}
		}
	}
}