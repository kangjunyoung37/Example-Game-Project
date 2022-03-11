using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossmissile : bullet
{

    public Transform target;
    NavMeshAgent nav;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);

    }
}
