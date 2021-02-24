using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager instance;

    public Card weaponSlot;
    public Card armorSlot;
    public Card specialSlot;
    public Card variousSlot;

    private void Awake()
    {
        instance = this;        
        instance = this;        
    }

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }
}
