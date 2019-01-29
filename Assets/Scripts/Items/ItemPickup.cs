using UnityEngine;

namespace Geekbrains
{
	public class ItemPickup : Interactable
	{
		public Item Item;
		public float Lifetime = 25;

		private void Update()
		{
			if (!isServer) return;
			Lifetime -= Time.deltaTime;
			if (Lifetime <= 0) Destroy(gameObject);
		}

		public override bool Interact(GameObject user)
		{
			return PickUp(user);
		}

		public bool PickUp(GameObject user)
		{
			var character = user.GetComponent<Character>();
			if (character != null && character.Player.Inventory.AddItem(Item))
			{
				Destroy(gameObject);
				return true;
			}

			return false;
		}
	}
}