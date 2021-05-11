using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI foodTxt;
    public TextMeshProUGUI waterTxt;
    public static Animator animator;
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


    // Update is called once per frame
    void Update()
    {
        foodTxt.text = "Food = " + Game.food;
        waterTxt.text = "Water = " + Game.water;
    }
}
