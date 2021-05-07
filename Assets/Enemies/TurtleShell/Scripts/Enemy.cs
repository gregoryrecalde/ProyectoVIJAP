﻿using System.Collections;
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

    bool attack1 = false;

    public Transform playerCheck;
    bool playerHitDetect = false;
    public float playerHitMaxDistance = 5;
    RaycastHit playerHit;

    public float attackDistance = 2;
    public float minFollowDistance = 2;
    public float maxFollowDistance = 2;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyProperties = GetComponent<EnemyProperties>();
        lifeBar.maxValue = enemyProperties.GetLife();
        lifeBar.value = enemyProperties.GetLife();
    }

    void Vision()
    {
        playerHitDetect = Physics.BoxCast(playerCheck.position, playerCheck.transform.localScale, playerCheck.transform.forward, out playerHit, playerCheck.rotation, playerHitMaxDistance);
        if (playerHitDetect)
        {
            if (playerHit.collider.tag == "Player")
            {
                isAlert = true;
                Vector3 playerPosition = playerHit.point;
                float currentDistance = Mathf.Abs(transform.position.x - playerPosition.x);
                if (currentDistance > minFollowDistance && currentDistance <= maxFollowDistance)
                {
                    transform.Translate(transform.right * Time.deltaTime);
                }
                if (currentDistance <= attackDistance)
                {
                    target = playerHit.transform.gameObject;
                    attack1 = true;
                }
                else attack1 = false;
            }
        }
        else
        {
            isAlert = false;
            attack1 = false;
        }
    }
    void DrawPlayerCast()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (playerHitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(playerCheck.position, playerCheck.transform.forward * playerHit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(playerCheck.position + playerCheck.transform.forward * playerHit.distance, playerCheck.transform.localScale);
        }

        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(playerCheck.position, playerCheck.transform.forward * playerHitMaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(playerCheck.position + playerCheck.transform.forward * playerHitMaxDistance, playerCheck.transform.localScale);
        }
    }
    void Effect()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame

    // Update is called once per frame

    public ParticleSystem shellAttackEffect;
    public Transform shellAttackZone;

    public float shellAttackDamageAmount = 25;
    public float shellAttackArea = 1;
    public void ShellAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(shellAttackZone.transform.position, shellAttackArea);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                if (playerController != null && !playerController.isDied)
                {
                    playerController.GetHit(shellAttackDamageAmount);
                    Vector3 shellAttackEffectPosition = playerController.gameObject.transform.position;
                    shellAttackEffectPosition.z -= 1f;
                    shellAttackEffectPosition.y += playerController.controller.height / 2;
                    Instantiate(damageEffect, shellAttackEffectPosition, Quaternion.identity);
                }
            }
        }

    }

    bool ShellTrigger()
    {
        Collider[] hitColliders = Physics.OverlapSphere(shellAttackZone.transform.position, shellAttackArea);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public ParticleSystem damageEffect;
    public Transform damageZone;

    public float damageAmount = 25;
    public float areaDamage = 1;
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
                    playerController.GetHit(damageAmount);
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

    void DebugShellTrigger()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(shellAttackZone.position, shellAttackArea);
    }
    void OnDrawGizmos()
    {
        DebugAttack();
        DebugShellTrigger();
        DrawPlayerCast();
    }
    void Update()
    {
        if (!die)
        {
            Vision();
            if (attack1) animator.Play("Attack01");
            else if (ShellTrigger()) animator.Play("ShellAttack");

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
