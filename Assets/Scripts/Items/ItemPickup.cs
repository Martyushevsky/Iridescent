using UnityEngine;

namespace Geekbrains
{
	public class ItemPickup : Interactable
	{
		public Item Item;

		public override bool Interact(GameObject user)
		{
			return PickUp(user);
		}

		public bool PickUp(GameObject user)
		{
			var character = user.GetComponent<Character>();
			if (character != null && character.Inventory.Add(Item))
			{
				Destroy(gameObject);
				return true;
			}

			return false;
		}
	}
}