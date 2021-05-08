using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    Rigidbody rb;
    public float force;
    public Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * 10 * force * Time.deltaTime);
    }
}
