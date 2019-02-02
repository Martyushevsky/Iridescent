using System.Collections.Generic;
using UnityEngine;

namespace Geekbrains
{
	[System.Serializable]
	public class Stat
	{
		public delegate void StatChanged(int value);
		public event StatChanged OnStatChanged;

		[SerializeField] private int _baseValue;

		private readonly List<int> _modifiers = new List<int>();

		public int GetValue()
		{
			var finalValue = _baseValue;
			_modifiers.ForEach(x => finalValue += x);
			return finalValue;
		}

		public void AddModifier(int modifier)
		{
			if (modifier != 0)
			{
				_modifiers.Add(modifier);
				OnStatChanged?.Invoke(GetValue());
			}
		}

		public void RemoveModifier(int modifier)
		{
			if (modifier != 0)
			{
				_modifiers.Remove(modifier);
				OnStatChanged?.Invoke(GetValue());
			}
		}
	}
}