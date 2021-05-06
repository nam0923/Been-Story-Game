using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeakManager : MonoBehaviour
{
    public TalkManager talkManager;     //대화 매니저를 변수로 선언후, 힘수로 사용
    public QuestManager questManager;   //퀘스트 매니저를 변수로 생성 후, 퀘스트 번호를 가져옴
    public Text questText;              //퀘스트 이름 전달할 변수
    public Text nameText;               //이름 텍스트 변수

    public GameObject menuSet;
    public GameObject player;

    public Animator talkPanel;
    public Animator PortraitAnim; //초상화 애니메이터
    public Image PortraitImg;     //초상화이미지 UI
    public Sprite prevPortrait;   //과거 스프라이트 저장
    public TypeEffect talk;
    public GameObject scanObject;
    public int talkIndex;

    //상태 저장용 변수를 추가
    public bool isAction;

    private void Start()
    {
        GameLoad(); //저장된거 불러오기
        questText.text = questManager.CheckQuest();
    }

    private void Update()
    {
        //Sub Menu
        //ESC키를 누르면 메뉴가 나오도록 설정
        if(Input.GetButtonDown("Cancel"))
        {
            //ESC키로 켜고 끄기 가능하도록 작성
            if(menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            { 
                menuSet.SetActive(true);
            }
        }
    }

    public void Action(GameObject scanObj)
    {
        //대화가 모두 끝나야 액션이 끝나도록 설정
        isAction = true;
        scanObject = scanObj;
        scanObject.name = scanObj.name;
        nameText.text = scanObject.name;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc); //오브젝트 변수를 매개변수로 활용

        talkPanel.SetBool("isShow",isAction);
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";

        //저장 //Set Talk Data
        if (talk.isAnim) //매니저에서도 플래그 변수를 이용하여 분기점 로직 작성
        {
            talk.SetMsg(""); //빈값
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex); //퀘스트번호 + NPC Id = 퀘스트 대화 데이터 Id
        }

        //대화 끝났을때 //End Talk
        if(talkData == null)
        {
            isAction = false;
            //talkIndex는 대화가 끝날때 0으로 초기화
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            return;
        }

        //대화 진행 중 //Continue Talk
        if(isNpc)
        {
            //Split() : 구분자를 통하여 배열로 나눠주는 문자열 함수 , :를 통해 배열로 나눠졌으니까 []로 만들어줘. 
            talk.SetMsg(talkData.Split(':')[0]);

            //Show Portrait
             PortraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1])); //Parse():문자열을 해당 타입으로 변환해주는 함수(형변환같은거) -> string을 int로 변환
            //npc일때만 Image가 보이도록 작성
            PortraitImg.color = new Color(1, 1, 1, 1); //4번째가 alpha값 투명한게 0, 안투명한게 1

            //Animation Portrait
            if (prevPortrait != PortraitImg.sprite)  //과거 스프라이트를 저장해두어 비교후, 애니메이션 실행
            {
                PortraitAnim.SetTrigger("doEffect"); 
                prevPortrait = PortraitImg.sprite; //과거 스프라이트 현재 스프라이트로 갱신
            }
        }
        else
        {
            talk.SetMsg(talkData);

            //Hide Portrait
            PortraitImg.color = new Color(1, 1, 1, 0); //npc아닐때는 숨겨두고 npc일때만 보이도록 설정
        }
        //Next Talk
        isAction = true;
        talkIndex++; //문장 뽑아냇으면 그다음 문장 나오게 하기위해 
    }

    //저장, 블러오기 함수
    public void GameSave()
    {
        // PlayerPrefs : 간단한 데이터 저장 기능을 지원하는 클래스
        PlayerPrefs.SetFloat("PlayerX",player.transform.position.x);                //player.x
        PlayerPrefs.SetFloat("PlayerY",player.transform.position.y);                //player.y
        PlayerPrefs.SetInt("QuestId",questManager.questId);                         //Quest.Id
        PlayerPrefs.SetInt("QuestActionIndex",questManager.questActionIndex);       //Quest Action Index
        //Save 기능 함수
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }
    public void GameLoad()
    {
        //최초 게임을 실행했을 땐 데이터가 없으므로 예외처리 로직 작성
        if(!PlayerPrefs.HasKey("PlayerX")) //한번도 키가 저장이 안되있다면 로드를 하지말고 넘어가자.
        {
            return;
        }

        //불러오기 또한 데이터 타입에 맞게 Get함수 사용
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        //불러온 데이터를 게임 오브젝트에 적용
        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;

        questManager.ControlObject();
    }

    //재시작 버튼 함수
    public void ReStart()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("SampleScene");
    }
    public void IntroStart()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Intro");
    }

    //종료하기 버튼을 위한 함수
    public void GameExit()
    {
        //게임 종료함수 
        Application.Quit();     //Application.Quit()는 에디터에서는 실행되지 않음 -> 빌드파일에서 실행
    }
}
