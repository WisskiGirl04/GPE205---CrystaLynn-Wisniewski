using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    private Rigidbody objectBody;

    // Start is called before the first frame update
    public override void Start()
    {
        // Get Rigidbody Component
        objectBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        objectBody.MovePosition(objectBody.position + moveVector);
    }

    public override void Rotate(float speed)
    {
        transform.Rotate(0, (speed * Time.deltaTime), 0);
    }
}
