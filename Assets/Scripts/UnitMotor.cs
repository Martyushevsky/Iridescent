using UnityEngine;
using UnityEngine.AI;

namespace Geekbrains
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class UnitMotor : MonoBehaviour
	{
		private NavMeshAgent _agent;
		private Transform _target;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
		}

		private void Update()
		{
			if (_target == null) return;
			if (_agent.velocity.magnitude == 0) FaceTarget();
			_agent.SetDestination(_target.position);
		}

		public void MoveToPoint(Vector3 point)
		{
			_agent.SetDestination(point);
		}

		private void FaceTarget()
		{
			var direction = _target.position - transform.position;
			var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
		}

		public void FollowTarget(Interactable newTarget)
		{
			_agent.stoppingDistance = newTarget.Radius;
			_target = newTarget.InteractionTransform;
		}

		public void StopFollowingTarget()
		{
			_agent.stoppingDistance = 0f;
			_agent.ResetPath();
			_target = null;
		}

		public void SetMoveSpeed(int speed)
		{
			_agent.speed = speed;
		}
	}
}
