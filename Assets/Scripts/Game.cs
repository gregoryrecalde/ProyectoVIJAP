using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int state = 0; // 0 pause, 1 gameplay, 2 win, 3 end game, 4 para pausa y otras cosas
    public static int food = 0;
    public static int water = 0;
    public static int pets = 0;

    public static int respawns = 0;

    public Animator canvasAnimator;
    public static Pet currentPet;

    public static AudioSource audioSource;
    public AudioClip[] itemsSfx;
    string levelIdentifier = "Level_";
    public static int goalLevel = 0;

    public static int score = 0;

    public static bool waitPlayerAnswer = false;

    public GameUIManager gameUIManager;
    public int currentLevel = 1;
    public AudioClip pausaClip;
    public AudioClip resumeClip;
    public GameObject pausaText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        state = 1;
        food = 0;
        water =0;
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.P) && state == 1)
        {
            if(Time.timeScale!=0)
            {
                Time.timeScale = 0;
                pausaText.SetActive(true);
                PlaySound(pausaClip);
            }
            else
            {
                Time.timeScale = 1;
                pausaText.SetActive(false);
                PlaySound(resumeClip);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.LoadScene("MainMenu");
        };

        Debug.Log("pets "+pets+" | goalLevel: "+goalLevel+"| state :"+state);
        if (pets >= goalLevel && state == 1)
        {
            Debug.Log("Level Complete " + currentLevel);
            currentLevel++;
            canvasAnimator.SetTrigger("Win");
            state = 2;
        }
        if (state == 3 || state == 2)
        {
            score += ((pets * 10) - (respawns * 5)) * 100;
            if (score < 0) score = 0;
            Debug.Log("SaveState");
            gameUIManager.UpdateScore();

            gameUIManager.UpdateGoalMessage();
            if(currentLevel == 5) SaveScore();
            if(state == 3) SaveScore();
            goalLevel = 0;
            pets = 0;
            state = 4;
        }
    }

    public void NextLevel()
    {
        if(currentLevel == 5) SceneController.LoadScene("MainMenu");
        else SceneController.LoadLevel(currentLevel);
    }
    public static void SaveScore()
    {
        string ranking = PlayerPrefs.GetString("ranking", "");
        string playerName = PlayerPrefs.GetString("player", "player");
        PlayerPrefs.SetString("ranking", ranking + "|" + playerName + " - " + Game.score + "|");

    }
    public static void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }


    public void Use(string itemName)
    {
        if (currentPet != null)
        {

            switch (itemName)
            {
                case "food":
                    if (!currentPet.FoodFull() && food > 0)
                    {
                        food--;
                        currentPet.foodRequired--;
                        currentPet.PlayEffect(itemName);
                        PlaySound(itemsSfx[0]);
                    }
                    break;
                case "water":
                    if (!currentPet.WaterFull() && water > 0)
                    {
                        water--;
                        currentPet.waterRequired--;
                        currentPet.PlayEffect(itemName);

                        PlaySound(itemsSfx[1]);
                    }
                    break;
                case "love":
                    if (!currentPet.LoveFull())
                    {
                        currentPet.loveRequired--;
                        currentPet.PlayEffect(itemName);

                        PlaySound(itemsSfx[2]);
                    }
                    break;
                case "shower":
                    if (!currentPet.ShowerFull())
                    {
                        currentPet.showerRequired--;
                        currentPet.PlayEffect(itemName);

                        PlaySound(itemsSfx[3]);
                    }
                    break;
                default: break;
            }
        }
    }

    public static void IncreaseItem(string itemName)
    {
        if (itemName == "food") food++;
        if (itemName == "water") water++;
    }
}
