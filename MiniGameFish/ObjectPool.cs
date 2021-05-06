using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : Singleton<ObjectPool>
{
    public List<PooledObject> objectPool = new List<PooledObject>();
    void Awake()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            objectPool[i].Initialize(transform);
        }
    }
                            //bulletName :반환할 객체의 pool오브젝트 이름. bullet :반환할 객체. parent:부모계층관계를 설정할 정보
    public bool PushToPool(string bulletName, GameObject bullet, Transform parent = null)   //사용한 객체를 오브젝트 풀에 반환할때 사용할 함수
    {
        PooledObject pool = GetPoolBullet(bulletName);
        if (pool == null)
        {
            return false;
        }
        pool.PushToPool(bullet, parent == null ? transform : parent);
        return true;
    }
                                  //bulletName: 요청할 객체의 pool 오브젝트 이름. parent: 부모계층 관계를 설정할 정보
    public GameObject PopFromPool(string bulletName, Transform parent = null)   //필요한 객체를 오브젝트 풀에 요청할때 사용할 함수
    {
        PooledObject pool = GetPoolBullet(bulletName);
        if(pool == null)
        {
            return null;
        }
        return pool.PopFromPool(parent);
    }

    PooledObject GetPoolBullet(string bulletName)   //전달된 불렛네임 파라미터와 같은 이름을 가진 오브젝트 풀을 검색하고, 검색에 성공하면 그 결과를 리턴
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (objectPool[i].poolBulletName.Equals(bulletName))
            {
                return objectPool[i];
            }
        }
        return null;
    }
}
