using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    private static Animations _instance;
    public static Animations Instance { get { return _instance; } }

    public Animator animator;
    public Text levelTransitionText;
    public Text gameOverTextMotivate;
    public Text gameOverText;

    private string[] gameOverTexts = {"Be Bold","Success is a state of Mind","Leaders Lead Followers Follow","Be prepared to opportunities","Believe in yourself"
       /*"Give it a try...", "Go for it.", "Why not again ", "It s worth a shot...", "What are you waiting for ",
        "What do you have to lose ", "You might as well...","Just do it.","There you go.","Keep up the good work.",
        "Keep it up.","Good job.","I m so proud of you.","Hang in there.","Don t give up.","Keep pushing.","Keep fighting.","Stay strong.",
        "Never give up.","Never say  die .","Come on. You can do it.","I ll support you either way...",
        "Im behind you IOO percent.","It s totally up to you.","It s your call.","Follow your dreams...",
        "Reach for the stars...","Do the impossible.","Believe in yourself.","The sky is the limit."*/
    };


    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        gameOverTextMotivate.text = gameOverTexts[Random.Range(0, gameOverTexts.Length)];
        animator.SetTrigger("FadeInMenu");
    }

    void OnMenuFadeOutComplete()
    {
        gameOverTextMotivate.text = gameOverTexts[Random.Range(0, gameOverTexts.Length)];
        gameOverText.text = "Game Over";
        GameManager.Instance.LoadNextScene();
        animator.SetTrigger("FadeIn");
    }

    public void FadeOutToNextLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void GameOver()
    {
        gameOverText.text = "YOU WON";
        gameOverTextMotivate.text = "You finally killed the Big Methastaser";
        animator.SetTrigger("GameOver");
    }


    void OnFadeOutComplete()
    {
        GameManager.Instance.LoadNextScene();
        animator.SetTrigger("FadeIn");
    }

    void OnGameOverComplete()
    {
        GameManager.Instance.LoadMenu();
        animator.SetTrigger("FadeInMenu");
        gameOverTextMotivate.text = gameOverTexts[Random.Range(0, gameOverTexts.Length)];
    }

}
