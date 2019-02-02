using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
	public class StatItem : MonoBehaviour
	{
		[SerializeField] private Text _value;
		[SerializeField] private Button _upgradeButton;

		public void ChangeStat(int stat)
		{
			_value.text = stat.ToString();
		}

		public void SetUpgradable(bool upgradable)
		{
			_upgradeButton.gameObject.SetActive(upgradable);
		}
	}
}