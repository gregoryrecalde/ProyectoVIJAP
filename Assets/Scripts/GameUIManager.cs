using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI foodTxt;
    public TextMeshProUGUI waterTxt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI goalTxt;
    public static Animator animator;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    public static void ShowAnimalGUI()
    {
        animator.SetBool("ShowAnimalGUI", true);
    }
    public static void HideAnimalGUI()
    {
        animator.SetBool("ShowAnimalGUI", false);
    }


    public void PlaySoundtrack(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        foodTxt.text = "x" + Game.food;
        waterTxt.text = "x" + Game.water;
    }

    public void UpdateScore()
    {
        scoreTxt.text = "score: " + Game.score;
    }

    public void UpdateGoalMessage()
    {
        goalTxt.text = "RESCATASTE " + Game.pets + " GATITOS";
    }
}
