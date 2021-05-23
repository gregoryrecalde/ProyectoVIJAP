using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyesEnemy : MonoBehaviour
{

    EnemyProperties enemyProperties;
    public ParticleSystem particleSystem;
    Animator animator;
    bool die = false;

    public GameObject target;

    bool isAlert = false;

    public float speed = 1;
    public Slider lifeBar;

    bool attack1 = false;
    bool attack2 = false;

    // Variables para detectar al player
    public Transform playerCheck;
    bool playerHitDetect = false;
    public float playerHitMaxDistance = 5;
    RaycastHit playerHit;

    public float speedRotation = 5;

    public float attackDistance = 2;
    public float attackDistance2 = 4;
    public float minFollowDistance = 2;
    public float maxFollowDistance = 2;


    //Variables del ataque
    public ParticleSystem damageEffect;
    public Transform damageZone;

    public float damageAmount = 25;
    public float areaDamage = 1;

    public AudioClip[] audioClip;
    AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyProperties = GetComponent<EnemyProperties>();
        lifeBar.maxValue = enemyProperties.GetLife();
        lifeBar.value = enemyProperties.GetLife();
        audioSource = GetComponent<AudioSource>();

    }

    void Vision()
    {
        playerHitDetect = Physics.BoxCast(playerCheck.position, playerCheck.transform.localScale, playerCheck.transform.forward, out playerHit, playerCheck.rotation, playerHitMaxDistance, 3);
        if (playerHitDetect)
        {
            if (playerHit.collider.tag == "Player")
            {
                isAlert = true;
                Vector3 playerPosition = playerHit.point;
                float currentDistance = Mathf.Abs(transform.position.x - playerPosition.x);

                if (currentDistance > minFollowDistance && currentDistance <= maxFollowDistance)
                {

                    transform.Translate(transform.right * Time.deltaTime * -transform.forward.x);
                }

                if (currentDistance > attackDistance && currentDistance <= attackDistance2)
                {

                    target = playerHit.transform.gameObject;
                    attack2 = true;
                }
                else if (currentDistance <= attackDistance)
                {
                    target = playerHit.transform.gameObject;
                    attack1 = true;
                }
                else
                {
                    target = null;
                    attack1 = false;
                    attack2 = false;
                }
                return;
            }
            else
            {
                target = null;
                isAlert = false;
                attack1 = false;
                attack2 = false;
            }
        }

        else
        {
            playerHitDetect = Physics.BoxCast(playerCheck.position, playerCheck.transform.localScale, -playerCheck.transform.forward, out playerHit, playerCheck.rotation, playerHitMaxDistance);
            target = null;
            isAlert = false;
            attack1 = false;
            attack2 = false;
            if (playerHitDetect)
            {
                if (playerHit.collider.tag == "Player")
                {
                    if (transform.forward.x <= 0f)
                    {


                        Quaternion newRotation = Quaternion.AngleAxis(90, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedRotation);
                    }
                    else
                    {


                        Quaternion newRotation = Quaternion.AngleAxis(-90, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedRotation);
                    }
                }
            }
        }


    }
    void DrawPlayerCast()
    {
        Gizmos.color = Color.green;

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
    void DieEffect()
    {
        Game.score += enemyProperties.points;
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }

    public void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(damageZone.transform.position, areaDamage);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlaySound(audioClip[0]);
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                if (playerController != null && target != null)
                {
                    bool playerIsDefending = false;
                    if ((target.transform.forward.x <= 0 && transform.forward.x <= 0) ||
                        (target.transform.forward.x >= 0 && transform.forward.x >= 0))
                    {
                        playerIsDefending = false;
                    }
                    else if (playerController.isDefending)
                    {
                        playerIsDefending = playerController.isDefending;
                    }
                    if (!playerController.isDied && !playerIsDefending)
                    {
                        playerController.GetHit(damageAmount);
                        Vector3 damageEffectPosition = playerController.gameObject.transform.position;
                        damageEffectPosition.z -= 1f;
                        damageEffectPosition.y += playerController.controller.height / 2;
                        Instantiate(damageEffect, damageEffectPosition, Quaternion.identity);
                        //Game.PlaySound("effect"); // hitOnShield Effect
                    }
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
        DrawPlayerCast();
    }
    void Update()
    {
        if (!die && Game.state == 1)
        {
            Vision();
            if (attack1)
            {
            Debug.Log("Attack 1 "+target);
                if (target != null)
                {
                    
                    if (!target.GetComponent<PlayerController>().isDied) animator.Play("Attack01");

                }
            }
            else if (attack2)
            {
                
            Debug.Log("Attack 2 "+target);
                if (target != null)
                {
                    if (!target.GetComponent<PlayerController>().isDied) animator.Play("Attack02");

                }
            }




            if (enemyProperties.GetLife() <= 0)
            {
                die = true;
                animator.Play("Die");
            }
            lifeBar.transform.localScale = new Vector3(-transform.forward.x, 1, 1);
            lifeBar.value = enemyProperties.GetLife();
            animator.SetBool("IsAlert", isAlert);

        }

    }
    void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
