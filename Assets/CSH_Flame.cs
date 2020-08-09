using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_Flame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy em = collision.gameObject.GetComponent<Enemy>();
            if (em)
            {
                em.OnDamageProcess();
                Debug.Log("Enemy On Fire");
            }
        }
    }
}
