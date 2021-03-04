using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public float timeToDisable;

    private void Awake()
    {
        StopAllCoroutines();
        StartCoroutine(DisableOnTime());
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DisableOnTime());
    }

    IEnumerator DisableOnTime()
    {
        yield return new WaitForSeconds(timeToDisable);

        this.gameObject.SetActive(false);
        yield break;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
