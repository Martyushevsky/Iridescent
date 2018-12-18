using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform _mTarget;
	[SerializeField] private Vector3 _offset;
	[SerializeField] private float _zoomSpeed = 4f;
	[SerializeField] private float _minZoom = 5f;
	[SerializeField] private float _maxZoom = 15f;
	[SerializeField] private float _pitch = 2f;

	private Transform _mTransform;
	private float _currentZoom = 10f;
	private float _currentRot = 0f;
	private float _prevMouseX;

	public Transform Target { set { _mTarget = value; } }


	private void OnValidate()
	{
		_mTarget = FindObjectOfType<PlayerController>().transform;
	}

	private void Start()
	{
		_mTransform = transform;
	}

	private void Update()
	{
		if (_mTarget != null)
		{
			_currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
			_currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

			if (Input.GetMouseButton(2))
			{
				_currentRot += Input.mousePosition.x - _prevMouseX;
			}
		}
		_prevMouseX = Input.mousePosition.x;
	}

	private void LateUpdate()
	{
		if (_mTarget != null)
		{
			_mTransform.position = _mTarget.position - _offset * _currentZoom;
			_mTransform.LookAt(_mTarget.position + Vector3.up * _pitch);
			_mTransform.RotateAround(_mTarget.position, Vector3.up, _currentRot);
		}
	}
}
