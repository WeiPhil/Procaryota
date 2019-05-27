using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextToggle : MonoBehaviour
{
    private UnityEngine.UI.Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = new Color32(0, 0, 0, 255);
            cb.highlightedColor = new Color32(0, 0, 0, 255);
            switch (gameObject.name)
            {
                case "Difficulty Easy Text":
                    GameManager.Instance.GameDifficulty = GameManager.Difficulty.Easy;
                    break;
                case "Difficulty Normal Text":
                    GameManager.Instance.GameDifficulty = GameManager.Difficulty.Normal;
                    break;
                case "Difficulty Hard Text":
                    GameManager.Instance.GameDifficulty = GameManager.Difficulty.Hard;
                    break;
            }
        }
        else
        {
            cb.normalColor = new Color32(0, 0, 0, 120);
            cb.highlightedColor = new Color32(0, 0, 0, 255);
        }
        toggle.colors = cb;

    }
}
