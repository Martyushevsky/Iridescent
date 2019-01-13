using UnityEngine;

namespace Geekbrains
{
	[System.Serializable]
	public class Stat
	{
		[SerializeField] private int _baseValue;

		public int GetValue()
		{
			return _baseValue;
		}
	}
}