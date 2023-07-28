using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public GameObject VirtualCam;
    GameObject[] objectInRoom;
    private void Start()
    {
        // tắt hoạt động của các object có trong room đi, bao giờ người chơi đi tới thì mới bật lên cho nó mượt
        // trong room có cả virtual camera của cinemachine nên khi bật nên sẽ có hiệu ứng chuyển cảnh
        Transform[] TransformObjectInRoom = gameObject.GetComponentsInChildren<Transform>();
        objectInRoom = TransformObjectInRoom.Where(transform => transform != this.transform)
                                            .Select(tranform => tranform.gameObject).ToArray();
        SetActiveGameObjects(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
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
        SetActiveGameObjects(set);
    }
    void SetActiveGameObjects(bool set)
    {
        foreach (GameObject obj in objectInRoom)
        {
            // phải kiểm tra null bởi vì trong room có chứa cherry(bị destroy khi thu thập)
            if(obj)
                obj.SetActive(set);
        }
    }
}
