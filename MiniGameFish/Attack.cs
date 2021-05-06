using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Been been;
    void Start()
    {
    }

    void Update()
    {
        transform.position = been.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            been.attackDmg = 5;
            gameObject.SetActive(false);
        }
    }
}
