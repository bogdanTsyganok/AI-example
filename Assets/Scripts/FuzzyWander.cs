using UnityEngine;

public class FuzzyWander : SteeringBehaviour
{
    Vector3 objectDirection = Vector3.zero;
    float collisionAvoidWeight = 1.0f;
    public float RayLength;

    private void Start()
    {
        objectDirection.z = 1;
    }

    public override Vector3 CalculateSteeringForce()
    {

        Vector3 desiredVelocity = Vector3.zero;

        Vector3 actualObjectDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.right;
        Debug.DrawLine(this.transform.position, this.transform.position + (rb.velocity.normalized * 4), Color.blue);
        for (float angle = -70.0f; angle < 71.0f; angle += 35.0f) {
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * actualObjectDirection;
            Vector3 reflectedDirection = Quaternion.AngleAxis(-angle, Vector3.up) * actualObjectDirection;
            direction.Normalize();
            reflectedDirection.Normalize();
            RaycastHit hitinfo = new RaycastHit();
            Physics.Raycast(this.transform.position, direction, out hitinfo, RayLength);
            Debug.DrawLine(this.transform.position, this.transform.position + (direction * RayLength), Color.red);
            if(angle > -10.0f && angle < 10.0f)
            {
                if (hitinfo.collider != null)
                {
                    //desiredVelocity += Quaternion.AngleAxis(45.0f * (collisionAvoidWeight > 3.0f ? 3.0f : collisionAvoidWeight), Vector3.up) * direction / Vector3.Distance(hitinfo.point, transform.position) * maxSpeed * collisionAvoidWeight;
                    //desiredVelocity.y = 0;
                    //collisionAvoidWeight *= 1.01f;
                    //if(hitinfo.collider.tag == "Player")
                    //{
                    //    Debug.Log("Player collision imminent");
                    //}
                    desiredVelocity += -direction * maxSpeed * RayLength / Vector3.Distance(hitinfo.point, transform.position); 
                }
                else
                {
                    desiredVelocity += direction * maxSpeed;
                    //collisionAvoidWeight = 1.0f;
                }
            }
            else
            {
                if (hitinfo.collider != null)
                {

                    if (hitinfo.collider.tag == "Player")
                    {
                        Debug.Log("Player Peripheral collision imminent");
                    }
                    //desiredVelocity += reflectedDirection * maxSpeed / Vector3.Distance(hitinfo.point, transform.position) / collisionAvoidWeight;

                    desiredVelocity += -direction * maxSpeed / Vector3.Distance(hitinfo.point, transform.position);
                }
                else
                {
                    //desiredVelocity = direction * maxSpeed;
                }
            }
        }
        desiredVelocity.y = 0;
        Debug.DrawLine(transform.position, transform.position + desiredVelocity, Color.green);
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
        steer = steer / mass;

        return steer;
    }
}
