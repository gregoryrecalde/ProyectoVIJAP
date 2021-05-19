using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public int foodRequired = 0;
    public int waterRequired = 0;
    public int loveRequired = 0;
    public int showerRequired = 0;

    public bool onAir;
    Animator animator;
    public ParticleSystem bubbleEffect;
    public ParticleSystem happyEffect;
    public ParticleSystem loveEffect;
    public ParticleSystem waterEffect;

    public GameObject waterSprite;

    public GameObject loveSprite;

    public GameObject foodSprite;

    public GameObject showerSprite;

    bool petObteined = false;

    public Color[] bodyColor;
    public Color[] necklaceColor;

    public GameObject catMesh;

    public BoxCollider boxCollider;

    public bool setRandomRequirements = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (setRandomRequirements) RandomRequirements();
        RandomColors();
        Game.goalLevel++;
    }

    void RandomColors()
    {
        SkinnedMeshRenderer mesh = catMesh.GetComponent<SkinnedMeshRenderer>();
        foreach (Material material in mesh.materials)
        {
            if (material.name == "bodyCat (Instance)") material.SetColor("_BaseColor", bodyColor[Random.Range(0, bodyColor.Length)]);
            else if (material.name == "necklace (Instance)") material.SetColor("_BaseColor", necklaceColor[Random.Range(0, necklaceColor.Length)]);
        }
    }

    void RandomRequirements()
    {
        foodRequired = Random.Range(0, 2);
        waterRequired = Random.Range(0, 2);
        loveRequired = Random.Range(0, 2);
        showerRequired = Random.Range(0, 2);

        if (FoodFull() && LoveFull() && WaterFull() && ShowerFull())
        {
            waterRequired = 1;
        }
    }

    void Update()
    {

        if (loveRequired <= 0) loveSprite.SetActive(false);

        if (showerRequired <= 0) showerSprite.SetActive(false);

        if (foodRequired <= 0) foodSprite.SetActive(false);

        if (waterRequired <= 0) waterSprite.SetActive(false);

        if (FoodFull() && LoveFull() && WaterFull() && ShowerFull())
        {
            animator.Play("Walk");
            transform.Translate(-Vector3.forward * Time.deltaTime, Space.World);
            if (!petObteined)
            {
                if (onAir)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    boxCollider.enabled = true;
                }
                Game.pets++;
                petObteined = true;
            }
            if (transform.position.z < -20) Destroy(gameObject);
        }
    }

    public void PlayEffect(string name)
    {
        switch (name)
        {
            case "food": happyEffect.Play(); break;
            case "shower": bubbleEffect.Play(); break;
            case "water": waterEffect.Play(); break;
            case "love": loveEffect.Play(); break;
            default: break;
        }
    }

    public bool FoodFull()
    {
        return foodRequired == 0;
    }
    public bool LoveFull()
    {
        return loveRequired == 0;
    }
    public bool WaterFull()
    {
        return waterRequired == 0;
    }
    public bool ShowerFull()
    {
        return showerRequired == 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("IsInteracting", true);
            PlayerController.canAction = false;
            GameUIManager.ShowAnimalGUI();
            CameraController.SetInteractiveCamera(true);
            Game.currentPet = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("IsInteracting", false);
            PlayerController.canAction = true;
            GameUIManager.HideAnimalGUI();
            Game.currentPet = null;
            CameraController.SetInteractiveCamera(false);
        }
    }
}
