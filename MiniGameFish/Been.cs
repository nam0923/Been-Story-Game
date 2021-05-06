using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Been : MonoBehaviour
{
    public int MaxHp;
    public int nowHp;
    public int attackDmg;
    public float attackSpeed;
    public bool isAttack;
    public Image nowHpBar;
    public Image UInowHpBar;

    //프리팹 & 총알
    public string bulletName = "Bullet";
    public string bulletName02 = "Bullet02";
    public string bulletName03 = "Bullet03";
    public GameObject PrefHpBarBeen;
    public GameObject UIfHpBarBeen;

    public Canvas canvas;
    public Transform BulletPos;
    RectTransform HpBarBeen;
    RectTransform UIHpBarBeen;
   
    int height = 2; //체력바 y위치

    bool isDeath;

    public Attack attackPoint;

    public float speed = 3;
    Rigidbody2D rigid;
    public float h;
    float v;

    bool isHorizonMove;
    bool isAction;

    Vector3 dirVec;
    Vector3 flip;

    Animator anim;
    //방향 전환하려고
    SpriteRenderer spriteRenderer;

    public Fish fish;
    public bool isFlip;

    public GameObject GameOverPannel;
    public GameObject WinPannel;
    public bool isFinish;

    public GameObject UIFireBullet;
    public GameObject UIPaBullet;
    public GameObject UIPunchBullet;
    Color UIBulletColr;
    Color UIBulletBaseColr;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        HpBarBeen = Instantiate(PrefHpBarBeen, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = HpBarBeen.transform.GetChild(0).GetComponent<Image>();
        
        UIHpBarBeen = UIfHpBarBeen.GetComponent<RectTransform>();
        UInowHpBar = UIfHpBarBeen.transform.GetChild(0).GetComponent<Image>();

        MaxHp = 50;
        nowHp = 50;
        attackDmg = 10;
        speed = 4;
        UIBulletColr = new Color(236 / 255, 255 / 255, 0);
        UIBulletBaseColr = new Color(255 / 255, 255 / 255, 255/255);
    }
    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;
    }
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        HpBarBeen.position = _hpBarPos;

        nowHpBar.fillAmount = (float)nowHp / (float)MaxHp;
        UInowHpBar.fillAmount = (float)nowHp / (float)MaxHp;

        if (!isDeath && !isFinish)
        {
            Flip();

            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            if (rigid.velocity.normalized.x == 0)
            {
                anim.SetBool("isMove", false);
            }
            else
            {
                anim.SetBool("isMove", true);
            }

            bool hDown = Input.GetButtonDown("Horizontal");
            bool vDown = Input.GetButtonDown("Vertical");
            bool hUp = Input.GetButtonUp("Horizontal");
            bool vUp = Input.GetButtonUp("Vertical");

            if (hDown)
            {
                isHorizonMove = true;
            }
            else if (vDown)
            {
                isHorizonMove = false;
            }
            else if (hUp || vUp)
            {
                isHorizonMove = h != 0;
            }

            if (vDown && v == 1)
            {
                dirVec = Vector3.up;
            }
            else if (vDown && v == -1)
            {
                dirVec = Vector3.down;
            }
            else if (hDown && h == 1)
            {
                dirVec = Vector3.right;
            }
            else if (hDown && h == -1)
            {
                dirVec = Vector3.left;
            }


            if (Input.GetKeyDown(KeyCode.Z) && !anim.GetCurrentAnimatorStateInfo(0).IsName("isKick"))
            {
                isAction = true;
                anim.SetBool("isKick", true);
                
            }
            if (Input.GetKeyDown(KeyCode.X) && !anim.GetCurrentAnimatorStateInfo(0).IsName("isHead"))
            {
                isAction = true;
                anim.SetBool("isHead", true);

                int randBullet = Random.Range(0,3);
                if (randBullet == 0)
                {
                    Shoot(); 
                }
                else if (randBullet == 1)
                {
                    Shoot02();
                }
                else
                {
                    Shoot03();
                }
            }
            if (Input.GetKeyDown(KeyCode.C) && !anim.GetCurrentAnimatorStateInfo(0).IsName("isPunch"))
            {
                isAction = true;
                anim.SetBool("isPunch", true); 
            }

            if (isAction)
            {
                StartCoroutine(TimeAnim());
            }
            else if (!isAction)
            {
                AnimReset();
            }

            if(nowHp<=0)
            {
                isDeath = true;
                Die();
            }
            
        }
        if(isFinish)
        {
            HpBarBeen.GetChild(0).GetComponent<Image>().enabled = false;
            HpBarBeen.GetComponent<Image>().enabled = false;
        }
    }

    IEnumerator TimeAnim()
    {
        yield return new WaitForSeconds(0.5f);
        isAction = false;
    }

    void AnimReset()
    {
        //isAnimReset = true;
        anim.SetBool("isKick", false);
        anim.SetBool("isHead", false);
        anim.SetBool("isPunch", false);
    }
    void Die()
    {
        isDeath = true;
        isFinish = true;
        fish.isFinish = true;
        anim.SetBool("isDeath", true);
        
        GetComponent<Collider2D>().enabled = false; //충돌체 비활성화
        HpBarBeen.GetComponent<Image>().enabled = false;
        HpBarBeen.GetComponentInChildren<Image>().enabled = false;
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameOverPannel.SetActive(true);
    }

    
    void Shoot()
    {
        UIPunchBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletColr);
        attackDmg = 10;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName);
        bullet.transform.position = BulletPos.transform.position + transform.forward;
        bullet.SetActive(true);
        StartCoroutine(UIBUlletBase());
    }
    void Shoot02()
    {
        UIPaBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletColr);
       
        attackDmg = 15;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName02);
        bullet.transform.position = BulletPos.transform.position + transform.forward;

        bullet.SetActive(true);
        StartCoroutine(UIBUlletBase());
    }
    void Shoot03()
    {
        UIFireBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletColr);
        attackDmg = 20;
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName03);
        bullet.transform.position = BulletPos.transform.position + transform.forward;
        bullet.SetActive(true);
        StartCoroutine(UIBUlletBase());
    }

    IEnumerator UIBUlletBase()
    {
        yield return new WaitForSeconds(0.3f);
        UIPunchBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletBaseColr);
        UIFireBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletBaseColr);
        UIPaBullet.GetComponent<CanvasRenderer>().SetColor(UIBulletBaseColr);
    }

    public void AttackPunch()
    {
        attackPoint.gameObject.SetActive(true);
    }
    public void AttackPunchFail()
    {
        attackPoint.gameObject.SetActive(false);

        flip = attackPoint.transform.localScale;
        if (fish.transform.position.x > this.transform.position.x)
        {
            flip.x = -0.15f;
        }
        else
        {
            flip.x = 0.15f;
        }
        attackPoint.transform.localScale = flip;

    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "ItemFish")
        {
            coll.gameObject.SetActive(false);
        }
    }

    void Flip()
    {
        Vector3 flip = transform.localScale;
        if (transform.position.x > fish.transform.position.x)
        {
            flip.x = -4f;
            isFlip = true;
        }
        else
        {
            flip.x = 4f;
            isFlip = false;
        }
        this.transform.localScale = flip;
    }
}
