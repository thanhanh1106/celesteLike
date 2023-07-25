using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject VirtualCam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (VirtualCam.active == true) return;
            StartCoroutine(Wait(true));
            CameraShake.Instance = VirtualCam.GetComponent<CameraShake>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            StartCoroutine(Wait(false));
        }
    }
    
    IEnumerator Wait(bool set)
    {
        yield return new WaitForSeconds(0.2f);
        VirtualCam.SetActive(set);
    }
}
