using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class Interactable : NetworkBehaviour
    {
        public Transform InteractionTransform;

        public bool HasInteract { get; set; } = true;

        public virtual bool Interact(GameObject user) => false;

        [SerializeField] private float radius = 2f;

        public virtual float GetInteractDistance(GameObject user)
        {
            return radius;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(InteractionTransform.position, radius);
        }
    }
}