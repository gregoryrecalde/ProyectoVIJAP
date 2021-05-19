using System.Collections;
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
    bool isRun = false;
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


    bool attackHitDetect = false;
    public float attackHitMaxDistance = 5;
    RaycastHit attackHit;

    public Transform groundCheck;
    bool groundHitDetect = false;
    public float groundHitMaxDistance = 5;
    RaycastHit groundHit;
    public Transform frontCheck;
    public Transform backCheck;
    bool objectHitDetect = false;
    public float visionRange = 3;
    public float wallHitMaxDistance = 0.1f;
    bool isInFrontOfTheWall = false;
    bool isBackOfTheWall = false;

    Vector3 expandColliderControllerCenter = new Vector3(0, 0.75f, 0.2f);
    float expandRadius = 0.45f;
    Vector3 normalColliderControllerCenter = new Vector3(0, 0.75f, 0f);
    float normalRadius = 0.15f;
    Vector3 move;
    [SerializeField]
    private float _rotateSpeed = 5f;
    public static bool canAction = true;

    Text timeToRespawnTxt;

    public AudioClip[] audioClips;

    AudioSource audioSource;
    // Start is called before the first frame update

    float timeToRespawn = 10.5f;
    Animator canvasAnimator;
    void Start()
    {
        playerProperties = GetComponent<PlayerProperties>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timeToRespawnTxt = GameObject.Find("timeToRespawnTxt").GetComponent<Text>();
        canvasAnimator = GameObject.Find("Game").GetComponentInChildren<Animator>();

    }

    void OnDrawGizmos()
    {
        DrawGroundCast();
        DrawFrontCheckCast();
        DrawBackCheckCast();
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

    void DrawFrontCheckCast()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(frontCheck.position, visionRange);
    }

    void DrawBackCheckCast()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(backCheck.position, visionRange);
    }


    bool IsInFrontOfTheWall()
    {
        Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, visionRange);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.tag == "Wall")
            {
                return true;

            }
        }
        return false;
    }


    bool IsBackOfTheWall()
    {
        Collider[] hitColliders = Physics.OverlapSphere(backCheck.position, visionRange);
        foreach (var other in hitColliders)
        {
            if (other.gameObject.tag == "Wall")
            {
                return true;

            }
        }
        return false;
    }
    bool IsOnGround()
    {
        groundHitDetect = Physics.BoxCast(groundCheck.position, groundCheck.transform.localScale, -groundCheck.transform.up, out groundHit, groundCheck.rotation, groundHitMaxDistance);
        if (groundHitDetect)
        {
            if (groundHit.collider.tag != "Item" && groundHit.collider.tag != "Food" && groundHit.collider.tag != "Pet")
            {
                Vector3 groundPosition = groundHit.point;
                if (Mathf.Abs(groundCheck.position.y - groundPosition.y) <= 0.2)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void Update()
    {

        Vector3 positionAux = transform.position;
        positionAux.z = 0;
        transform.position = positionAux;

        if (Game.state == 2)
        {
            canAction = false;
        }

        if (!isDied)
        {
            groundedPlayer = IsOnGround();

            isInFrontOfTheWall = IsInFrontOfTheWall();
            isBackOfTheWall = IsBackOfTheWall();

            animator.SetBool("IsGrounded", groundedPlayer);

            animator.SetFloat("VelocityY", playerVelocity.y);


            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            if (isInFrontOfTheWall)
            {
                controller.Move(1f * -transform.forward * Time.deltaTime);
                controller.center = expandColliderControllerCenter;
                controller.radius = expandRadius;
            }
            else if (isBackOfTheWall)
            {
                controller.Move(1f * transform.forward * Time.deltaTime);
                controller.center = expandColliderControllerCenter;
                controller.radius = expandRadius;
            }
            else
            {
                controller.center = normalColliderControllerCenter;
                controller.radius = normalRadius;
            }

            if (isDefending)
            {
                currentPlayerSpeed = playerSpeed / 4;
                if (Vector3.Dot(gameObject.transform.forward, move) < 0) animator.SetFloat("DefendingVelocity", -1);
                else animator.SetFloat("DefendingVelocity", 1);
            }
            else if (isRun)
            {
                currentPlayerSpeed = playerSpeed;
            }

            else currentPlayerSpeed = playerSpeed / 2f;

            controller.Move(move * Time.deltaTime * currentPlayerSpeed);

            if (!groundedPlayer) animator.SetFloat("VelocityX", 0);
            else if (!isRun && move.x != 0)
            {
                animator.SetFloat("VelocityX", 0.3f);
            }
            else animator.SetFloat("VelocityX", Mathf.Abs(move.x));

            if (move != Vector3.zero && !isDefending)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer && canAction)
            {
                PlaySound(audioClips[1]);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);

            animator.SetFloat("VelocityY", controller.velocity.y);
            if (canAction) Actions();
            CheckHealth();
        }
        else
        {
            if (canAction)
            {
                timeToRespawn -= Time.deltaTime;
                timeToRespawnTxt.text = ((int)timeToRespawn).ToString();
                if (timeToRespawn <= 0)
                {
                    Game.state = 3;
                    canAction = false;
                    Invoke("GoToMenu", 5);
                }
                else timeToRespawn -= Time.deltaTime;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);
        }
        if (isDied && Input.anyKeyDown && !isDieRecovering && canAction) Respawn();

    }

    void GoToMenu()
    {
        SceneController.LoadScene("MainMenu");
    }
    public void WeaponAttack()
    {
        weapon.Attack();
    }

    void CheckHealth()
    {
        lifeBar.value = playerProperties.GetLife();
        if (playerProperties.GetLife() <= 0)
        {
            {
                isDied = true;
                animator.SetBool("IsDied", isDied);
                animator.Play("Die");
                canvasAnimator.SetTrigger("Die");
            }
        }
    }
    void Actions()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlaySound(audioClips[0]);
            }
            isDefending = true;
        }
        else isDefending = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = false;
        }
        else
        {
            isRun = true;
        }

        animator.SetBool("IsDefending", isDefending);

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.Play("Attack01");
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            animator.Play("Attack02");
        }
    }

    public void Respawn()
    {
        if (!isDieRecovering)
        {
            Game.state = 1;
            Game.respawns++;
            canvasAnimator.SetTrigger("Battle");
            isDied = true;
            animator.SetBool("IsDied", isDied);
            GameObject go = Instantiate(dieRecoverEffect);
            go.transform.parent = transform;
            go.transform.position = transform.position;
            animator.Play("DieRecoverCustom");
            isDieRecovering = true;
        }

    }
    void DieRecover()
    {
        playerProperties.ResetLife();
        isDieRecovering = false;
        isDied = false;
        animator.SetBool("IsDied", isDied);
        timeToRespawn = 10.5f;
    }


    public void GetHit(float damage)
    {
        playerProperties.SetLife(-damage);
        animator.Play("GetHit");
    }

    void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
