using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour
{
	[SerializeField] Animator _animator;
	[SerializeField] NavMeshAgent _agent;

	private void OnValidate()
	{
		_animator = GetComponent<Animator>();
		_agent = GetComponentInParent<NavMeshAgent>();
	}

	private void FixedUpdate()
	{
		_animator.SetBool("Moving", _agent.hasPath);
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
