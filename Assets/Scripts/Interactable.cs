using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class Interactable : NetworkBehaviour
	{
		public Transform InteractionTransform;
		public float Radius = 2f;

		private void OnValidate()
		{
			InteractionTransform = transform;
		}

		public bool HasInteract { get; protected set; } = true;

		public virtual bool Interact(GameObject user) => false;

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(InteractionTransform.position, Radius);
		}
	}
}