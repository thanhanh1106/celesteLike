using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public List<GameObject> DialogBoxes;
    int currentDialog;

    private bool playerInRange;

    private void Start()
    {
        currentDialog = 0;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && playerInRange)
            ShowDialog();
    }
    void ShowDialog()
    {
        // nếu bấm show mà trước đấy có 1 hộp thoại rồi thì tắt hộp thoại đó đi
        if(currentDialog - 1 >= 0 && currentDialog <= DialogBoxes.Count)
        {
            if (DialogBoxes[currentDialog - 1].activeSelf)
                DialogBoxes[currentDialog - 1].SetActive(false);
        }
        //show
        if (currentDialog <= DialogBoxes.Count - 1)
        {
            if (DialogBoxes[currentDialog])
                DialogBoxes[currentDialog].SetActive(true);
            currentDialog++;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConst.PLAYER_TAG))
        {
            if (DialogBoxes[currentDialog])
            {
                DialogBoxes[currentDialog].SetActive(true);
            }
            currentDialog++;
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConst.PLAYER_TAG))
        {
            playerInRange = false;
            if(DialogBoxes.Count > 0 && DialogBoxes[currentDialog - 1].activeSelf)
            {
                DialogBoxes[currentDialog - 1].SetActive(false);
            }
            currentDialog = 0;
        }
    }
}
