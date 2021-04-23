using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpowner : MonoBehaviour
{
    public GameObject[] powers;
    public GameObject powerIndicator;
    void Update()
    {

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Floor")
            {
                Vector3 indicatorPosition = hitInfo.point;
                indicatorPosition.y += 0.01f;
                powerIndicator.transform.position = indicatorPosition;
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (powers.Length > 0)
                    {
                        GameObject power = Instantiate(powers[0]);
                        power.transform.position = indicatorPosition;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    if (powers.Length > 1)
                    {
                        GameObject power = Instantiate(powers[1]);
                        power.transform.position = indicatorPosition;
                    }
                }
            }
        }
    }
}
