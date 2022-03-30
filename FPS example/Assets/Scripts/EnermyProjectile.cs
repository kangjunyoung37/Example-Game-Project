using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyProjectile : MonoBehaviour
{
    private MovementTransform movement;
    private float projectileDistance = 30;
    private int damage = 5;


    public void Setup(Vector3 position)
    {
        movement = GetComponent<MovementTransform>();
        StartCoroutine("OnMove", position);
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 start = transform.position;
        movement.MoveTo((targetPosition - transform.position).normalized);
        while (true)
        {
            if (Vector3.Distance(transform.position,start) >= projectileDistance)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
