using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    Animator animator;

    public Text rankingTxt;
    public string levelIdentifier = "Level_";

    public TMP_InputField playerName;
    void Start()
    {
        animator = GetComponent<Animator>();
        LoadRanking();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PlayerPrefs.SetString("ranking", "");
            rankingTxt.text = "";
        }
    }
    public void Play()
    {
        if (!string.IsNullOrEmpty(playerName.text) && !string.IsNullOrWhiteSpace(playerName.text))
        {
            PlayerPrefs.SetString("player", playerName.text);
            SceneController.LoadLevel(2);
        }
    }

    void LoadRanking()
    {
        string ranking = PlayerPrefs.GetString("ranking", "");
        string[] scores = ranking.Split('|');
        Debug.Log("SCORES" + scores.Length);
        ranking = "";
        foreach (string score in scores)
        {
            if (!string.IsNullOrEmpty(score))
            {
                ranking = ranking + score + "\n";
            }
        }
        rankingTxt.text = ranking;
    }


    public void SetAnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}
