using UnityEngine;

[RequireComponent(typeof(UnitMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] LayerMask movementMask;

    Camera cam;
    UnitMotor motor;

    void Start () {
        cam = Camera.main;
        motor = GetComponent<UnitMotor>();
        cam.GetComponent<CameraController>().target = transform;
    }
	
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, movementMask)) {
                motor.MoveToPoint(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) {

            }
        }
    }
}
