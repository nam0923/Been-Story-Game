using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    [Multiline(12)]
    public string[] StageStr;   //stage string 에 따라 *이 빈칸 0이 빨간색 블럭, 1이 주황색 블럭 이런 식으로 블록이 만들어지도록
    public Sprite[] B_MiniGame; //미니게임 블록 리소스 자른거 저장
    public GameObject P_Item;   //프리팹 아이템
    public SpriteRenderer P_ItemSr;
    public Text StageText;
    public Text ScoreText;
    public GameObject Life0;
    public GameObject Life1;
    public GameObject WinPanel;
    public GameObject GameOverPanel;
    public GameObject PausePanel;
    public GameObject StartPanel;
    public Transform ItemsTr;
    public Transform BlocksTr;
    public BoxCollider2D[] BlockCol;
    public GameObject[] Ball;
    public Animator[] BallAni;
    public Transform[] BallTr;
    public SpriteRenderer[] BallSr;
    public Rigidbody2D[] BallRg;
    public GameObject[] Bullet;
    public SpriteRenderer PaddleSr;
    public BoxCollider2D PaddleCol;
    public GameObject Magnet;
    public GameObject Gun;
    public AudioSource S_Break;
    public AudioSource S_Eat;
    public AudioSource S_Fail;
    public AudioSource S_Gun;
    public AudioSource S_HardBreak;
    public AudioSource S_Paddle;
    public AudioSource S_Victory;

    public GameObject Corn;
    bool isCorn;
    public GameObject FindCornPanel;

    bool isStart; 
    public float paddleX; //마우스 포지션이나 터치 포지션을 가져와서 패들의 x값 정하기
    public float ballSpeed; //공 속도
    float oldBallSpeed = 300; //고정값
    float paddleBorder = 4.406f; //화면 넘어가지 않게
    float paddleSize = 1.56f; //처음 패들 사이즈
    int combo; //콤보 스코어에 적용
    int score;
    int stage;

    public PlayerAction player;

    void Awake()
    {
    } 

    private void Start()
    {
        StartPanel.SetActive(true);
        //숨겨진 콘 위치
        FindCorn(new Vector2(Random.Range(-4.24f, 4.24f), Random.Range(1f, 5f)));
    }
    //뒤로가기 키를 누르면 일시정지가 뜨게 하기 위해 함수
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf)
            {
                PausePanel.SetActive(false);
                Time.timeScale = 1; 
            }
            else
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    //스테이지 초기화(-1 :재시작 0: 다음스테이지, 숫자 :숫자스테이지
    public void AllReset(int _stage)
    {
        if (_stage == 0)
        {
            stage++; //다음스테이지로 ㄱㄱ
        }
        else if (_stage != -1)
        {
            stage = _stage; //재시작이 아닐때는 숫자 스테이지와 스테이지를 같게 만들어주자
        }
        if (stage >= StageStr.Length)
        {
            return; //만들어둔 사이즈 (단계) 보다 크거나 같을 때는 리턴
        }

        Clear();
        BlockGenerator();
        StartCoroutine("BallReset");

        //초기화
        StageText.text = stage.ToString();
        score = 0;
        ScoreText.text = "0";
        PaddleSr.enabled = true;
        Life0.SetActive(true);
        Life1.SetActive(true);
        GameOverPanel.SetActive(false);
        StartPanel.SetActive(false);
        WinPanel.SetActive(false);
        FindCornPanel.SetActive(false);
        isCorn = false;
        Corn.name = "Corn";
    }

    //블록생성
    void BlockGenerator()
    {
        //현재 스테이지 스트링 = 스테이지 스트링[스테이지]의 엔터를 빈칸으로 대체 -> 유니티 스테이지 첫줄뒤 둘째줄로 들어갈때 엔터친거를 빈칸으로 대체한다는 거.
        string currentStr = StageStr[stage].Replace("\n", "");
        currentStr = currentStr.Replace(" ", ""); // 혹시 모르니까 현재 스테이지 스트링에서 띄어쓰기가 있을떄도 빈칸으로 대체.

        // for쓰고 탭두번누르면 for문 자동완성됨!!
        for (int i = 0; i < currentStr.Length; i++)
        {
            BlockCol[i].gameObject.SetActive(false);

            char A = currentStr[i];
            string currentName = "Block";
            int currentB = 0; //현재 블록

            //stage에 저장된 블록의 모양이 * 빈칸, 0 빨강, 1 노랑, 2 초록, 3 보라, 4 파랑, 5 다홍, 6 주황, 7 분홍, 8 돌, 9 랜덤
            if (A == '*') continue;
            else if (A == '8')
            {
                currentB = 8;
                currentName = "HardBlock0";
            }
            else if (A == '9')
            {
                //0~7까지의 숫자 랜덤
                currentB = Random.Range(0, 8);
            }
            else
            {
                currentB = int.Parse(A.ToString());
            }

            BlockCol[i].gameObject.name = currentName;
            BlockCol[i].gameObject.GetComponent<SpriteRenderer>().sprite = B_MiniGame[currentB];
            BlockCol[i].gameObject.SetActive(true);
        }
    }

    //볼위치 초기화하고 1초간 깜빡이는 애니메이션 재생
    IEnumerator BallReset()
    {
        isStart = false;
        combo = 0;
        Ball[0].SetActive(true);
        Ball[1].SetActive(false);
        Ball[2].SetActive(false);
        
        BallAni[0].SetTrigger("Blink");

        //BallTr[0].position = new Vector2(transform.position.x, transform.position.y);
        BallTr[0].position = new Vector2(paddleX, -2.9f);

        StopCoroutine("InfinityLoop");
        yield return new WaitForSeconds(1f);
        StartCoroutine("InfinityLoop");
    }

    //무한루프
    IEnumerator InfinityLoop()
    {
        //무한반복하기 위해 while문
        while (true)
        {   //마우스 누를때 공이 붙어있지
            // 마우스 좌클릭                //모바일에서 손가락 1개의 상태가 움직이고 잇는 상태이다!! 
            if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {                                           //마우스왼쪽버튼이 맞으면 인풋 마우스 포지션, 아니면 모바일이니까 모바일 포지션 //모바일은 vector2이기 때문에 형변환 
                paddleX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position).x, -paddleBorder, paddleBorder);
                // Mathf.Clamp 사용(패들의 x값을 패들 보더만큼 제한 >> 마지막 에 패들이 더이상 못가게 막아줘야 될 수치 -와 + 사용)

                transform.position = new Vector2(paddleX, transform.position.y);
                //맨처음에 시작할때만 붙어있게!
                if (!isStart)
                {
                    BallTr[0].position = new Vector2(paddleX, BallTr[0].position.y);
                }
            }
            //마우스 떼지면 공 날라가
                                //마우스 띄었을때      || 모바일에서 손이 화면에서 떨어졌을때
            if (!isStart && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
            {
                isStart = true; //이값으로 위의 if가 계속 발송되지 못하게 만들자
                ballSpeed = oldBallSpeed;
                BallRg[0].AddForce(new Vector2(0.1f, 0.9f).normalized * ballSpeed); //그냥발사하면 한줄 다뚫어버리기때문이라는데
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    //볼이 충돌 할 때
    //Paddle.cs에서 모든 게임오브젝트들을 참조했기에 Ball.cs에서 하지 않고 여기서 코루틴을 만들어 사용(깔끔하기때문).
    public IEnumerator BallCollisionEnter2D(Transform ThisBallTr, Rigidbody2D ThisBallRg, Ball ThisBallCs, GameObject Col, Transform ColTr, SpriteRenderer ColSr, Animator ColAni)
    {
        //2,2번의 레이어는 서로 충돌해도 무시한다!!! //같은 볼끼리 충돌 무시
        Physics2D.IgnoreLayerCollision(2, 2);

        if (!isStart) yield break;

        //ball과 부딪히는 Collision의 이름들에 따라 다른 행동을 보여줄 수 있게 스위치문 사용.
        switch (Col.name)
        {
            //패들에 부딪히면 차이값만큼 힘 주기.
            case "Paddle":
                ThisBallRg.velocity = Vector2.zero; // 속력을 0으로 줘야 힘을 제대로 받음.
                ThisBallRg.AddForce((ThisBallTr.position - transform.position).normalized * ballSpeed);
                S_Paddle.Play(); //sound 실행
                combo = 0;
                break;
            //자석패들에 부딪히면 볼이 자석에 붙어있음
            case "MagnetPaddle":
                ThisBallCs.isMagnet = true;
                ThisBallRg.velocity = Vector2.zero;
                //공이 날라오다가 마그넷 패들과 부딪혔을때 부딪힌 순간 패들과 공의 갭차이를 갭x에 저장
                float gapX = transform.position.x - ThisBallTr.position.x;
                while(ThisBallCs.isMagnet)
                {
                    //마우스를 누르고 있는 상태
                    if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
                    {                                       //부딪힌 순간의 위치에서 같이 움직여야하기때문에 현재 패들의 좌표와 갭을 합쳐줌
                        ThisBallTr.position = new Vector2(transform.position.x +gapX, ThisBallTr.position.y);
                    }
                    //패들의 상태가 원래대로 돌아왔을 때 //마우스 띄었을 때 상태      || 모바일에서 손이 화면에서 떨어졌을때
                    if (gameObject.name == "Paddle" || (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
                    {
                        ThisBallRg.velocity = Vector2.zero;
                        ThisBallRg.AddForce((ThisBallTr.position - transform.position).normalized * ballSpeed);
                        ThisBallCs.isMagnet = false;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            //데스존에 부딪히면 비활성화, 볼체크
            case "DeathZone":
                ThisBallTr.gameObject.SetActive(false); //볼 게임오브젝트 비활성화
                //볼체크 함수 실행
                BallCheck();
                break;
            //돌에 부딪히면 돌1이 됨
            case "HardBlock0":
                Col.name = "HardBlock1";
                ColSr.sprite = B_MiniGame[9];
                S_HardBreak.Play(); //sound 실행
                break;
            //돌1에 부딪히면 돌2이 됨
            case "HardBlock1":
                Col.name = "HardBlock2";
                ColSr.sprite = B_MiniGame[10];
                S_HardBreak.Play(); //sound 실행
                break;
            //돌2에 부딪히거나 블록에 부딪히면 불록 부숴짐
            case "HardBlock2":
            case "Block":
                //블럭뿌셔지는 함수 실행
                BlockBreak(Col, ColTr, ColAni);
                break;
        }
    }

    // 패들이 아이템과 충돌할 때
    //아이템과 패들 트리거 충돌 후 아이템 사라지고 아이템 효과 
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
        S_Eat.Play();
        switch(col.name)
        {
            //볼 세개 전부 활성화
            case "Item_TripleBall":
                GameObject OneBall = BallCheck();
                for (int i = 0; i < 3; i++)
                {
                    if(OneBall.name == Ball[i].name)
                    {
                        continue;
                    }
                    else
                    {
                        BallTr[i].position = OneBall.transform.position;
                        Ball[i].SetActive(true);
                        BallRg[i].velocity = Vector2.zero;
                        BallRg[i].AddForce(Random.insideUnitCircle.normalized * ballSpeed); //원형으로 랜덤방향
                    }
                }
                break;
            //7.5초동안 패들 커지기
            case "Item_Big":
                paddleSize =2.5f;
                paddleBorder = 3.99f;
                //stopCoroutin() 먼저 해줘야 아이템을 먹은 상태에서 다시 먹은 시점에서 7.5초의 딜레이를 늘어나게
                StopCoroutine("Item_BigOrSmall");
                StartCoroutine("Item_BigOrSmall", false); //초기화할때 skip의 상태를 true로!
                break;
            //7.5초동안 패들 작아짐
            case "Item_Small":
                paddleSize = 1.0f;
                paddleBorder = 4.72f;
                StopCoroutine("Item_BigOrSmall");
                StartCoroutine("Item_BigOrSmall", false);
                break;
            //7.5초동안 볼의 속도 늦어짐
            case "Item_SlowBall":
                StopCoroutine("Item_SlowBall");
                StartCoroutine("Item_SlowBall", false);
                break;
            //4초동안 불공이됨
            case "Item_FireBall":
                StopCoroutine("Item_FireBall");
                StartCoroutine("Item_FireBall", false);
                break;
            //7.5초동안 자석 활성화
            case "Item_Magnet":
                StopCoroutine("Item_Magnet");
                StartCoroutine("Item_Magnet", false);
                break;
            //4초동안 24발의 총발사
            case "Item_Gun":
                StopCoroutine("Item_Gun");
                StartCoroutine("Item_Gun", false);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Col = collision.gameObject;
        if(collision.gameObject.tag == "Corn")
        {
            isCorn = true;
            //player.getCorn = true;

            Col.SetActive(false);
            print("isCorn");
            if (isCorn)
            {
                FindCornPanel.SetActive(true);
                Clear();
            }
            Destroy(Col, 0.02f);
        }
    }

    //총알 아이템
    IEnumerator Item_Gun(bool isSkip)                        
    {
        if (!isSkip)
        {
            Gun.SetActive(true);
            for (int i = 0; i < 12; i++)
            {
                Bullet[i * 2].SetActive(true);      //tag == Untagged (짝수)
                Bullet[i * 2 + 1].SetActive(true);  //tag == odd (홀수)
                S_Gun.Play(); // sound 실행
                yield return new WaitForSeconds(0.34f); //0.34 *12 = 대략 4초
            }
        }
        Gun.SetActive(false);
    }

    //자석 아이템
    IEnumerator Item_Magnet(bool isSkip)                      
    {
        if (!isSkip)
        {
            gameObject.name = "MagnetPaddle";
            Magnet.SetActive(true);                             //
            yield return new WaitForSeconds(5.5f);              //
            Magnet.SetActive(false);                            //
            yield return new WaitForSeconds(0.5f);              //
            Magnet.SetActive(true);                             //
            yield return new WaitForSeconds(0.5f);              //
            Magnet.SetActive(false);                            //
            yield return new WaitForSeconds(0.25f);             // Magnet의 애니메이션
            Magnet.SetActive(true);                             //
            yield return new WaitForSeconds(0.25f);             //
            Magnet.SetActive(false);                            //
            yield return new WaitForSeconds(0.25f);             //
            Magnet.SetActive(true);                             //
            yield return new WaitForSeconds(0.25f);             // 
        }
        gameObject.name = "Paddle";
        Magnet.SetActive(false);
    }

    //불공 아이템
    IEnumerator Item_FireBall(bool isSkip)                      
    {
        if (!isSkip)
        {
            for (int i = 0; i < 3; i++)
            {
                BallSr[i].sprite = B_MiniGame[23];
                ParticleSystem.MainModule Ps = BallTr[i].GetChild(0).GetComponent<ParticleSystem>().main;
                Ps.startColor = Color.red;
            }
            for (int i = 0; i < BlockCol.Length; i++)
            {
                BlockCol[i].tag = "TriggerBlock";
                BlockCol[i].isTrigger = true;
            }
            yield return new WaitForSeconds(4f);
        }
        for (int i = 0; i < 3; i++)
        {
            BallSr[i].sprite = B_MiniGame[22];
            ParticleSystem.MainModule Ps = BallTr[i].GetChild(0).GetComponent<ParticleSystem>().main;
            Ps.startColor = Color.yellow;
        }
        for (int i = 0; i < BlockCol.Length; i++)
        {
            BlockCol[i].tag = "Untagged";
            BlockCol[i].isTrigger = false;
        }
    }

    //슬로우볼 아이템
    IEnumerator Item_SlowBall(bool isSkip)   
    {
        if(!isSkip)
        {
            for (int i = 0; i < 3; i++)
            {
                ballSpeed = 200f;
                BallAddForce(BallRg[i]); //힘 다시
            }
                yield return new WaitForSeconds(7.5f);
        }
        for (int i = 0; i < 3; i++)
        {
            ballSpeed = oldBallSpeed; //speed = 300
            BallAddForce(BallRg[i]);
        }
    }

    //패들크기 작아졌다가 늘어났다가 하는 아이템
    IEnumerator Item_BigOrSmall(bool isSkip)     
    {
        if(!isSkip)
        {
            PaddleSr.size = new Vector2(paddleSize, PaddleSr.size.y);
            PaddleCol.size = new Vector2(paddleSize, PaddleCol.size.y);
            yield return new WaitForSeconds(7.5f);

        }
        paddleSize = 1.56f;
        paddleBorder = 4.406f;
        PaddleSr.size = new Vector2(paddleSize, PaddleSr.size.y);   //코루틴 안에서 7.5초 후 다시 원래대로의 크기로 돌아가는 패들
        PaddleCol.size = new Vector2(paddleSize, PaddleCol.size.y);
    }

    //블록이 부숴질 때
    //ball.cs에서 사용해야해서 public화 해줌
    public void BlockBreak(GameObject Col, Transform ColTr, Animator ColAni)
    {
        //아이템 생성
        ItemGenerator(ColTr.position);
        
        //스코어 증가  콤보당 1점, 3콤보이상 3점
        score += (++combo > 3) ? 3: combo;
        ScoreText.text = score.ToString();
        //벽돌이 뿌셔지는 애니메이션 
        ColAni.SetTrigger("Break");
        S_Break.Play();     //sound 실행
        StartCoroutine(ActiveFalse(Col));

        //블럭갯수체크
        StopCoroutine("BlockCheck");
        StartCoroutine("BlockCheck");
    }

    // 16퍼센트 확률로 아이템 생성!! 아이템이 부서진 위치에 생성되도록 벡터저장
    void ItemGenerator(Vector2 ColTr)
    {
        int rand = Random.Range(1, 10000);
        if(rand <800)
        {
            string currentName = "";
            switch(rand%7) //0부터6까지
            {
                case 0:
                    currentName = "Item_TripleBall";
                    break;
                case 1:
                    currentName = "Item_Big";
                    break;
                case 2:
                    currentName = "Item_Small";
                    break;
                case 3:
                    currentName = "Item_SlowBall";
                    break;
                case 4:
                    currentName = "Item_FireBall";
                    break;
                case 5:
                    currentName = "Item_Magnet";
                    break;
                case 6:
                    currentName = "Item_Gun";
                    break;
               
            }
            //0부터 6까지의 rand 번호에 아이템 sprite 시작번호인 11을 더해주자
            P_ItemSr.sprite = B_MiniGame[rand % 7+11];
            GameObject item = Instantiate(P_Item, ColTr, Quaternion.identity);
            item.name = currentName;
            item.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 0.008f);
            item.transform.SetParent(ItemsTr);
            Destroy(item, 7f);
        }
    }

    //0.2초 후 비활성화
    IEnumerator ActiveFalse(GameObject Col)
    {
        yield return new WaitForSeconds(0.2f);
        Col.SetActive(false);
    }

    //볼에 힘을 줌
    public void BallAddForce(Rigidbody2D ThisBallRg)
    {
        //방향
        Vector2 dir = ThisBallRg.velocity.normalized; //일정한 속력 저장
        ThisBallRg.velocity = Vector2.zero;
        ThisBallRg.AddForce(dir * ballSpeed);
    }

    //볼 체크
    //아이템트리거 체크할때 3볼이 어떤 볼인지 확인하기 위해 void에서 GameObject로 바꿔주고 return을 할수 있게 만들어줫어
    GameObject BallCheck()
    {
        //볼의 갯수를 체크해줄 변수
        int BallCount = 0;
        GameObject ReturnBall = null;
        //GameObject.Find~는 활성화되어있는 것만 찾는다.
        foreach (GameObject OneBall in GameObject.FindGameObjectsWithTag("Ball"))
        {
            BallCount++;
            ReturnBall = OneBall;
        }
        print(BallCount);
        //볼이 하나도 없을때 라이프 깍임
        if(BallCount == 0)
        {
            if(Life1.activeSelf)
            {
                Life1.SetActive(false);
                StartCoroutine("BallReset");
                S_Fail.Play();
            }
            else if (Life0.activeSelf)
            {
                Life0.SetActive(false);
                StartCoroutine("BallReset");
                S_Fail.Play();
            }
            else
            {
                GameOverPanel.SetActive(true);
                S_Fail.Play();
                Clear();
            }
        }
        return ReturnBall;
    }

    //블럭 체크
    //블럭갯수세는 함수
    IEnumerator BlockCheck()
    {
        yield return new WaitForSeconds(0.5f);
        int blockCount = 0;
        for (int i = 0; i < BlocksTr.childCount; i++) //BlocksTr.childCount == 자식의 갯수 전부다 새는거
        {
            // 블럭이 활성화 되어있다면 블럭 카운트를 증가
            if (BlocksTr.GetChild(i).gameObject.activeSelf)
            {
                blockCount++;
            }
        }
        //승리
        if (blockCount == 0)
        {
            WinPanel.SetActive(true);
            S_Victory.Play();
            Clear();
        }

        //가끔 아이템 흘리게
        //블록을 체크할때마다 공중에서 아이템을 스폰
        ItemGenerator(new Vector2(Random.Range(-5f, 5f), 6.44f));
    }

   void FindCorn(Vector2 Col)
   {
       Corn = Instantiate(Corn, Col, Quaternion.identity);
       Corn.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.0002f);
   }

    //승리또는 게임오버시 호출
    void Clear()
    {
        StopAllCoroutines();
        StartCoroutine("Item_BigOrSmall", true);
        StartCoroutine("Item_SlowBall", true);
        StartCoroutine("Item_FireBall", true);
        StartCoroutine("Item_Magnet", true);
        StartCoroutine("Item_Gun", true);

        for (int i = 0; i <3; i++)
        {
            Ball[i].SetActive(false);
        }
        PaddleSr.enabled = false;
    }
}

