using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public float life = 100;
    private float mana = 100;

    private float maxLife = 100;
    public float GetLife()
    {
        return life;
    }


    public void ResetLife()
    {
        life = maxLife;
    }
    public float GetMana()
    {
        return mana;
    }

    public void SetLife(float amount)
    {
        life += amount;
    }

    public void SetMana(float amount)
    {
        mana += amount;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
