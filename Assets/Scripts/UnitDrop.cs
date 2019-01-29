using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	[RequireComponent(typeof(Unit))]
	public class UnitDrop : NetworkBehaviour
	{
		[SerializeField] private DropItem[] _dropItems = new DropItem[0];

		public override void OnStartServer()
		{
			GetComponent<Unit>().EventOnDie += Drop;
		}

		private void Drop()
		{
			for (var i = 0; i < _dropItems.Length; i++)
			{
				if (Random.Range(0, 100f) <= _dropItems[i].Rate)
				{
					var pickupItem = Instantiate(_dropItems[i].Item.PickupPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
					pickupItem.Item = _dropItems[i].Item;
					NetworkServer.Spawn(pickupItem.gameObject);
				}
			}
		}

		[System.Serializable]
		struct DropItem
		{
			public Item Item;
			[Range(0f, 100f)]
			public float Rate;
		}
	}
}