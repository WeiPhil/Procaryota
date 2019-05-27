using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageSwitcher : MonoBehaviour
{
    public Sprite[] sprites;
    public Image imageHolder;
    public SpriteRenderer spriteRenderer;

    private int currentImage;

    void Awake()
    {
        currentImage = 0;
    }

    public void NextImage()
    {
        if (currentImage + 1 <= sprites.Length)
        {
            if (imageHolder != null)
                imageHolder.sprite = sprites[currentImage + 1];
            if (spriteRenderer != null)
                spriteRenderer.sprite = sprites[currentImage + 1];
            currentImage++;
        }
    }

    public void SetFirstImage()
    {
        currentImage = 0;
        if (imageHolder != null)
            imageHolder.sprite = sprites[currentImage];
        if (spriteRenderer != null)
            spriteRenderer.sprite = sprites[currentImage];
    }

}
