using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int state = 0; // 0 pause, 1 start, 2 gameover
    public static int food = 4;
    public static int water = 5;
    public static int pets = 0;

    public static Pet currentPet;

    public static AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 2) Debug.Log("GameOver xd");
    }
    public static void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    public static void GameOver()
    {
        state = 2;
        Debug.Log("GameOver");
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
                    }
                    break;
                case "water":
                    if (!currentPet.WaterFull() && water > 0)
                    {
                        water--;
                        currentPet.waterRequired--;
                        currentPet.PlayEffect(itemName);
                    }
                    break;
                case "love":
                    if (!currentPet.LoveFull())
                    {
                        currentPet.loveRequired--;
                        currentPet.PlayEffect(itemName);
                    }
                    break;
                case "shower":
                    if (!currentPet.ShowerFull())
                    {
                        currentPet.showerRequired--;
                        currentPet.PlayEffect(itemName);
                    }
                    break;
                default: break;
            }
        }
    }
}
