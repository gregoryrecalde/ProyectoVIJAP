using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ParticleSystem damageEffect;
    public Transform damageZone;

    public float damage = 25;
    public float areaDamage = 1;
    // Update is called once per frame

    public void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(damageZone.transform.position, areaDamage);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyProperties enemyProperties = other.gameObject.GetComponent<EnemyProperties>();
                if (enemyProperties == null)
                {
                    enemyProperties = other.gameObject.transform.parent.gameObject.GetComponent<EnemyProperties>();
                }
                if (enemyProperties != null)
                {
                    enemyProperties.SetLife(-damage);
                    Vector3 damageEffectPosition = other.gameObject.transform.position;
                    damageEffectPosition.z -= 1f;
                    Instantiate(damageEffect, damageEffectPosition, Quaternion.identity);
                }
            }
        }

    }

    void DebugAttack()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(damageZone.position, areaDamage);

    }
    void OnDrawGizmos()
    {
        DebugAttack();
    }

    /*void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.CompareTag("Enemy"))
         {
             EnemyProperties enemyProperties = other.gameObject.GetComponent<EnemyProperties>();
             if (enemyProperties == null)
             {
                 enemyProperties = other.gameObject.transform.parent.gameObject.GetComponent<EnemyProperties>();
             }
             if (enemyProperties != null)
             {
                 enemyProperties.SetLife(-damage);
                 Instantiate(damageEffect, effectContainer.position, Quaternion.identity);
             }
         }
     }*/
}
