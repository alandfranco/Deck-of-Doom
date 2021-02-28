using PixelDragonDevs.SavingLoading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManager : MonoBehaviour, ISaveable
{
    public static CardsManager instance;

    public CardSO weaponSlot;
    public CardSO armorSlot;
    public CardSO specialSlot;
        
    [SerializeField] List<CardSO> allCardsInGame = new List<CardSO>();
    [SerializeField] List<CardSO> ownedCards = new List<CardSO>();

    [SerializeField] string[] ownCardsID;

    public bool ofAvailable;
    public bool defAvailable;
    public bool spAvailable;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach (var item in allCardsInGame)
        {
            if (ownCardsID.Contains(item.Id))
            {
                ownedCards.Add(item);
                item.isOwned = true;
            }
        }

        foreach (var item in ownedCards)
        {
            if (item.cardType == CardSO.CardType.attack)
                ofAvailable = true;
            if (item.cardType == CardSO.CardType.defense)
                defAvailable = true;
            if (item.cardType == CardSO.CardType.special)
                spAvailable = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ownCardsID = new string[2];
            for (int i = 0; i < 2; i++)
            {
                ownCardsID[i] = allCardsInGame[i].Id;
            }
        }
    }

    public IEnumerable<CardSO> ShowCards(CardSO.CardType cardType)
    {
        var cardsToShow = ownedCards.Where(x => x.cardType == cardType);
        return cardsToShow;
    }

    public void AddCard(List<CardSO> onGameCards)
    {
        var newCard = onGameCards.OrderBy(x => UnityEngine.Random.value).First();

        
    }

    public void PickCard(CardSO card)
    {
        if (card.cardType == CardSO.CardType.attack)
            weaponSlot = card;

        if (card.cardType == CardSO.CardType.defense)
            armorSlot = card;

        if (card.cardType == CardSO.CardType.special)
            specialSlot = card;
    }

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            ownedCards = ownCardsID,
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        ownCardsID = saveData.ownedCards;
    }

    [Serializable]
    private struct SaveData
    {
        public string[] ownedCards;
    }
    #endregion
}
