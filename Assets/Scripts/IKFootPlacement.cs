using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    Animator anim;

    //Add all layers and then Remove those we dont want the feet to Interact with
    //This is for the Raycast. We dont want it to check with every collider it meets along the way.
    public LayerMask layerMask;

    [Range(0,1f)]
    public float distanceToGround;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            //Weights
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));

            //Left Foot
            RaycastHit hit;

            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, -Vector3.up);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            //Right Foot

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, -Vector3.up);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}
