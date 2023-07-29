using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonusDash : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConst.PLAYER_TAG))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.BonusDash();
            Invoke("Reappears", 2);
            CameraShake.Instance.ShakeCamera(1f, 2f, 0.1f);
            gameObject.SetActive(false);
            
        }
    }
    void Reappears()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}
