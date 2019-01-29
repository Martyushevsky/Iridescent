using UnityEngine;

namespace Geekbrains
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
	public class Item : ScriptableObject
	{
		public string Name = "New Item";
		public Sprite Icon = null;
		public ItemPickup PickupPrefab;

		public virtual void Use(Player player)
		{
			Debug.Log("Using " + name);
		}
	}
}