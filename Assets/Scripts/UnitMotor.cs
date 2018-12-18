using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour
{
	private NavMeshAgent _agent;

	private void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	public void MoveToPoint(Vector3 point)
	{
		_agent.SetDestination(point);
	}
}
