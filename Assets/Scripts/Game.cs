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
    public AudioClip[] itemsSfx;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    public static void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    public static void GameOver()
    {
        state = 2;
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
}
