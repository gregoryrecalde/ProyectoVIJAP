using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int state = 0; // 0 pause, 1 start, 2 win
    public static int food = 0;
    public static int water = 0;
    public static int pets = 0;

    public Animator canvasAnimator;
    public static Pet currentPet;

    public static AudioSource audioSource;
    public AudioClip[] itemsSfx;
    int currentLevel;
    string levelIdentifier = "Level_";
    public static int goalLevel = 0;

    GameObject level;
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt(levelIdentifier, 1);
        currentLevel = 1;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (pets >= goalLevel && state != 2)
        {
            state = 2;
            Debug.Log("Level Complete " + currentLevel);
            currentLevel++;
            PlayerPrefs.SetInt("Level_", currentLevel);
            canvasAnimator.SetTrigger("Win");
        }
    }

    public void NextLevel()
    {
        SceneController.LoadLevel(currentLevel);
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
