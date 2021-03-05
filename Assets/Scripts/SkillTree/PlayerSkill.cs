using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public new string name;
    [TextArea(3, 10)] public string description;
    public Sprite icon;

    public bool isAvailable;
    public bool isOwned;
    public int levelToUnlock;
    public List<PlayerSkill> skillsToUnlockThis = new List<PlayerSkill>();

    public Text nameText;
    public Text descriptionText;

    Button myButton;

    public int multiplier;

    public enum TypeBonus
    {
        health, damage, stamina, skill, cooldown
    }

    public List<TypeBonus> typeBonus = new List<TypeBonus>();

    public void Start()
    {
        myButton = this.GetComponent<Button>();
        if(icon != null)
            this.GetComponent<Image>().sprite = icon;
        if (PlayerProfile.instance.level >= levelToUnlock)
        {
            isAvailable = true;
            myButton.interactable = true;
        }
        else
            myButton.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)myButton).OnPointerEnter(eventData);
        nameText.text = name;
        string skillsToString = "";
        if (skillsToUnlockThis.Count > 0)
        {
            foreach (var item in skillsToUnlockThis)
            {
                skillsToString += " ," + item.name;
            }
            descriptionText.text = description + "\n" + "Level To Unlock " + levelToUnlock + "\n" + "Skills Require To Unlock" + skillsToString;
        }
        else
            descriptionText.text = description + "\n" + "Level To Unlock " + levelToUnlock;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)myButton).OnPointerExit(eventData);
        nameText.text = "";
        descriptionText.text = "";
    }
}
