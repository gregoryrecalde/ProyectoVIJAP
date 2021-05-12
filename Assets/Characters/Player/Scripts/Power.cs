using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public float damage = 0.5f;

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {
            if (other.gameObject.transform.parent.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.transform.parent.gameObject.GetComponent<EnemyProperties>().SetLife(damage * Time.deltaTime);
            }
        }
    }

}
