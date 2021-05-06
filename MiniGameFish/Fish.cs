using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    public GameObject PrefHpBarFish;
    public GameObject UIfHpBarFish;
    public GameObject UIDefenseBullet;
    public GameObject UIDefenseBullet2;
    public GameObject UIDefenseBullet3;
    public GameObject UIDefenseBullet4;
    public GameObject UIDefenseBullet5;
    public GameObject UIDefenseTex;
    public GameObject UIDefenseFailTex;
    public Canvas canvas;
    RectTransform HpBarFish;
    RectTransform UIHpBarFish;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    int height = 2;
 
    public Been been;
    public Transform target;
    public GameObject itemFish;
    public FishAi fishAi;
    Animator anim;

    public float speed = 0.4f;
  
    public int maxHp = 100;
    public int nowHp = 100;

    public int attackKickDmg = 5;
    public int attackPunchDmg = 7;
    public int attackBulletDmg = 10;

    public int attackSpeed = 2;
    public float attackRange = 3f;
    public float fileldOfVision = 9f;

    int WalkCount = 20;
    int defenseCount = 0;
    int defenseBulletCount = 0;

    Image nowHpBar;
    public Image UInowHpBar;

    bool isDeath;
    public bool isFinish;
    public bool isDefense;

    public GameObject WinPannel;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        HpBarFish = Instantiate(PrefHpBarFish, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = HpBarFish.transform.GetChild(0).GetComponent<Image>();

        UIHpBarFish = UIfHpBarFish.GetComponent<RectTransform>();
        UInowHpBar = UIHpBarFish.transform.GetChild(0).GetComponent<Image>();

        maxHp = 100;
        nowHp = 100;
        attackRange = 2f;
        attackKickDmg = 5;
        attackPunchDmg = 7;
        attackBulletDmg = 10;
        attackSpeed = 2;
        speed = 0.4f;
    }

    void Update()
    {
        //체력바
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        HpBarFish.position = _hpBarPos;
        nowHpBar.fillAmount = (float)nowHp / (float)maxHp;
        UInowHpBar.fillAmount = (float)nowHp / (float)maxHp;

        if (NearPlayer() && !isDeath && !isFinish)
        {
            Flip();
            return;
        }
       
        if(isDeath)
        {
            Die();
        }

        if(isFinish)
        {   
            HpBarFish.GetComponent<Image>().enabled = false;
            HpBarFish.GetChild(0).GetComponent<Image>().enabled = false;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            DefenseBulletSetting();
            defenseCount++;
            
            if (defenseCount > 5)
            {
                print("방어실패");
                isDefense = false;
                StartCoroutine(DefenseFail());
            }
            else
            {
                defenseBulletCount++;
                print("방어성공");
                isDefense = true;
            }
            
            if(nowHp<=0)
            {
                isDeath = true;
                Die();
            }
        }
        if(collision.CompareTag("NearDmg"))
        {
            nowHp -= been.attackDmg;
        }
    }

    void DefenseBulletSetting()
    {
        if(defenseBulletCount == 4 )
        {
            StartCoroutine(DefenseDelayDefenseTex());

            UIDefenseBullet.SetActive(false);
        }
        else if (defenseBulletCount == 3)
        {
            UIDefenseBullet2.SetActive(false);
        }
        else if (defenseBulletCount == 2)
        {
            UIDefenseBullet3.SetActive(false);
        }
        else if (defenseBulletCount == 1)
        {
            UIDefenseBullet4.SetActive(false);
        }
        else if (defenseBulletCount == 0)
        {
            UIDefenseBullet5.SetActive(false); 
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(DefenseFailDelayUI());
        }
    }

    IEnumerator DefenseDelayDefenseTex()
    {
       
        UIDefenseTex.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UIDefenseTex.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIDefenseTex.SetActive(false);
        yield return new WaitForSeconds(0.1f); 
        UIDefenseTex.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIDefenseTex.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UIDefenseTex.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UIDefenseFailTex.SetActive(true);
    }
    IEnumerator DefenseFailDelayUI()
    {
        yield return new WaitForSeconds(0.7f);
        UIDefenseFailTex.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UIDefenseFailTex.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        UIDefenseFailTex.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        UIDefenseTex.SetActive(true);
        UIDefenseBullet.SetActive(true);
        UIDefenseBullet2.SetActive(true);
        UIDefenseBullet3.SetActive(true);
        UIDefenseBullet4.SetActive(true);
        UIDefenseBullet5.SetActive(true);
        defenseBulletCount = 0;
    }
    IEnumerator DefenseFail()
    {
        nowHp -= been.attackDmg;
        
        yield return new WaitForSeconds(1f);
        defenseCount = 0;
        defenseBulletCount = 0;
        isDefense = true;
    }

    void Die()
    {
        isDeath = true;
        isFinish = true;
        been.isFinish = true;
        anim.SetBool("isDeath", true);
        GetComponent<FishAi>().enabled = false; //추적 Ai 비활성화
        GetComponent<Collider2D>().enabled = false; //충돌체 비활성화

        StartCoroutine(DieItem());   
    }

    IEnumerator DieItem()
    {
        itemFish.transform.position = transform.position;

        HpBarFish.GetComponent<Image>().enabled = false;
        HpBarFish.GetComponentInChildren<Image>().enabled = false;
        
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        WinPannel.SetActive(true);
        
    }

    void Flip()
    {  
        Vector3 flip = transform.localScale;
        if(been.transform.position.x > this.transform.position.x)
        {
            flip.x = -5f;
        }
        else
        {
            flip.x = 5f;
        }
        this.transform.localScale = flip;
    }

    private bool NearPlayer()
    {
        if(Mathf.Abs(target.transform.position.x) - Mathf.Abs(this.transform.position.x) <= speed* WalkCount * 1.01f)
        {
            if (Mathf.Abs(target.transform.position.y) - Mathf.Abs(this.transform.position.y) <= speed * WalkCount * 0.5f)
            {
                return true;
            }
        }
        if (Mathf.Abs(target.transform.position.y) - Mathf.Abs(this.transform.position.y) <= speed * WalkCount * 1.01f)
        {
            if (Mathf.Abs(target.transform.position.x) - Mathf.Abs(this.transform.position.x) <= speed * WalkCount * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator WaitForKick()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isKick", false);
    }
}
