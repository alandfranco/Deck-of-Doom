using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroFear : Skills
{
    [Header("Connections")]
    [SerializeField] private Animator animator = default;
    [Header("Visuals")]
    [SerializeField] private Renderer skinnedMesh = default;
    [SerializeField] private ParticleSystem particle = default;
    [Header("Variables")]
    public float duration;
    public float range;

    private void Initialize()
    {
        Activate();
    }

    public override void Skill()
    {
        Initialize();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, range);
        foreach (var item in hitColliders)
        {
            if (item.TryGetComponent<Enemy>(out var enemies))
            {
                enemies.GetScared(duration);
            }
        }
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
        yield break;
    }
}
