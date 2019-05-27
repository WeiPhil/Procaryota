using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;

    void Awake()
    {
        originalSize = mask.rectTransform.rect.height;
    }

    public void ChangeWaitValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }

}
