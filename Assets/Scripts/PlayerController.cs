using UnityEngine;
using UnityEngine.EventSystems;
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
            if (isLocalPlayer)
            {
                _cam.GetComponent<CameraController>().Target = character.transform;
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (_character == null) return;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // при нажатии на правую кнопку мыши перемещаемся в указанную точку
                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100f, _movementMask))
                    {
                        CmdSetMovePoint(hit.point);
                    }
                }
                // при нажатии на левую кнопку мыши взаимодействуем с объектами
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                        }
                    }
                }
            }
            // использование навыков
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Input.GetButtonDown("Skill1")) CmdUseSkill(0);
                if (Input.GetButtonDown("Skill2")) CmdUseSkill(1);
                if (Input.GetButtonDown("Skill3")) CmdUseSkill(2);
            }
        }

        [Command]
        void CmdSetMovePoint(Vector3 point)
        {
            if (!_character.unitSkills.inCast) _character.SetMovePoint(point);
        }

        [Command]
        void CmdSetFocus(NetworkIdentity newFocus)
        {
            if (!_character.unitSkills.inCast)
                _character.SetNewFocus(newFocus.GetComponent<Interactable>());
        }

        [Command]
        void CmdUseSkill(int skillNum)
        {
            Debug.Log("using skill");
            if (!_character.unitSkills.inCast) _character.UseSkill(skillNum);
        }
    }
}
