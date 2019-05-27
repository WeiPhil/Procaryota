using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite menuBackground;
    public Sprite optionsScreen;

    public Sprite[] helpPannels;

    private bool soundMute;

    private int helpPanellIndex;


    void Awake()
    {
        soundMute = false;
        helpPanellIndex = 0;
        GetComponent<Canvas>().enabled = true;
    }

    public void HandlePlayClick()
    {
        GameManager.Instance.StartGame();
    }


    public void HandleSoundClick(Button soundButton)
    {
        SoundManager.Instance.ToggleSound(!soundMute);
        soundMute = !soundMute;
        soundButton.GetComponent<Image>().sprite = soundMute ? soundOff : soundOn;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void HandleOptionsClick(Canvas optionPannel)
    {
        optionPannel.enabled = true;
    }

    public void HandleChangePageClick(bool forward)
    {
        Canvas helpPannel = transform.Find("Help Pannel").gameObject.GetComponent<Canvas>();
        helpPanellIndex += forward ? 1 : -1;

        helpPannel.GetComponentInChildren<Image>().sprite = helpPannels[helpPanellIndex];
        if (helpPanellIndex + 1 < helpPannels.Length)
            helpPannel.transform.Find("Next Page").gameObject.GetComponent<Image>().enabled = true;
        else
            helpPannel.transform.Find("Next Page").gameObject.GetComponent<Image>().enabled = false;

        if (helpPanellIndex > 0)
            helpPannel.transform.Find("Previous Page").gameObject.GetComponent<Image>().enabled = true;
        else
            helpPannel.transform.Find("Previous Page").gameObject.GetComponent<Image>().enabled = false;
    }

    public void HandleHelpClick(Canvas helpPannel)
    {
        helpPannel.enabled = true;
        helpPannel.GetComponentInChildren<Image>().sprite = helpPannels[0];
        helpPannel.transform.Find("Previous Page").gameObject.GetComponent<Image>().enabled = false;
    }



    public void HandleExitClick(Canvas pannel)
    {
        pannel.enabled = false;
        helpPanellIndex = 0;
    }
}
