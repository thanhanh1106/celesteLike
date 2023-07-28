using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cherry : MonoBehaviour
{
    public int Id { get; private set; }
    Vector3 velocity;
    bool collected;
    PlayerController player;
    Vector3 fistPosition;
    private void Awake()
    {
        Id = GetHashCode();
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        fistPosition = transform.position;
    }
    private void Update()
    {
        if (player.IsDie)
            collected = false;

        if(collected)
            gameObject.transform.position = 
                Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, 0.2f);
        else
            gameObject.transform.position =
                Vector3.SmoothDamp(transform.position, fistPosition, ref velocity, 0.3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConst.PLAYER_TAG))
        {
            collected = true;
        }
        if (collision.CompareTag(GameConst.CHECK_POINT_TAG))
        {
            GameManager.Instance.CherryIdManager.AddElementToListInJson(this.Id);
            Destroy(gameObject);
            GameManager.Instance.CherryCount();
        }
    }
    
}
