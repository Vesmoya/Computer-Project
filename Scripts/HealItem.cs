using ARPG.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public float healthAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Health>().HealOnPickUp(healthAmount);
            Destroy(gameObject);
        }
    }
}
