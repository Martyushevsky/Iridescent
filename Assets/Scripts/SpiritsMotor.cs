using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SpiritsMotor : NetworkBehaviour {

    [SerializeField] float speed;
    Transform _tratsform;
    Quaternion oldRot;

    void Start() {
        _tratsform = transform;
    }

    void Update () {
		if (hasAuthority) {
            oldRot = _tratsform.localRotation;
            _tratsform.localRotation = Quaternion.Euler(oldRot.eulerAngles.x, oldRot.eulerAngles.y + speed * Time.deltaTime, oldRot.eulerAngles.z);
        }
	}
}
