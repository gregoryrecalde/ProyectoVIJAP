using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuController : MonoBehaviour
{
    Animator animator;
    public string levelIdentifier = "Level_";
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Play()
    {
        SceneController.LoadLevel(PlayerPrefs.GetInt(levelIdentifier, 1));
    }

    public void SetAnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}
