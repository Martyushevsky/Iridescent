using UnityEngine.Networking;

namespace Geekbrains
{
	public class SyncListItem : SyncList<Item>
	{
		protected override void SerializeItem(NetworkWriter writer, Item item)
		{
			writer.Write(ItemBase.GetItemId(item));
		}


		protected override Item DeserializeItem(NetworkReader reader)
		{
			return ItemBase.GetItem(reader.ReadInt32());
		}
	}
}