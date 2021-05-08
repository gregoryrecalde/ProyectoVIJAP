using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int state = 0; // 0 pause, 1 start, 2 gameover
    public static bool[] items = new bool[] { false, false, false };
    public static int foodA = 0;
    public static int foodB = 0;
    public static int foodC = 0;

    public static AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (AllItems()) Debug.Log("Has encontrado todos los objectos");
        if (state == 2) Debug.Log("GameOver xd");
    }

    public static void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public static void IncreaseFood(char foodType)
    {
        if (foodType == 'A') foodA++;
        if (foodType == 'B') foodB++;
        if (foodType == 'C') foodC++;
        Debug.Log("Foods: A = " + foodA + " | B = " + foodB + " C = " + foodC);
    }
    public static void GameOver()
    {
        state = 2;
        Debug.Log("GameOver");
    }
    bool AllItems()
    {
        if (items[0] && items[1] && items[2]) return true;
        else return false;
    }
}
