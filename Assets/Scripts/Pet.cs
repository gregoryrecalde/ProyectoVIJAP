using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public int foodRequired = 3;
    public int waterRequired = 3;
    public int loveRequired = 3;
    public int showerRequired = 3;
    Animator animator;
    public ParticleSystem bubbleEffect;
    public ParticleSystem happyEffect;
    public ParticleSystem loveEffect;
    public ParticleSystem waterEffect;

    bool petObteined = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (FoodFull() && LoveFull() && WaterFull() && ShowerFull())
        {
            animator.Play("Walk");
            transform.Translate(-Vector3.forward * Time.deltaTime, Space.World);
            Debug.Log("transform.position.z " + transform.position.z);
            if (!petObteined)
            {
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
