using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsContainer : MonoBehaviour
{
    public Card weaponSlot;
    public Card armorSlot;
    public Card specialSlot;

    GameObject weaponCard;
    GameObject armorCard;
    GameObject specialCard;

    void Start()
    {
        if(FindObjectOfType<CardsManager>())
        {
            if (CardsManager.instance.weaponSlot != null)
            {
                weaponCard = GameObject.Instantiate(CardsManager.instance.weaponSlot.myCard);
                weaponCard.name = "WeaponSlot";
                weaponSlot = weaponCard.GetComponent<Card>();
            }

            if (CardsManager.instance.armorSlot != null)
            {
                armorCard = GameObject.Instantiate(CardsManager.instance.armorSlot.myCard);
                armorCard.name = "ArmorSlot";
                armorSlot = armorCard.GetComponent<Card>();
            }

            if (CardsManager.instance.specialSlot != null)
            {
                specialCard = GameObject.Instantiate(CardsManager.instance.specialSlot.myCard);
                specialCard.name = "SpecialSlot";
                specialSlot = specialCard.GetComponent<Card>();
            }
        }
    }
}
