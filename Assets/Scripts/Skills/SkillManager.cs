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

    [SerializeField] Image[] skillsImg = new Image[4];
    
    void Start()
    {
        cam = Camera.main;
        viewVisual = true;
        skillWheelPanel.SetActive(false);
        currentSkill = skills[0];
        baseColor = skillsImg[0].color;
        anim = GetComponentInChildren<AnimatorHandler>();
    }

    public void Select(Image img)
    {
        img.color = hoverColor;
    }

    public void Deselect(Image img)
    {
        img.color = baseColor;
    }

    void Update()
    {
        AimingPoint();
        
        if(Input.GetKey(KeyCode.Tab))
        {
            ActivateWheel();
            //this.GetComponent<InputHandler>().enabled = false;
            //this.GetComponent<PlayerMovement>().enabled = false;
            FindObjectOfType<Cinemachine.CinemachineFreeLook>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            normalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

            currentAngle = (currentAngle + 360) % 360;

            selection = (int)currentAngle / 90;

            Select(skillsImg[selection]);
            if(selection != previousSelection)
            {
                Deselect(skillsImg[previousSelection]);
                currentSkill = skills[selection];
                previousSelection = selection;
            }
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            DeactivateWheel();
            //this.GetComponent<InputHandler>().enabled = true;
            FindObjectOfType<Cinemachine.CinemachineFreeLook>().enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (viewVisual)
            visualPoint.SetActive(true);
        else
            visualPoint.SetActive(false);
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
    }

    public void PerfomSkill()
    {
        currentSkill.transform.position = this.transform.position;
        currentSkill.TriggerAbility();
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
