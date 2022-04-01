using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnermyState { None = -1 , Idle = 0, Wander,Pursuit,Attack,}
public class EnermyFSM2 : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionrange = 8;
    [SerializeField]
    private float pursuitlimitRange = 10;
    private EnermyState enermyState = EnermyState.None;
    [Header("Attack")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float attackRange = 5;
    [SerializeField]
    private float attackRate = 1;

    private float lastAttackTime = 0;
    private Status status;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private EnermyMemoryPool enermyMemoryPool;

    public void Setup(Transform target, EnermyMemoryPool enermyMemoryPool)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        this.enermyMemoryPool = enermyMemoryPool;
        navMeshAgent.updateRotation = false;
    }

    private void OnEnable()
    {
 
        ChangeState(EnermyState.Idle);
    }
    private void OnDisable()
    {
        StopCoroutine(enermyState.ToString());
        enermyState = EnermyState.None;
    }

    public void ChangeState(EnermyState newState)
    {
        if (enermyState == newState) return;
        StopCoroutine(enermyState.ToString());
        enermyState = newState;
        StartCoroutine(enermyState.ToString());
    }

    private IEnumerator Idle()
    {
        StartCoroutine("AutoChangeFromIdleToWander");
        while ( true)
        {
            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }
    private IEnumerator AutoChangeFromIdleToWander()
    {


        int changeTime = Random.Range(1, 5);
        yield return new WaitForSeconds(changeTime);
        ChangeState(EnermyState.Wander);
    }

    private IEnumerator Wander()
    {
        float currentTime = 0;
        float maxTime = 10;

        navMeshAgent.speed = status.WalkSpeed;

        navMeshAgent.SetDestination(CaculateWanderPostion());

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);
        while (true)
        { 

            currentTime += Time.deltaTime;
            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
        if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
        {
            ChangeState(EnermyState.Idle);
        }
            CalculateDistanceToTargetAndSelectState();
             yield return null;
        }
    }

    private IEnumerator Attack()
    {
        navMeshAgent.ResetPath();
        while (true)
        {
            LookRotationToTarget();
            CalculateDistanceToTargetAndSelectState();
            if(Time.time -lastAttackTime > attackRate)
            {
                lastAttackTime += Time.time;

                GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                clone.GetComponent<EnermyProjectile>().Setup(target.position);

            }
            yield return null;
        }
    }
    private Vector3 CaculateWanderPostion()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetposition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetposition.x = Mathf.Clamp(targetposition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetposition.y = 0.0f;
        targetposition.z = Mathf.Clamp(targetposition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);
        return targetposition;
    }
    private Vector3 SetAngle(float radius,int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }
    private IEnumerator Pursuit()
    {
        while (true)
        {
            navMeshAgent.speed = status.RunSpeed;
            navMeshAgent.SetDestination(target.position);
            LookRotationToTarget();
            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);
    }
    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) return;
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= attackRange)
        {
            ChangeState(EnermyState.Attack);
        }
        else if (distance <= targetRecognitionrange)
        {
            ChangeState(EnermyState.Pursuit);
        }
        else if (distance >= pursuitlimitRange)
        {
            ChangeState(EnermyState.Wander);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionrange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitlimitRange);
        Gizmos.color = new Color(0.39f,0.04f,0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }
    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        if (isDie == true)
        {
            enermyMemoryPool.DeactivateEnemy(gameObject);
        }
    }

}
