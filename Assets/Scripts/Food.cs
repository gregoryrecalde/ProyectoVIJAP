using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public char foodType;
    bool isObteined = false;
    public ParticleSystem particleSystem;
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag=="Player" && !isObteined)
        {
            animator.Play("FoodObteined");
            isObteined = true;
            Game.IncreaseFood(foodType);
        }
    }

    void Effect()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
    }

    void DestroyFood()
    {
        Destroy(gameObject);
    }

}
