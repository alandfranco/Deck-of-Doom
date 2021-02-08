using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;

    public float speed;

    void Start()
    {
        
    }
        
    void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.TryGetComponent<PlayerMovement>(out var pl))
        {
            pl.GetComponent<TakeDamage>().TakeDamageToHealth(damage, this.gameObject);
        }
        if(!c.GetComponent<Enemy>())
        {
            this.gameObject.SetActive(false);
        }
    }
}
