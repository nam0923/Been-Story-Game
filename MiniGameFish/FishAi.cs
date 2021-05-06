using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAi : MonoBehaviour
{
    public string bulletName4   = "FishBullet";
    public string bulletName5 = "FishBullet02";
    public string bulletName6 = "FishBullet03";
   
    public Transform target;
    float attackDelay;
  
    public Transform FishBulletPos1;
    public Transform FishBulletPos2;
    public Transform FishBulletPos3;

    Fish fish;
   
    Animator anim;

    bool isDefense;
    int BulletTIme;
   
    void Start()
    {
        fish = GetComponent<Fish>();
        anim = GetComponent<Animator>();
        BulletTIme = 0;
    }

    void Update()
    {
        if (!fish.isFinish)
        {
            isDefense = fish.isDefense;
            attackDelay -= Time.deltaTime;
            if (attackDelay < 0)
            {
                attackDelay = 0;
            }

            float distance = Vector3.Distance(transform.position, target.position);

            if (attackDelay == 0 && distance <= fish.fileldOfVision)
            {
                // 1에서 3
                if (distance <= 3 && distance >= 1)
                {

                    target.GetComponent<Been>().nowHp -= fish.attackKickDmg;
                    anim.SetBool("isKick", true);
                    if (target.GetComponent<Been>().nowHp <= 0)
                    {
                        anim.SetBool("isKick", false);
                    }
                    anim.SetTrigger("Attack");
                    attackDelay = fish.attackSpeed;
                }  
                else
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        AnimReset();

                        anim.SetBool("isDefense", false);
                        MoveToTarget();
                    }
                }
            }
            else if (attackDelay == 0 && distance > fish.fileldOfVision)
            {
                anim.SetBool("isHead", true);
                BulletTIme++;
                print(BulletTIme);
                Shoot();

                if (target.GetComponent<Been>().nowHp <= 0)
                {
                    anim.SetBool("isHead", false);
                }
                anim.SetTrigger("Attack");
                attackDelay = fish.attackSpeed + 3f;
            }
            else
            {
                anim.SetBool("isDefense", false);
                MoveToTarget();
            }

            if (isDefense)
            {
                anim.SetBool("isDefense", true);
                if (target.GetComponent<Been>().nowHp <= 0)
                {
                    anim.SetBool("isDefense", false);
                }
            }
            else
            {
                anim.SetBool("isDefense", false);
                MoveToTarget();
            }
        }
    }
  
    void MoveToTarget()
    {
        float dirX = target.position.x - transform.position.x;
        float dirY = target.position.y - transform.position.y;
        dirX = (dirX < 0) ? -1 : 1;
        dirY = (dirY < 0) ? -1 : 1;
        transform.Translate(new Vector2(dirX, dirY) * fish.speed * Time.deltaTime);
        anim.SetBool("isMove", true);
    }

    void AnimReset()
    {
        anim.SetBool("isKick", false);
        anim.SetBool("isHead", false);
        anim.SetBool("isPunch", false);
        anim.SetBool("isDefense", false);
    }
   
    void Shoot()
    { 
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName4);
        bullet.transform.position = FishBulletPos1.transform.position + transform.up;
        bullet.SetActive(true);

        GameObject bullet2 = ObjectPool.Instance.PopFromPool(bulletName5);
        bullet2.transform.position = FishBulletPos2.transform.position + transform.up;
        bullet2.SetActive(true);

        GameObject bullet3 = ObjectPool.Instance.PopFromPool(bulletName6);
        bullet3.transform.position = FishBulletPos3.transform.position + transform.up;
        bullet3.SetActive(true);
    }
    
    IEnumerator BulletReset()
    {
        yield return new WaitForSeconds(1f);
    }
}
