using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : SteeringBehaviour
{
    Vector3 targetPos;
    public override Vector3 CalculateSteeringForce()
    {
        Vector3 direction = targetPos - transform.position;

        Vector3 desiredVelocity = direction.normalized * maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

        steer = steer / mass;

        return steer;
    }

    public void setTargetPosition(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
