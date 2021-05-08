using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public Test test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3((float)test.positionX, 0f, (float)test.positionY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
