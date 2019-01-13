using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Geekbrains
{
	public class Test: MonoBehaviour
	{
		Dictionary<Type, object> _managers = new Dictionary<Type, object>();

		public void Registr(object o)
		{
			_managers.Add(o.GetType(), o);
		}

		private void Start()
		{
			void Test(GameObject obj, string name)
			{
				obj.name = name;
				var rendererObj = obj.GetComponent<Renderer>();
				if (rendererObj != null) rendererObj.material.color = Random.ColorHSV();
			}

			var sceneGo = FindObjectsOfType<GameObject>();
			foreach (var o in sceneGo)
			{
				Test(o, "Roman");
			}

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out var hit, 100f))
			{
				var temp = hit.collider.GetComponent<MonoBehaviour>();
				switch (temp)
				{
					case PlayerController boxCollider:
						break;
					case PlayerStats capsuleCollider:
						break;
				}


				if (hit.collider.GetComponent<PlayerController>())
				{
					
				}
				else if (hit.collider.GetComponent<PlayerStats>())
				{

				}
			}
		}
	}
}