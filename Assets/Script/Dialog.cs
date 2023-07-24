using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text TitleText;
    public Text ContentText;

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public void UpdateDialog(string title,string content)
    {
        if(TitleText) TitleText.text = title;
        if(ContentText) ContentText.text = content;
    }
}
