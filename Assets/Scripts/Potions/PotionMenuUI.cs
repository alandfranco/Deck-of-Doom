using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Potion potion;

    [SerializeField] Button myButton;

    public Image typeButton;

    public PotionMenu potionMenu;

    void Start()
    {
        typeButton.sprite = potion.icon;
    }

    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)myButton).OnPointerEnter(eventData);
        potionMenu.descriptionText.text = potion.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)myButton).OnPointerExit(eventData);

        potionMenu.descriptionText.text = "";        
    }

    public void SelectPotion()
    {
        potionMenu.UpdateCurrentPotion(potion);
    }
}