using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    EnemyProperties enemyProperties;
    public ParticleSystem particleSystem;
    Animator animator;
    bool die = false;

    public GameObject target;

    bool isAlert = false;

    public float speed = 1;
    public Slider lifeBar;

    // Variables para detectar al player
    public Transform playerCheck;
    bool playerHitDetect = false;
    public float playerHitMaxDistance = 5;
    RaycastHit playerHit;

    public float speedRotation = 5;

    public float attackDistance = 2;
    public float minFollowDistance = 2;
    public float maxFollowDistance = 2;
    string attackName = "Attack01";

    //Variables del ataque
    public ParticleSystem damageEffect;
    public Transform damageZone;

    public float damageAmount = 25;
    public float areaDamage = 1;
    int random = 1;
    bool attack = false;
    public AudioClip[] audioClip;
    AudioSource audioSource;
    public float timeAttack = 0;
    float nextAttack = 0;
    public float frequencyAttack = 3;
    void Start()
    {
        Game.goalLevel++;
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
                if (!attack) isAlert = true;
                else isAlert = false;
                Vector3 playerPosition = playerHit.point;
                float currentDistance = Mathf.Abs(transform.position.x - playerPosition.x);

                if (currentDistance > minFollowDistance && currentDistance <= maxFollowDistance)
                {

                    transform.Translate(transform.right * Time.deltaTime * -transform.forward.x * speed);
                }
                if (currentDistance <= attackDistance)
                {
                    target = playerHit.transform.gameObject;
                    attack = true;
                }
                else
                {
                    target = null;
                    attack = false;
                }
                return;
            }
            else
            {
                target = null;
                isAlert = false;
                attack = false;
            }
        }

        else
        {
            playerHitDetect = Physics.BoxCast(playerCheck.position, playerCheck.transform.localScale, -playerCheck.transform.forward, out playerHit, playerCheck.rotation, playerHitMaxDistance, 3);
            target = null;
            isAlert = false;
            attack = false;
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

        damageEffect.Play();
        Collider[] hitColliders = Physics.OverlapSphere(damageZone.transform.position, areaDamage);
        foreach (var other in hitColliders)
        {
            Debug.Log(other.gameObject);
            if (other.gameObject.CompareTag("Player"))
            {
                PlaySound(audioClip[0]);
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                Debug.Log("Player controller " + playerController);
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
        timeAttack += Time.deltaTime;
        if (!die && Game.state == 1)
        {
            Vision();
            if (attack)
            {
                if (target != null)
                {
                    random = Random.Range(1, 15);
                    if (random <= 2) attackName = "Attack01";
                    else if (random <= 4) attackName = "Attack02";
                    else attackName = "Attack03";
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0)
                        && !target.GetComponent<PlayerController>().isDied && timeAttack >= nextAttack)
                    {
                        animator.SetTrigger(attackName);
                        nextAttack = Random.Range(0, 3) / frequencyAttack;
                        timeAttack = 0;
                    }

                }
            }

            if (enemyProperties.GetLife() <= 0)
            {
                die = true;
                animator.Play("Die");
                Game.pets++;
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
