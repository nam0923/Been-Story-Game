using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public string poolBulletName = string.Empty;     //객체 검색할때 사용할 이름
    public GameObject prefab = null;                 //오브젝트 풀에 저장할 프리팹
    public int poolCount = 0;                        //초기화할 때 생성할 객체의 수
    [SerializeField]
    private List<GameObject> poolList = new List<GameObject>(); //생성한 객체들을 저장할 리스트

    public void Initialize(Transform parent = null) //초기화 pool Count 만큼 리스트에 생성후 추가하는 역할
    {
        for (int i = 0; i < poolCount; i++)
        {
            poolList.Add(CreateBullet(parent));
        }
    }
    public void PushToPool(GameObject bullet, Transform parent = null)  //사용한 객체를 다시 오브젝트 풀에 반환할때 사용
    {
        bullet.transform.SetParent(parent);
        bullet.SetActive(false);
        poolList.Add(bullet);
    }
    public GameObject PopFromPool(Transform parent = null)   //객체가 필요할때 오브젝트 풀에 요청하는 용도로 사용할 함수 => 먼저 저장해둔 오브젝트가 남아있는지 확인, 없으면 새로 생성후 추가
    {
        if(poolList.Count ==0)
        {
            poolList.Add(CreateBullet(parent));
        }
        GameObject bullet = poolList[0];
        poolList.RemoveAt(0);
        return bullet;
    }
    private GameObject CreateBullet(Transform parent =null) //프리팹변수에 지정된 게임 오브젝트를 생성하는 역할
    {
        GameObject bullet = Object.Instantiate(prefab) as GameObject;
        bullet.name = poolBulletName;
        bullet.transform.SetParent(parent);
        bullet.SetActive(false);
        return bullet;
    }
}
