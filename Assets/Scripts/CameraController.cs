using UnityEngine;

namespace Geekbrains
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _offset;
		[SerializeField] private float _zoomSpeed = 4f;
		[SerializeField] private float _minZoom = 5f;
		[SerializeField] private float _maxZoom = 15f;
		[SerializeField] private float _pitch = 2f;

		private Transform _transform;
		private float _currentZoom = 10f;
		private float _currentRot = 0f;
		private float _prevMouseX;

		public Transform Target { set => _target = value; }

		private void Start()
		{
			_transform = transform;
		}

		private void Update()
		{
			if (_target != null)
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
			if (_target == null) return;
			_transform.position = _target.position - _offset * _currentZoom;
			_transform.LookAt(_target.position + Vector3.up * _pitch);
			_transform.RotateAround(_target.position, Vector3.up, _currentRot);
		}
	}
}
