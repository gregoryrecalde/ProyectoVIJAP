using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemType;
    bool isObteined = false;
    public ParticleSystem particleSystem;
    Animator animator;

    public AudioClip obteinedSfx;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isObteined)
        {
            animator.Play("ItemObteined");
            Game.PlaySound(obteinedSfx);
            isObteined = true;
            Game.IncreaseItem(ItemType);
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
