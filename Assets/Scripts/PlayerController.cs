using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitMotor))]
	public class PlayerController : NetworkBehaviour
	{
		[SerializeField] private LayerMask _movementMask;

		private Character _character;
		private Camera _cam;

		private void Awake()
		{
			_cam = Camera.main;
		}
		
		public void SetCharacter(Character character, bool isLocalPlayer)
		{
			_character = character;
			if (isLocalPlayer) _cam.GetComponent<CameraController>().Target = character.transform;
		}

		private void Update()
		{
			if (!isLocalPlayer) return;
			if (_character == null) return;

			if (Input.GetMouseButtonDown(0))
			{
				if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), 
					out var hitI, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
				{
					var interactable = hitI.collider.GetComponent<Interactable>();
					if (interactable != null)
					{
						CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
					}
				}
			}

			if (!Input.GetMouseButtonDown(1)) return;
			var ray = _cam.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out var hit, 100f, _movementMask))
			{
				CmdSetMovePoint(hit.point);
			}
		}
		
		[Command]
		public void CmdSetMovePoint(Vector3 point)
		{
			_character.SetMovePoint(point);
		}

		[Command]
		public void CmdSetFocus(NetworkIdentity newFocus)
		{
			_character.SetNewFocus(newFocus.GetComponent<Interactable>());
		}
	}
}
