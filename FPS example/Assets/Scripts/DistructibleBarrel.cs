using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistructibleBarrel :InteractionObject
{
    [Header("Destructible Barrel")]
    [SerializeField]
    private GameObject destructibleBareelPiecse;

    private bool isDestroyed = false;

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0 && isDestroyed == false)
        {
            isDestroyed = true;
            Instantiate(destructibleBareelPiecse, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }

}
