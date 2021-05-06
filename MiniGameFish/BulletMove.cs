using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletMove : MonoBehaviour
{
    public string poolBulletName = "Bullet";
    public string poolBulletName02 = "Bullet02";
    public string poolBulletName03 = "Bullet03";
    public float moveSpeed = 3f;
    public float lifeTime = 10f;
    public float _elapsedTime = 0;

    SpriteRenderer Image;

    public GameObject target;
    public GameObject Mine;
    public bool isFlip;
    Rigidbody2D rig2D;

    void Awake()
    {
        target = GameObject.Find("Fish");
        Mine = GameObject.Find("Player");

        rig2D = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
       
    }
    private void Start()
    {

    }

    void Update()
    {
        // -4 == 불렛을 가지고 있는 객체의 scale
        if (Mine.transform.localScale.x == -4)
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;
        }
        else if (Mine.transform.localScale.x == 4)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }

        if (GetTimer() > lifeTime)
        {
            SetTimer();
            ObjectPool.Instance.PushToPool(poolBulletName, gameObject);
            ObjectPool.Instance.PushToPool(poolBulletName02, gameObject);
            ObjectPool.Instance.PushToPool(poolBulletName03, gameObject);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    void Flip()
    { 
        if (target.transform.position.x > this.transform.position.x)
        {
            isFlip = true;
        }
        else
        {
            isFlip = false;
        }
    }
}
