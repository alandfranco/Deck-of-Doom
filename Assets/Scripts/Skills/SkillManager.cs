using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;

public class SkillManager : MonoBehaviour
{
    public List<Skills> skills = new List<Skills>();

    Camera cam;

    public Transform aimingSpot;

    public GameObject visualPoint;

    public bool viewVisual;

    public Skills currentSkill;

    public GameObject skillWheelPanel;

    public Color hoverColor;
    public Color baseColor;

    public Vector2 normalisedMousePosition;
    public float currentAngle;

    public int selection;
    int previousSelection;

    AnimatorHandler anim;

    CardsContainer cards;

    [SerializeField] Image[] skillsImg = new Image[4];

    [Header("UI")]
    public Image skillBar;
    public Image skillLogo;
    public Text skillName;
    public Text skillState;    
    
    void Start()
    {
        cam = Camera.main;
        viewVisual = true;
        skillWheelPanel.SetActive(false);
        currentSkill = skills[0];
        baseColor = skillsImg[0].color;
        anim = GetComponentInChildren<AnimatorHandler>();
        cards = FindObjectOfType<CardsContainer>();
        if(FindObjectOfType< PlayerPassives>() != null)
        {
            foreach (var item in skills)
            {
                item.cooldownTime = item.cooldownTime - (item.cooldownTime * PlayerPassives.instance.cooldownBonus);
            }
        }
    }

    public void Select(Image img)
    {
        img.color = hoverColor;
    }

    public void Deselect(int skill)
    {
        if (skills[skill].currentCooldown > 0)
            return;

        skillsImg[skill].color = baseColor;
    }

    void Update()
    {
        AimingPoint();
        
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0.4f;
        }
        if(Input.GetKey(KeyCode.Tab))
        {
            ActivateWheel();
            //this.GetComponent<InputHandler>().enabled = false;
            //this.GetComponent<PlayerMovement>().enabled = false;
            FindObjectOfType<CameraMovement>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            normalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

            currentAngle = (currentAngle + 360) % 360;

            selection = (int)currentAngle / 90;

            Select(skillsImg[selection]);
            if(selection != previousSelection)
            {
                Deselect(previousSelection);
                SelectSkill(selection);
                previousSelection = selection;
            }
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            DeactivateWheel();
            //this.GetComponent<InputHandler>().enabled = true;
            Time.timeScale = 1;
            FindObjectOfType<CameraMovement>().enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (currentSkill.currentCooldown > 0)
        {
            skillBar.fillAmount = 1 - (currentSkill.currentCooldown / currentSkill.cooldownTime);            
        }
        else
        {
            skillBar.fillAmount = 1;
            OnSelectSkill();
        }

        foreach (var item in skills)
        {
            if (item.currentCooldown > 0 || currentSkill == item)
            {
                skillsImg[skills.IndexOf(item)].color = hoverColor;
            }
            else
                skillsImg[skills.IndexOf(item)].color = baseColor;
        }

        #region Testing
        if (viewVisual)
            visualPoint.SetActive(true);
        else
            visualPoint.SetActive(false);
        #endregion
    }

    void OnSelectSkill()
    {
        skillBar.fillAmount = 1 - (currentSkill.currentCooldown / currentSkill.cooldownTime);
        skillLogo.sprite = currentSkill.icon;
        skillName.text = currentSkill.title;
        if(currentSkill.currentCooldown > 0)
            skillState.text = "On Cooldown";
        else
            skillState.text = "Ready To Use";
    }

    public void ActivateWheel()
    {
        skillWheelPanel.SetActive(true);
    }

    public void DeactivateWheel()
    {
        skillWheelPanel.SetActive(false);
    }

    public void SelectSkill(int index)
    {
        currentSkill = skills[index];
        OnSelectSkill();
    }

    public void PerfomSkill()
    {
        currentSkill.transform.position = this.transform.position;
        currentSkill.TriggerAbility();
        skillsImg[skills.IndexOf(currentSkill)].color = hoverColor;
        OnSelectSkill();
        if(cards.weaponSlot != null && cards.weaponSlot.activateToUse)
            cards.weaponSlot.CanUseCard();
    }
    /*
    public void PerformSkilOne()
    {
        skills[0].transform.position = this.transform.position;        
        skills[0].TriggerAbility();
    }

    public void PerfomSkillTwo()
    {
        skills[1].transform.position = this.transform.position;        
        skills[1].TriggerAbility();
    }

    public void PerfomSkillThree()
    {
        skills[2].transform.position = this.transform.position;        
        skills[2].TriggerAbility();
    }

    public void PerfomSkillFour()
    {
        skills[3].transform.position = this.transform.position;        
        skills[3].TriggerAbility();
    }
    */
    public void AimingPoint()
    {
        int layerMask2 = 1 << 10;
        int layerMask = 1 << 9;
        //int layerMask3 = 1 << 11;
        layerMask = ~((layerMask) | (layerMask2) /*| (layerMask3*/);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, layerMask))
        {
            aimingSpot.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            visualPoint.transform.position = aimingSpot.position;
        }
        
    }
}
