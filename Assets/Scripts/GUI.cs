using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{

    private static GUI _instance;
    public static GUI Instance { get { return _instance; } }

    public LifeBar lifeBar;
    public ExperienceBar experienceBar;
    public GameObject freezeBar;
    public Text scoreText;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);
        freezeBar.SetActive(false);
        GetComponent<Canvas>().enabled = false;
    }

    public void ResetGUI()
    {
        freezeBar.SetActive(false);
        lifeBar.SetMaxLives(PlayerManager.Instance.maxHealth);
        SetExperience(0);
    }


    public void SetScore(int value)
    {
        scoreText.text = value.ToString();
    }

    public void SetExperience(float fraction)
    {
        experienceBar.ChangeExperienceValue(fraction);
    }

    public void SetFreezeWait(float fraction)
    {
        freezeBar.GetComponent<FreezeBar>().ChangeWaitValue(fraction);
    }

    public void ChangeLife(int ammount)
    {
        if (ammount > 0)
            lifeBar.GainLife(ammount);
        else
            lifeBar.LooseLife(ammount);
    }

    public void ChangeMaxLife(int ammount)
    {
        if (ammount > 0)
            lifeBar.AddLife(ammount);
        else
            lifeBar.RemoveLife(ammount);
    }

}
