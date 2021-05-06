using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed;
    public SpeakManager speakManager;   //플레이어에서 매니저 함수를 호출할 수 있도록 변수 생성
    Rigidbody2D rigid;
    float h;
    float v;
    bool isHorizonMove;
    public bool getCorn;
    Vector3 dirVec;      //현재 바라보고 있는 방향값을 가진 변수
    Animator anim;
    GameObject scanObject; //스캔이 된 오브젝트
    public GameObject FinPannel;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Move 값
        h = speakManager.isAction ? 0 : Input.GetAxisRaw("Horizontal"); //상태변수를 사용하여 플레이어의 이동 제한 (삼항연산자 사용)
        v = speakManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        //Button Down & Up 체크
        //수평, 수직 이동 버튼이벤트를 변수로 저장
        bool hDown = speakManager.isAction ? false : Input.GetButtonDown("Horizontal"); //상태변수를 사용하여 플레이어의 이동 제한 (삼항연산자 사용)
        bool vDown = speakManager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = speakManager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = speakManager.isAction ? false : Input.GetButtonUp("Vertical");

        //Horizontal Move 체크
        if (hDown)
        {
            //버튼 다운으로 수평이동 체크
            isHorizonMove = true;
        }
        else if (vDown)
        {
            isHorizonMove = false;
        }
        else if (hUp || vUp)
            isHorizonMove = h != 0;

        //Animation
        if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else if (anim.GetInteger("vAxisRaw") != v)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else
            anim.SetBool("isChange", false);

        //Direction 상하좌우순으로
        if(vDown && v == 1) //버티컬누른 상태인데 그 값이 1일때 -> 위를 누른 상황
        {
            dirVec = Vector3.up;
        }
        else if(vDown && v == -1) //버티컬누른 상태인데 그 값이 -1일때 -> 아래를 누른 상황
        {
            dirVec = Vector3.down;
        }
        else if(hDown && h == -1) //호라이젠탈 누른 상태인데 그 값이 -1일때 -> 왼쪽을 누른 상황
        {
            dirVec = Vector3.left;
        }
        else if(hDown && h == 1) //호라이젠탈 누른 상태인데 그 값이 1일때 -> 오른쪽을 누른 상황
        {
            dirVec = Vector3.right;
        }

        //Scan Object => 내앞에 뭐가 있다!!! ex) 내앞에 달걀이 있다.
        if (Input.GetButtonDown("Jump") && scanObject != null) //스페이스바
        {
            //대화창에 텍스트 추가
            speakManager.Action(scanObject);
        }
    }

    private void FixedUpdate()
    {
        //Move
        //플래그 변수 하나로 수평, 수직이동을 결정
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;

        //Ray
        Debug.DrawRay(rigid.position, dirVec *0.7f, new Color(1,0,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            //RayCast된 오브젝트를 변수로 저장하여 활용
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "DragonGift")
        {
            StartCoroutine(FinishDragon());
        }
    }

    IEnumerator FinishDragon()
    {
        yield return new WaitForSeconds(4.5f);
        FinPannel.SetActive(true);
    }
}
