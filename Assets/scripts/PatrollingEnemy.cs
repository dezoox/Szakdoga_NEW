using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    //private float wanderRange;
    //private float wanderTime;

    //private Transform target;
    //private NavMeshAgent agent;
    //private float timer;

    //void OnEnable()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //    timer = wanderTime;
    //}
    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= wanderTime)
    //    {
    //        Vector3 newPosition = WanderingRandomPosition(transform.position, wanderRange, -1);
    //        agent.SetDestination(newPosition);
    //        timer = 0;
    //    }
    //}

    //private Vector3 WanderingRandomPosition(Vector3 origin, float distance, int layermask)
    //{
    //    Vector3 RandomDirection = Random.insideUnitSphere * distance;
    //    RandomDirection += origin;
    //    NavMeshHit navHit;
    //    NavMesh.SamplePosition(RandomDirection, out navHit, distance, layermask);
    //    return navHit.position;
    //}
}
