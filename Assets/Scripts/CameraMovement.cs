using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerCam, centerPoint, centerPointLook;
    public float maxHeight, minHeight, orbitSpeed, verticalSpeed;
    float distance;

    float _startY;
    Vector3 originalPos;

    Vector3 centerPointDistance;

    float _height;

    public Vector3 pointLookOriginalPos;
    public Vector3 pointLookShieldPos;
    public Vector3 centerPointOriginalPos;
    public Vector3 centerPointShieldPos;

    public float originalDistance;
    public float shieldDistance;

    public float speedShieldTransition;

    public bool isShieldCam;
    float lerp;

    public float shakeAmount;

    void Start()
    {
        _startY = this.transform.eulerAngles.y;
        originalPos = centerPoint.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        centerPointLook.localPosition = pointLookOriginalPos;
        centerPointDistance = centerPointOriginalPos;
        distance = originalDistance;
    }

    void Update()
    {
        centerPoint.position = this.gameObject.transform.position + centerPointDistance;

        centerPoint.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime, 0);

        _height += Input.GetAxis("Mouse Y") * Time.deltaTime * -verticalSpeed;
        _height = Mathf.Clamp(_height, minHeight, maxHeight);

        

        if (Input.GetKeyUp(KeyCode.T))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isShieldCam)
        {
            lerp += Time.deltaTime * speedShieldTransition;
            centerPointLook.localPosition = Vector3.Lerp(centerPointOriginalPos, centerPointShieldPos, lerp);
            //centerPointLook.localPosition = pointLookShieldPos;
            centerPointDistance = Vector3.Lerp(centerPointOriginalPos, centerPointShieldPos, lerp);
            //centerPointDistance = centerPointShieldPos;
            distance = Mathf.Lerp(originalDistance, shieldDistance, lerp);
            //distance = shieldDistance;
        }
        else
        {
            lerp -= Time.deltaTime * speedShieldTransition;
            centerPointLook.localPosition = Vector3.Lerp(centerPointOriginalPos, centerPointShieldPos, lerp);
            centerPointLook.localPosition = pointLookOriginalPos;
            centerPointDistance = Vector3.Lerp(centerPointOriginalPos, centerPointShieldPos, lerp);
            //centerPointDistance = centerPointOriginalPos;
            distance = Mathf.Lerp(originalDistance, shieldDistance, lerp);
            //distance = originalDistance;
        }
        lerp = Mathf.Clamp(lerp, 0, 1);
        /*if (this.GetComponent<Animator>().GetFloat("Life") == 0)
            this.enabled = false;*/
        DisolveCamera();
    }

    void LateUpdate()
    {

        /*Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(this.transform.position, -playerCam.transform.forward);
        var _distance = Vector3.Distance(this.transform.position + new Vector3(0, 1, -1), playerCam.transform.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, _distance, 11, QueryTriggerInteraction.Collide))            
        {
            Debug.Log("colisionando");
            var _hitDist = Vector3.Distance(this.transform.position, hit.transform.position);
            playerCam.position = centerPoint.position + centerPoint.forward * -1 * ( _distance - _hitDist) + Vector3.up * _height;
        }
        else
        {
            Debug.Log("NoCol");
        }*/
        playerCam.position = centerPoint.position + centerPoint.forward * -1 * distance + Vector3.up * _height;
        playerCam.eulerAngles = Vector3.Slerp(playerCam.eulerAngles, centerPoint.eulerAngles, orbitSpeed * Input.GetAxis("Mouse X"));
        playerCam.LookAt(centerPointLook);
    }

    void DisolveCamera()
    {
        Ray ray = new Ray(playerCam.position, (this.transform.position - (playerCam.position - new Vector3(0, 0, 10))).normalized);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.collider.GetComponent<PlayerManager>())
            {
                playerCam.GetComponent<Camera>().nearClipPlane = Vector3.Distance(this.transform.position, playerCam.position) - 2f;
            }
            else
            {
                playerCam.GetComponent<Camera>().nearClipPlane = 0.3f;
            }
        }
    }
}
