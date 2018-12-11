using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float radius;
    private Vector2 vel;
    private float headingAngle;
    public float wanderRadius;
    public float wanderDistance;
    public float wanderJitter;
    private Vector2 wanderTarget;
    private int neighbourCount;

    public int SeparationWeight;
    public int AlignmentWeight;
    public int CohesionWeight;
    public int WanderWeight;
    public int MaxSteeringForce;

    private List<Boid> boids = new List<Boid>();

    void Start()
    {
        wanderTarget = new Vector2(1, 0);

        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Boid"))
        {
            if (b != gameObject)
                boids.Add(b.GetComponent<Boid>());
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(SteeringForce());
        //set heading 
        vel = GetComponent<Rigidbody2D>().velocity;
        headingAngle = (Mathf.Atan2(vel.y, vel.x)) * Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().MoveRotation(headingAngle);
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        int x = (int)(transform.position.x * 0.25f);
        int y = (int)(transform.position.y * 0.25f);
        int i = ((y * 4) + x);
    }

    private Vector2 SteeringForce()
    {
        neighbourCount = 0;
        Vector2 Separation = new Vector2(0, 0);
        Vector2 Alignment = new Vector2(0, 0);
        Vector2 Cohesion = new Vector2(0, 0);
        Vector2 SteerForce = new Vector2(0, 0);

        int x_0 = (int)((transform.position.x - radius) * 0.25f);
        if (x_0 < 0)
        {
            x_0 = 0;
        }
        

        int y_0 = (int)((transform.position.y + radius) * 0.25f);
        if (y_0 > 2)
        {
            y_0 = 2;
        }

        int x_1 = (int)((transform.position.x + radius) * 0.25f);
        if (x_1 > 3)
        {
            x_1 = 3;
        }

        int y_1 = (int)((transform.position.y - radius) * 0.25f);
        if (y_1 < 0)
        {
            y_1 = 0;
        }

        for (int i = y_1; i <= y_0; i++)
        {
            for (int j = x_0; j <= x_1; j++)
            {
                foreach (Boid b in boids)
                {
                    if (this.GetInstanceID() != b.GetInstanceID())
                    {
                        Vector2 agentToTarget = (Vector2)(transform.position - b.transform.position); 
                        //check the radius
                        if (agentToTarget.magnitude < radius)
                        {
                            neighbourCount++;
                            //separation 
                            Separation += agentToTarget.normalized / agentToTarget.magnitude;
                            //sum of heading 
                            Alignment += (Vector2)b.transform.right;
                            //sum of mass 
                            Cohesion += (Vector2)b.transform.position;
                        }
                    }
                }
            }
        }

        if (neighbourCount > 0)
        {
            Alignment /= neighbourCount;
            Alignment -= (Vector2)transform.right;
            Cohesion /= neighbourCount;
            Cohesion = Seek(Cohesion);
        }
        
        SteerForce += AccForce(SteerForce.magnitude, Separation * SeparationWeight);
        SteerForce += AccForce(SteerForce.magnitude, Cohesion * CohesionWeight);
        SteerForce += AccForce(SteerForce.magnitude, Alignment * AlignmentWeight);
        SteerForce += AccForce(SteerForce.magnitude, Wander() * WanderWeight);

        return SteerForce;
    }

    private Vector2 Seek(Vector2 targetPo)
    {
        Vector2 DesiredVelocity = (targetPo - (Vector2)transform.position).normalized;
        return (DesiredVelocity - vel);
    }

    private Vector2 Wander()
    {
        float R = Random.Range(-10f, 10f);
        wanderTarget += (new Vector2(R, R) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector2 target = new Vector2(wanderDistance, 0);
        target += wanderTarget;
        target = transform.TransformPoint(target);
        target = (target - (Vector2)transform.position).normalized;

        return (target - vel);
    }

    private Vector2 AccForce(float SteeringMag, Vector2 ForceToAdd)
    {
        Vector2 returnForce = new Vector2(0, 0);
        float ForceToAddMag = ForceToAdd.magnitude;
        float RemainingForceMag = MaxSteeringForce - SteeringMag;
        if (RemainingForceMag <= 0) return returnForce;
        if (ForceToAddMag < RemainingForceMag) return ForceToAdd;
        else return (ForceToAdd.normalized * RemainingForceMag);
    }
}
