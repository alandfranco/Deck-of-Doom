using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsContainer : MonoBehaviour
{
    public Card weaponSlot;
    public Card armorSlot;
    public Card specialSlot;
    public Card variousSlot;
    
    void Start()
    {
        if(CardsManager.instance.weaponSlot)
            weaponSlot = CardsManager.instance.weaponSlot;
        if(CardsManager.instance.armorSlot)
            armorSlot = CardsManager.instance.armorSlot;
        if(CardsManager.instance.specialSlot)
            specialSlot = CardsManager.instance.specialSlot;
        if(CardsManager.instance.variousSlot)
            variousSlot = CardsManager.instance.variousSlot;
    }
}
