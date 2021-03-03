using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button creditsButton;
    public Button quitButton;
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject creditsPanel;
    public GameObject optionsPanel;

    public GameObject LoadingScreenCanvas;

    public static MenuManager instance;

    public AudioMixer audioMixer;

    public GameObject sword;
    public GameObject necro;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainPanel.SetActive(true);
        playPanel.SetActive(false);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        LoadingScreenCanvas.SetActive(false);
        cardInventoryPanel.SetActive(false);
        necro.SetActive(false);
        sword.SetActive(true);

        ResolutionAvailables();
    }
        
    void Update()
    {
        if(playPanel.activeInHierarchy)
        {
            if (CardsManager.instance.ofAvailable && !ofCardSelected)
            {
                offensiveCard.image.sprite = noCardSelected;
                offensiveCard.interactable = true;
            }
            else if (!CardsManager.instance.ofAvailable)
            {
                offensiveCard.image.sprite = noCardAvailable;
                offensiveCard.interactable = false;
            }

            if (CardsManager.instance.defAvailable && !defCardSelected)
            {
                defensiveCard.image.sprite = noCardSelected;
                defensiveCard.interactable = true;
            }
            else if (!CardsManager.instance.defAvailable)
            {
                defensiveCard.image.sprite = noCardAvailable;
                defensiveCard.interactable = false;
            }

            if (CardsManager.instance.spAvailable && !spCardSelected)
            {
                specialCard.image.sprite = noCardSelected;
                specialCard.interactable = true;
            }
            else if (!CardsManager.instance.spAvailable)
            {
                specialCard.image.sprite = noCardAvailable;
                specialCard.interactable = false;
            }
        }
    }

    #region MenuButtonsAndFunctions
    public void PlayButton()
    {
        playPanel.SetActive(true);
        mainPanel.SetActive(false);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        characterPanel.SetActive(true);
        cardInventoryPanel.SetActive(false);
        necro.SetActive(true);
        sword.SetActive(false);
    }

    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void CreditsButtonExit()
    {
        creditsPanel.SetActive(false);
    }

    public void OptionsButton()
    {
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OptionsButtonExit()
    {
        optionsPanel.SetActive(false);
    }

    public void BackButtonPlay()
    {
        playPanel.SetActive(false);
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);

        necro.SetActive(false);
        sword.SetActive(true);

        foreach (var item in cardInventoryLayout.GetComponentsInChildren<RectTransform>())
        {
            if (item.gameObject != cardInventoryLayout.gameObject)
                Destroy(item.gameObject);

        }
    }

    public void LoadingScreen()
    {
        LoadingScreenCanvas.SetActive(true);

        foreach (var item in cardInventoryLayout.GetComponentsInChildren<RectTransform>())
        {
            if (item.gameObject != cardInventoryLayout.gameObject)
                Destroy(item.gameObject);

        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Cards
    [Header("Cards")]
    public Button offensiveCard;
    public bool ofCardSelected;
    public Button defensiveCard;
    public bool defCardSelected;
    public Button specialCard;
    public bool spCardSelected;
    public Sprite noCardAvailable;
    public Sprite noCardSelected;

    public Transform cardInventoryLayout;
    public GameObject cardInventoryPanel;

    public Text descriptionText;
    public Text nameText;
    
    public void ShuffleCards(int cardType)
    {
        cardInventoryPanel.SetActive(true);
        characterPanel.SetActive(false);

        foreach (var item in cardInventoryLayout.GetComponentsInChildren<RectTransform>())
        {
            if (item.gameObject != cardInventoryLayout.gameObject)
                Destroy(item.gameObject);
        }

        var cards = CardsManager.instance.ShowCards((CardSO.CardType)cardType);

        foreach (var item in cards)
        {
            var card = Instantiate(Resources.Load<GameObject>("Card"), cardInventoryLayout).GetComponent<CardMenuUI>();
            card.card = item;
        }
    }

    public void CardPicked(CardSO card)
    {
        if (card.cardType == CardSO.CardType.attack)
        {
            offensiveCard.image.sprite = card.image;
            ofCardSelected = true;
        }
        if (card.cardType == CardSO.CardType.defense)
        {
            defensiveCard.image.sprite = card.image;
            defCardSelected = true;
        }
        if (card.cardType == CardSO.CardType.special)
        {
            specialCard.image.sprite = card.image;
            spCardSelected = true;
        }

        cardInventoryPanel.SetActive(false);
        characterPanel.SetActive(true);
    }
    #endregion

    #region CharacterSelector
    public GameObject characterPanel;
    #endregion

    #region Options

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void ScreenMode(int index)
    {
        Screen.fullScreenMode = (FullScreenMode)index;
    }

    void ResolutionAvailables()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + " hz";
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
    #endregion
}
