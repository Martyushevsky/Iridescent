using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
	
	void FixedUpdate () {
		if (!agent.hasPath) {
            animator.SetBool("Moving", false);
        } else {
            animator.SetBool("Moving", true);
        }
    }

    //Placeholder functions for Animation events
    void Hit() {
    }

    void FootR() {
    }

    void FootL() {
    }
}
