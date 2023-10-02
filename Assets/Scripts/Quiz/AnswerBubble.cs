using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class AnswerBubble : MonoBehaviour
{
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ShowCorretAnswer()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        _image.SetNativeSize();
    }
    public void ResetAnswer()
    {
        Debug.Log("Reset");
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b,0);
        _image.SetNativeSize();
    }
    
}
