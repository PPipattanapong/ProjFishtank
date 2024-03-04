using UnityEngine;
using System.Collections.Generic;

public class FishMove : MonoBehaviour
{
    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private Vector3 acceleration;

    [SerializeField]
    private float mass;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private FishMove[] agents;

    [SerializeField]
    private float avoidanceForce = 1.0f;


    [SerializeField]
    private float avoidanceDist = 2.0f;

    private void Start()
    {
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
        acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        mass = 1.0f;
    }

    public void AddForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    public void SeekTo()
    {
        if (target != null)
        {
            Vector3 d = target.position - transform.position;
            Vector3 f = d - velocity;
            f = f.normalized;
            f *= 5;
            AddForce(f);
        }
    }

    private void Avoidance()
    {
        Vector3 f = Vector3.zero;

        if (agents != null)
        {
            for (int i = 0; i < agents.Length; i++)
            {
                if (agents[i] != null)
                {
                    Vector3 d = transform.position - agents[i].transform.position;
                    float dist = d.magnitude;
                    if (dist < avoidanceDist)
                    {
                        f += d;
                    }
                }
            }
        }

        f = f.normalized;
        f *= avoidanceForce;

        AddForce(f);
    }

    private void Cohesion()
    {
        Vector3 f = Vector3.zero;

        for (int i = 0; i < agents.Length; i++)
        {
            Vector3 d = agents[i].transform.position - transform.position;
            float dist = d.magnitude;
            if (dist < 10)
            {
                f += d;
            }

        }
        f = f.normalized;
        f *= 5;

        AddForce(f);
    }

    private void Alignment()
    {
        Vector3 f = Vector3.zero;

        for (int i = 0; i < agents.Length; i++)
        {
            Vector3 d = transform.position - agents[i].transform.position;
            float dist = d.magnitude;
            if (dist < 7)
            {
                f += agents[i].velocity;
            }

        }
        f = f.normalized;
        f *= 1;

        AddForce(f);
    }

    private void FixedUpdate()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        acceleration *= 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddForce(new Vector3(10, 0, 0));
        }
        SeekTo();
        Avoidance();
        Cohesion();
        Alignment();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, avoidanceDist);
    //}
}