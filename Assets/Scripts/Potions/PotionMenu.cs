using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMenu : MonoBehaviour
{
    public GameObject potionLayoutPanel;
    public Transform potionLayout;

    public Text descriptionText;

    public Potion currentPotion;
    public Image currentPotionIcon;

    public GameObject potionUIPrefab;

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }

    public void UpdateCurrentPotion(Potion _p)
    {
        currentPotion = _p;
        currentPotionIcon.sprite = _p.icon;
        PlayerPassives.instance.potion = _p;
        ClosePotionLayout();
    }

    public void OpenPotionLayout()
    {
        potionLayoutPanel.SetActive(true);
        DisplayPotions();
    }

    public void ClosePotionLayout()
    {
        RemovePotions();
        potionLayoutPanel.SetActive(false);
    }

    void DisplayPotions()
    {
        foreach (var item in Resources.LoadAll<GameObject>("Potion"))
        {
            var _potion = Instantiate(potionUIPrefab, potionLayout);
            _potion.GetComponent<PotionMenuUI>().potion = item.GetComponent<Potion>();
            _potion.GetComponent<PotionMenuUI>().potionMenu = this;
        }
    }

    void RemovePotions()
    {
        foreach(var item in potionLayout.GetComponentsInChildren<Transform>())
        {
            Destroy(item.gameObject);
        }
    }
}