using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //패들값 가져오기
    public Paddle paddle;
    float mag; //현재 리지드 바디의 벨로시티의 크기
    GameObject Col;
    public Rigidbody2D Rg;

    //마그넷아이템사용  //공 각각의 bool값을 보고 paddle에서 인식하도록
    public bool isMagnet;

    private void Update()
    {
        mag = Rg.velocity.magnitude;
        //공의 속도가 느려지는거 대비해서 벨로시티를 일정한 속력으로 저장되어있는 ballAddForce()함수 불러오기
        if(paddle.ballSpeed == 200)
        {
            if(mag < 4.7f || mag > 5.1f)
            {
                paddle.BallAddForce(Rg);
            }
        }
        else
        {
            if(mag < 5.7f || mag > 6f)
            {
                paddle.BallAddForce(Rg);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        //공이 충돌할때 그정보를 패들에게 넘겨주자.
        Col = coll.gameObject;
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(paddle.BallCollisionEnter2D(transform, GetComponent<Rigidbody2D>(), GetComponent<Ball>(), Col, Col.transform, Col.GetComponent<SpriteRenderer>(), Col.GetComponent<Animator>()));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Col = col.gameObject;
        if(Col.tag == "TriggerBlock")
        {
            paddle.BlockBreak(Col, Col.transform, Col.GetComponent<Animator>());
        }
    }
}
