using UnityEngine;

namespace Tests
{
    public class MoveSphere : MonoBehaviour
    {
	    public Transform Target;
	    private int t;

	    private void Start()
	    {
			t = Random.Range(2, 5);
		}

		private void LateUpdate()
	    {
			transform.position = new Vector3(transform.position.x,3+ Mathf.Sin(Time.fixedTime)*t, transform.position.z);
			transform.Rotate(0, 2.0f, 0);
		}
    }
}
