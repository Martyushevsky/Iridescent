using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour {

    NavMeshAgent agent;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }
}
