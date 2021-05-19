using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public float life = 100;
    public int points = 500;
    public float GetLife()
    {
        return life;
    }

    public void SetLife(float amount)
    {
        life += amount;
    }
}
