using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFollowing : SteeringBehaviour
{
    public bool isFollowingLeader = true;

    public float arriveRadius = 0.0f;
    public float sepRadius = 1.0f;
    public float sightRadius = 1.0f;
    public float aliRadius = 1.0f;
    public float cohRadius = 1.0f;

    public float distToCircle = 1.0f;
    public float evadeWeight = 1.0f;
    public float seekWeight = 1.0f;
    public float sepWeight = 1.0f;
    public float cohWeight = 1.0f;
    public float aliWeight = 1.0f;

    public bool isFlocking = true;

    Vector3 targetPos;

    public void setTargetPosition(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void setLeaderFollowingWeight(float weight)
    {
        evadeWeight = weight;
        seekWeight = weight;
    }

    public void setFlocking(bool shouldFlock)
    {
        isFlocking = shouldFlock;
    }
    

    public void setFollowing(bool shouldFollow)
    {
        isFollowingLeader = shouldFollow;
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform coordTransform = GameObject.Find("Coordinator").transform;
        target = coordTransform;
    }
    private Vector3 Seek(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;

        Vector3 desiredVelocity = direction.normalized * maxSpeed;

        float distance = direction.magnitude;
        if(distance < arriveRadius)
        {
            desiredVelocity *= distance / arriveRadius;
        }

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

        steer = steer / mass;

        return steer;
    }
    private Vector3 Separate()
    {
        Vector3 totalFlee = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);

            if (distance < sepRadius && distance > 0.0f)
            {
                Vector3 fleeVector = rb.position - i.position;
                fleeVector.Normalize();
                fleeVector /= distance;
                totalFlee += fleeVector;
                neighbourhoodCount++;
            }
        }


        Vector3 sepForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 desiredVelocity = totalFlee / neighbourhoodCount;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            sepForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            sepForce = sepForce / mass;
        }

        return sepForce;
    }


    private bool ShouldEvade(Vector3 leaderPosition, Vector3 sightPosition)
    {
        return Vector3.Distance(leaderPosition, transform.position) < sightRadius || Vector3.Distance(sightPosition, transform.position) < sightRadius;
    }

    private Vector3 Evade()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        float speed = rb.velocity.magnitude;

        float T = distance / speed; //target is close
        
        //calculate the future position the vehicle will pursue towards
        Vector3 futurePosition = target.position + target.gameObject.GetComponent<Rigidbody>().velocity * T;


        /*now we seek towards this future position*/
        Vector3 directionToFuturePosition = transform.position - futurePosition;

        Vector3 desiredVelocity = directionToFuturePosition.normalized * maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

        steer = steer / mass;

        return steer;
    }

    private Vector3 Align()
    {
        Vector3 totalVelocity = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);

            if (distance < aliRadius && distance > 0.0f)
            {
                totalVelocity += i.GetComponent<Rigidbody>().velocity;
                neighbourhoodCount++;
            }
        }


        Vector3 aliForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 desiredVelocity = totalVelocity / neighbourhoodCount;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            aliForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            aliForce = aliForce / mass;
        }

        return aliForce;
    }

    private Vector3 Cohesion()
    {
        Vector3 totalPosition = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);

            if (distance < cohRadius && distance > 0.0f)
            {
                totalPosition += i.position;
                neighbourhoodCount++;
            }
        }

        Vector3 cohForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 targetPos = totalPosition / neighbourhoodCount;

            Vector3 direction = targetPos - transform.position;

            Vector3 desiredVelocity = direction.normalized * maxSpeed;

            cohForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            cohForce = cohForce / mass;
        }

        return cohForce;
    }
    public override Vector3 CalculateSteeringForce()
    {
        
        Vector3 steer = Vector3.zero;

        if (isFlocking)
        {
            steer += Separate() * sepWeight;
            steer += Cohesion() * cohWeight;
            steer += Align() * aliWeight;

            if (isFollowingLeader)
            {
                Vector3 sightPoint = target.GetComponent<Rigidbody>().velocity;
                sightPoint = sightPoint.normalized * distToCircle;
                Vector3 followPoint = sightPoint * -1;
                followPoint += target.position;
                sightPoint += target.position;
                Debug.DrawLine(followPoint, rb.position);
                Debug.DrawLine(target.position, sightPoint);
                steer += Seek(followPoint) * seekWeight;
                if (ShouldEvade(target.position, sightPoint))
                {
                    steer += Evade() * evadeWeight;
                }
            }
        }
        else
        {
            Vector3 direction = targetPos - transform.position;

            Vector3 desiredVelocity = direction.normalized * maxSpeed;

            steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

            steer = steer / mass;

        }
        return steer;
    }
}
