using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool boss = false;
    public static Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(boss) animator.Play("BossEntry");
    }
    
    public static void Shake()
    {
        animator.Play("Shake");
    }

    public static void SetInteractiveCamera(bool value)
    {
        animator.SetBool("IsInteracting", value);
    }
}
