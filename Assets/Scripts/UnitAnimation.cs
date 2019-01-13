using UnityEngine;
using UnityEngine.AI;

namespace Geekbrains
{
	public class UnitAnimation : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private NavMeshAgent _agent;

		private void Start()
		{
			_animator = GetComponent<Animator>();
			_agent = GetComponentInParent<NavMeshAgent>();
		}

		private void FixedUpdate()
		{
			_animator.SetBool("Move", _agent.hasPath);
		}

		//Placeholder functions for Animation events
		void Hit()
		{
		}

		void FootR()
		{
		}

		void FootL()
		{
		}
	}
}
