using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private Transform tr;
    public float coeff;
    private float dt;
    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        tr.Rotate(Vector3.up * coeff * dt);
    }
}
