using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    EnemyProperties enemyProperties;
    public ParticleSystem particleSystem;
    Animator animator;
    bool die = false;

    public GameObject target;

    public Transform eyes;

    bool isAlert = false;

    public float speed = 1;
    public Slider lifeBar;


    public float distanceAttack = 2;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyProperties = GetComponent<EnemyProperties>();
        lifeBar.maxValue = enemyProperties.GetLife();
        lifeBar.value = enemyProperties.GetLife();
    }

    bool CheckPlayer(float distance)
    {
        Ray ray = new Ray(eyes.transform.position, eyes.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Player")
            {
                Vector3 playerPosition = hitInfo.point;
                //Si la distancia con el jugador es <= distance
                if (Mathf.Abs(transform.position.x - playerPosition.x) <= distance)
                {
                    target = hitInfo.transform.gameObject;
                    return true;
                }
            }
        }
        return false;
    }
    void Effect()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame

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
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                if (playerController != null && !playerController.isDied)
                {
                    playerController.GetHit(damage);
                    Vector3 damageEffectPosition = playerController.gameObject.transform.position;
                    damageEffectPosition.z -= 1f;
                    damageEffectPosition.y += playerController.controller.height / 2;
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
    void Update()
    {
        if (!die)
        {
            if (CheckPlayer(5))
            {
                isAlert = true;
            }
            else isAlert = false;


            if (CheckPlayer(2))
            {
                animator.Play("Attack01");
            }


            if (enemyProperties.GetLife() <= 0)
            {
                die = true;
                animator.Play("Die");
            }
            lifeBar.value = enemyProperties.GetLife();
            animator.SetBool("IsAlert", isAlert);
        }

    }
}
