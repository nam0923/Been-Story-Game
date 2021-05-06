using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Paddle paddle;

    //start()는 비활성화 됐다가 활성화해도 초반 한번만 실행되는 반면에 OnEnable()함수는 활성화될때마다 호출
    private void OnEnable() //불랫이 활성화될때마다 호출되는 함수 OnEnable
    {
        transform.position = new Vector2(paddle.paddleX + (CompareTag("Odd") ? 0.355f : -0.355f), -2.534f);
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.05f);
        
        //비활성화 >> 블록에 안부딪히고 어디 멀리 날아가면 2초후에 자동 비활성화 시키게 해주자.
        Invoke("ActiveFalse", 2f);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.name)
        {
            case "Block":
            case "HardBlock0":
            case "HardBlock1":
            case "HardBlock2":
                GameObject Col = col.gameObject;
                paddle.BlockBreak(Col, Col.transform, Col.GetComponent<Animator>());
                break;
            case "Background":
                ActiveFalse();
                break;

        }
    }
    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
