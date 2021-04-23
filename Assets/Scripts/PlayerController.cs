﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    Animator animator;
    //PlayerProperties es la clase donde almacenaremos la vida, magia, etc.
    PlayerProperties playerProperties;
    //https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
    public CharacterController controller;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public float playerSpeed = 5.0f;

    float currentPlayerSpeed = 0;
    public float jumpHeight = 2.5f;
    public float gravityValue = -12f;

    public ParticleSystem shieldBlockEffect;
    public GameObject dieRecoverEffect;

    public Weapon weapon;
    public Slider lifeBar;

    public bool isDied = false;
    public bool isDefending = false;

    bool isDieRecovering = false;
    public Collider shieldCollider;

    private Quaternion _targetRot = Quaternion.identity;

    public Transform groundCheck;

    bool attackHitDetect = false;
    public float attackHitMaxDistance = 5;
    RaycastHit attackHit;

    bool groundHitDetect = false;
    public float groundHitMaxDistance = 5;
    RaycastHit groundHit;
    Vector3 move;
    [SerializeField]
    private float _rotateSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerProperties = GetComponent<PlayerProperties>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void OnDrawGizmos()
    {
        DrawGroundCast();
    }

    void DrawAttackCast()
    {

    }
    void DrawGroundCast()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (groundHitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(groundCheck.position, -groundCheck.transform.up * groundHit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(groundCheck.position - groundCheck.transform.up * groundHit.distance, groundCheck.transform.localScale);
        }

        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(groundCheck.position, -groundCheck.transform.up * groundHitMaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(groundCheck.position - groundCheck.transform.up * groundHitMaxDistance, groundCheck.transform.localScale);
        }
    }
    bool IsGrounded()
    {
        groundHitDetect = Physics.BoxCast(groundCheck.position, groundCheck.transform.localScale, -groundCheck.transform.up, out groundHit, groundCheck.rotation, groundHitMaxDistance);
        if (groundHitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit tag: " + groundHit.collider.tag);
            Vector3 groundPosition = groundHit.point;
            //Debug.Log(Mathf.Abs(groundCheck.position.y - groundPosition.y));
            //Si la distancia con el jugador es <= distance
            if (Mathf.Abs(groundCheck.position.y - groundPosition.y) <= 0.1)
            {
                return true;
            }

        }
        return false;
    }
    void Update()
    {
        if (!isDied)
        {
            groundedPlayer = IsGrounded();
            animator.SetBool("IsGrounded", groundedPlayer);

            animator.SetFloat("VelocityY", playerVelocity.y);

            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            if (isDefending)
            {
                currentPlayerSpeed = playerSpeed / 4;
                if (Vector3.Dot(gameObject.transform.forward, move) < 0) animator.SetFloat("DefendingVelocity", -1);
                else animator.SetFloat("DefendingVelocity", 1);
            }
            else currentPlayerSpeed = playerSpeed;

            controller.Move(move * Time.deltaTime * currentPlayerSpeed);

            if (!groundedPlayer) animator.SetFloat("VelocityX", 0);
            else animator.SetFloat("VelocityX", Mathf.Abs(move.x));

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);


            animator.SetFloat("VelocityY", controller.velocity.y);
            Actions();
        }
    }

    public void WeaponAttack()
    {
        weapon.Attack();
    }
    void Actions()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            isDefending = true;
            shieldCollider.isTrigger = false;
        }
        else
        {
            shieldCollider.isTrigger = true;
            isDefending = false;
        }

        animator.SetBool("IsDefending", isDefending);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.Play("Attack01");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.Play("Attack02");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            controller.Move(-transform.forward * 1000000);
        }

        lifeBar.value = playerProperties.GetLife();

        if (playerProperties.GetLife() <= 0)
        {
            {
                isDied = true;
                animator.SetBool("IsDied", isDied);
                animator.Play("Die");
            }
        }
        else if (Game.lives >= 0)
        {

            if (Game.lives == 0 && isDied && !isDieRecovering)
            {
                Game.GameOver();
            }
            else if (Input.GetKey(KeyCode.R) && !isDieRecovering)
            {
                GameObject go = Instantiate(dieRecoverEffect);
                go.transform.position = transform.position;
                animator.Play("DieRecoverCustom");
                Game.UseLive();
                isDieRecovering = true;
            }
        }
    }
    void DieRecover()
    {
        playerProperties.ResetLife();
        isDied = false;
        isDieRecovering = false;
    }


    public void GetHit(float damage)
    {
        playerProperties.SetLife(-damage);
        animator.Play("GetHit");
    }
}
