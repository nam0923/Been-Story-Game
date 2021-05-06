using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulltetMove02 : MonoBehaviour
{
    public string poolBulletName6 = "FishBullet03";
    public float moveSpeed = 3f;
    public float lifeTime = 10f;
    public float _elapsedTime = 0;

    SpriteRenderer Image;

    void Start()
    {
    }

    void Update()
    {
        transform.position -= transform.right * moveSpeed * Time.deltaTime;

        if (GetTimer() > lifeTime)
        {
            SetTimer();
            ObjectPool.Instance.PushToPool(poolBulletName6, gameObject);
        }
    }

    float GetTimer()
    {
        return (_elapsedTime += Time.deltaTime);
    }

    void SetTimer()
    {
        _elapsedTime = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}

