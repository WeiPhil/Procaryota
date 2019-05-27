using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvolutionUI : MonoBehaviour
{
    private static EvolutionUI _instance;
    public static EvolutionUI Instance { get { return _instance; } }

    public Button TimeFreezeButton;
    public Button LifePointsButton;
    public Button ProtectorsButton;
    public Button ShieldTimeButton;
    public GameObject TimeFreezeInfo;

    public ImageSwitcher EvolutionPointsLeftImage;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);
        GetComponent<Canvas>().enabled = false;
        TimeFreezeInfo.SetActive(false);
    }

    public void ResetUI()
    {
        foreach (Transform child in transform)
        {
            ImageSwitcher imageSwitcher = child.GetComponent<ImageSwitcher>();
            if (child.GetComponent<ImageSwitcher>())
            {
                imageSwitcher.SetFirstImage();
            }
        }
    }

    public void ContinueGame()
    {
        PlayerManager.Instance.EndEvolve();
        // For now reset
        PlayerManager.Instance.EvolutionPointsLeft = 0;
        GetComponent<Canvas>().enabled = false;
    }

    public void BuyTimeFreeze()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PlayerManager.Instance.UpgradeTimeFreeze();
        PlayerManager.Instance.EvolutionPointsLeft--;
        if (PlayerManager.Instance.EvolutionPointsLeft == 0)
            DisableAllBuyButtons();
    }

    public void BuyLifePoints()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PlayerManager.Instance.UpgradeLifePoints();
        PlayerManager.Instance.EvolutionPointsLeft--;
        if (PlayerManager.Instance.EvolutionPointsLeft == 0)
            DisableAllBuyButtons();
    }

    public void BuyProtectors()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PlayerManager.Instance.UpgradeProtectors();
        PlayerManager.Instance.EvolutionPointsLeft--;
        if (PlayerManager.Instance.EvolutionPointsLeft == 0)
            DisableAllBuyButtons();
    }

    public void BuyShieldTime()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PlayerManager.Instance.UpgradeShieldTime();
        PlayerManager.Instance.EvolutionPointsLeft--;
        if (PlayerManager.Instance.EvolutionPointsLeft == 0)
            DisableAllBuyButtons();
    }

    public void DisableAllBuyButtons()
    {
        DisableBuyTimeFreeze();
        DisableBuyLifePoints();
        DisableBuyProtectors();
        DisableBuyShieldTime();
    }

    public void Activate()
    {
        GetComponent<Canvas>().enabled = true;
        EvolutionPointsLeftImage.SetFirstImage();
        PlayerManager playerInstance = PlayerManager.Instance;
        if (playerInstance.TimeFreezeLevel < playerInstance.MaxUpgrades)
            TimeFreezeButton.interactable = true;
        if (playerInstance.LifePointsLevel < playerInstance.MaxUpgrades)
            LifePointsButton.interactable = true;
        if (playerInstance.ProtectorsLevel < playerInstance.MaxUpgrades)
            ProtectorsButton.interactable = true;
        if (playerInstance.ShieldTimeLevel < playerInstance.MaxUpgrades)
            ShieldTimeButton.interactable = true;
    }

    public void DisableBuyTimeFreeze() { TimeFreezeButton.interactable = false; }

    public void DisableBuyLifePoints() { LifePointsButton.interactable = false; }

    public void DisableBuyProtectors() { ProtectorsButton.interactable = false; }

    public void DisableBuyShieldTime() { ShieldTimeButton.interactable = false; }
}
