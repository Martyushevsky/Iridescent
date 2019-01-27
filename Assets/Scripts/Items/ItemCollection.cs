using UnityEngine;

namespace Geekbrains
{
	[CreateAssetMenu(fileName = "New Item Collection", menuName = "Inventory/Item Collection")]
	public class ItemCollection : ScriptableObject
	{
		public Item[] Items = new Item[0];
	}
}