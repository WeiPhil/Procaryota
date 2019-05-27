using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

enum Difficulty { Easy, Normal, Hard };

public class GameManager : MonoBehaviour
{
    public enum Difficulty { Easy, Normal, Hard };

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public PlayerManager playerManager;
    public GameObject backgroundImage;
    public Sprite[] backgroundSprites;

    private int currentLevel = 0;

    private int _score;

    private Difficulty _gameDifficulty;

    private bool _gamePaused;
    public GameObject pauseMenu;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            GUI.Instance.SetScore(_score);
        }
    }

    public Difficulty GameDifficulty { get { return _gameDifficulty; } set { _gameDifficulty = value; } }
    public bool GamePaused { get { return _gamePaused; } set { _gamePaused = value; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void UnpauseGame()
    {
        _gamePaused = false;
        pauseMenu.SetActive(false);
    }

    public void Start()
    {
        pauseMenu.SetActive(false);
        _gameDifficulty = Difficulty.Normal;
        _score = 0;
        Application.targetFrameRate = 60;
        LoadMenu();
        string[] startScenes = { "Menu", "Singletons" };
        UnloadAllScenesExcept(startScenes);
        _gamePaused = false;
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Singletons")
        {
            Animations.Instance.animator.SetTrigger("FadeInMenu");
            SoundManager.Instance.PlayMenuSound();
        }
        else
        {
            SoundManager.Instance.PlayGameSound();
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[0];
        }
        GUI.Instance.SetScore(_score);

    }

    private void UnloadAllScenesExcept(string[] sceneNames)
    {
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!sceneNames.Contains(scene.name))
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 9")) && PlayerManager.Instance != null && PlayerManager.Instance.InGame && !PlayerManager.Instance.Animating)
        {
            pauseMenu.SetActive(!_gamePaused);
            Text level = pauseMenu.transform.Find("Actual Level").GetComponent<Text>();
            level.text = LevelText(currentLevel);
            _gamePaused = !_gamePaused;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GoToLevel(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GoToLevel(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GoToLevel(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GoToLevel(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GoToLevel(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GoToLevel(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GoToLevel(10);
        }
    }

    public void StartGame()
    {
        Instantiate(playerManager, transform.position, Quaternion.identity);
        Animations.Instance.levelTransitionText.text = "Level I";
        Animations.Instance.animator.SetTrigger("FadeOutMenu");
        SoundManager.Instance.PlayGameSound();
        GUI.Instance.SetScore(_score);
    }

    public void LoadMenu()
    {
        currentLevel = 0;
        _score = 0;
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.Destroy();
        GUI.Instance.GetComponent<Canvas>().enabled = false;
        SceneManager.LoadScene("Menu");
        GameManager.Instance.GamePaused = true;
        SoundManager.Instance.PlayMenuSound();
    }

    public void LoadNextScene()
    {
        GUI.Instance.GetComponent<Canvas>().enabled = true;
        currentLevel++;
        if (currentLevel < 3)
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[0];
        }
        else if (currentLevel < 8)
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[1];
        }
        else
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[2];
        }
        SceneManager.LoadScene("Level" + currentLevel);
        PlayerManager.Instance.ResetPosition();
        PlayerManager.Instance.EnterLevelAndStart();
    }

    public string LevelText(int level)
    {
        switch (level)
        {
            case 1:
                return "Level I";
            case 2:
                return "Level II";
            case 3:
                return "Level III";
            case 4:
                return "Level IV";
            case 5:
                return "Level V";
            case 6:
                return "Level VI";
            case 7:
                return "Level VII";
            case 8:
                return "Level VIII";
            case 9:
                return "Level IX";
            case 10:
                return "Level X";
            default:
                return "Level X";
        }
    }

    public void GoToLevel(int level)
    {

        currentLevel = level;
        SoundManager.Instance.PlayGameSound();
        if (PlayerManager.Instance == null)
        {
            _score = 0;
            Instantiate(playerManager, transform.position, Quaternion.identity);
        }
        GUI.Instance.GetComponent<Canvas>().enabled = true;
        currentLevel = level;
        GameManager.Instance.GamePaused = true;

        Animations.Instance.levelTransitionText.text = LevelText(level);


        if (currentLevel < 3)
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[0];
        }
        else if (currentLevel < 8)
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[1];
        }
        else
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundSprites[2];
        }
        SceneManager.LoadScene("Level" + level);
        PlayerManager.Instance.ResetPosition();
        PlayerManager.Instance.EnterLevelAndStart();
    }

    public void GoToNextLevel()
    {
        GameManager.Instance.GamePaused = true;

        Animations.Instance.levelTransitionText.text = LevelText(currentLevel + 1);

        if (currentLevel + 1 == 11)
        {
            Animations.Instance.GameOver();
            return;
        }
        Animations.Instance.FadeOutToNextLevel();
        PlayerManager.Instance.AnimateTo(new Vector2(7.5f, 0.0f), 4.0f);
    }

}
