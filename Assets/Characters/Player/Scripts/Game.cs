using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int state = 0; // 0 pause, 1 start, 2 gameover
    public static bool[] items = new bool[] { false, false, false };

    public static int lives = 3;

    // Update is called once per frame
    void Update()
    {
        if (AllItems()) Debug.Log("Has encontrado todos los objectos");
        if (state == 2) Debug.Log("GameOver xd");
    }

    public static void UseLive()
    {
        lives -= 1;
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
