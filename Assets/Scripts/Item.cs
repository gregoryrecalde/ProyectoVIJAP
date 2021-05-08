using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public int id;
    bool isObteined = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isObteined)
        {
            isObteined = true;
            Game.items[id]=isObteined;
            animator.Play("Obteined");
        }
    }

    void Effect()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }
}
