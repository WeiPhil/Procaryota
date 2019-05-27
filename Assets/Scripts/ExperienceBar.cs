using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;

    void Awake()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void ChangeExperienceValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }

}
