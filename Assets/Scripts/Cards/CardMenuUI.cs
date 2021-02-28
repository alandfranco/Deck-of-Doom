using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardSO card;

    [SerializeField]Button myButton;

    public Image typeButton;

    void Start()
    {
        this.GetComponent<Image>().sprite = card.image;
    }

    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)myButton).OnPointerEnter(eventData);
        MenuManager.instance.descriptionText.text = card.description;
        MenuManager.instance.nameText.text = card.name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)myButton).OnPointerExit(eventData);

        MenuManager.instance.descriptionText.text = "";
        MenuManager.instance.nameText.text = "";
    }

    public void SelectCard()
    {
        CardsManager.instance.PickCard(card);
        MenuManager.instance.CardPicked(card);
    }
}
