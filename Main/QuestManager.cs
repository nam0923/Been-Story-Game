using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public int questId;

    //퀘스트 대화순서 변수 생성
    public int questActionIndex;

    //퀘스트 오브젝트를 저장할 변수 생성
    public GameObject[] questObject;

    //퀘스트 화살표 UI
    public Text CheckPoint01;
    public Text CheckPoint02;
    public Text CheckPoint03;
    public Text CheckPoint04;
    Color CheckColor;

    //퀘스트UI 투명도 올리기
    public Image EggGetUI;
    public Image CornGetUI;
    public Image FishGetUI;
    public Image DragonGetUI;

    //퀘스트 데이터를 저장할 Dictionary 변수생성
    Dictionary<int, QuestData> questList;

    void Awake()
    {
        CheckColor = new Color(255 / 255, 148 / 255, 0);

        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }
    void GenerateData()
    {
        //Add함수로 <QuestId, QuestData> 데이터를 저장  //퀘스트 순서 {1000, 2000} == 해당 퀘스트에 연관된 NPC Id를 입력 npc_01 다음 npc_02
        questList.Add(10,new QuestData("마을 사람들과 대화하기.", new int[] { 1000, 2000})); //int [] 에는 해당퀘스트에 연관된 npc Id를 입력
        questList.Add(20,new QuestData("떠돌이 계란 찾기.", new int[] { 5000, 2000})); 
        questList.Add(30, new QuestData("꽃밭에서 옥수수 찾기.", new int[] { 3000,2000 }));
        questList.Add(40, new QuestData("돌이 지키던 물고기 찾기.", new int[] { 1000,3000,6000 }));
        questList.Add(50, new QuestData("축복을 찾아서.", new int[] { 6000,8000 }));
        questList.Add(60, new QuestData("미션 클리어.", new int[] { 0 }));
    }

    //Npc Id를 받고 퀘스트번호를 반환하는 함수 생성
    public int GetQuestTalkIndex(int id)
    {
        //퀘스트번호 + 퀘스트 대화순서 = 퀘스트 대화 Id
        return questId + questActionIndex;
    }

    //대화진행을 위해 퀘스트 대화 순서를 올리는 함수 생성
    public string CheckQuest(int id)
    {
        //Next Talk Target
        //순서에 맞게 대화했을때만 퀘스트 대화 순서를 올리도록 작성
        if (id == questList[questId].npcId[questActionIndex])
        questActionIndex++;

        //Control Quest Object
        ControlObject();
        
        //Talk Complete & Next Quest
        //퀘스트 대화 순서가 끝에 도달했을 때 퀘스트 번호 증가
        if(questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }
        return questList[questId].questName;
    }
    public string CheckQuest()
    {
        //Quest Name
        return questList[questId].questName;
    }

    //다음 퀘스트를 위한 함수 생성
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0; //0으로 다시 초기화
    }

    //퀘스트 오브젝트를 관리할 함수 생성
    public void ControlObject()
    {     
        switch (questId) // 10 ~ 60
        {
            case 10:
                //퀘스트 번호, 퀘스트 대화 순서에 따라 오브젝트 조절
                if(questActionIndex == 2)
                {
                    CheckPoint01.GetComponent<Text>().color = CheckColor;
                    questObject[0].SetActive(true);
                }
                break;
            case 20:
                if (questActionIndex == 0)
                { 
                    questObject[0].SetActive(true); // 불러오기 했을 당시의 퀘스트 순서와 연결된 오브젝트 관리 추가    
                }
                else if (questActionIndex == 1) //퀘스트 에그 먹었을때 엑티브 false 처리.
                {
                    EggGetUI.GetComponent<Image>().color = new Color(EggGetUI.color.r, EggGetUI.color.g, EggGetUI.color.b, 255f);
                    questObject[0].SetActive(false);
                }
                break;
            case 30:
                if (questActionIndex == 1)
                {
                    CheckPoint02.GetComponent<Text>().color = CheckColor;
                    questObject[1].SetActive(true);
                }
                else if (questActionIndex == 2) 
                {
                    questObject[1].transform.position = new Vector3(22.83f, -10.23f, 0);
                    CornGetUI.GetComponent<Image>().color = new Color(CornGetUI.color.r, CornGetUI.color.g, CornGetUI.color.b, 255f);
                }
                break;
            case 40:
                if(questActionIndex == 0)
                {
                    questObject[1].SetActive(true);
                    questObject[1].transform.position = new Vector3(22.83f, -10.23f, 0);

                    CheckPoint02.GetComponent<Text>().color = CheckColor;
                    CornGetUI.GetComponent<Image>().color = new Color(CornGetUI.color.r, CornGetUI.color.g, CornGetUI.color.b, 255f);
                }
                else if (questActionIndex == 1)
                {
                    CheckPoint03.GetComponent<Text>().color = CheckColor;
                }
                else if (questActionIndex == 2) //퀘스트 물고기 등장
                {
                    questObject[1].SetActive(true);
                    questObject[1].transform.position = new Vector3(22.83f, -10.23f, 0);

                    CheckPoint02.GetComponent<Text>().color = CheckColor;
                    CornGetUI.GetComponent<Image>().color = new Color(CornGetUI.color.r, CornGetUI.color.g, CornGetUI.color.b, 255f);

                    CheckPoint03.GetComponent<Text>().color = CheckColor;
                    questObject[2].SetActive(true);
                }
                else if (questActionIndex == 3)
                {
                    questObject[2].transform.position = new Vector3(4.51f, -16.56f, 0);
                    FishGetUI.GetComponent<Image>().color = new Color(FishGetUI.color.r, FishGetUI.color.g, FishGetUI.color.b, 255f);
                    CheckPoint04.GetComponent<Text>().color = CheckColor;
                }
                break;
            case 50:
                if (questActionIndex == 1)
                {
                    CheckPoint04.GetComponent<Text>().color = CheckColor;
                    questObject[3].SetActive(true);
                }
                else if (questActionIndex == 2) //퀘스트 인형 등장
                {
                    questObject[3].SetActive(false);
                    DragonGetUI.GetComponent<Image>().color = new Color(DragonGetUI.color.r, DragonGetUI.color.g, DragonGetUI.color.b, 255f);
                }
                break;
            case 60:
                if (questActionIndex == 0)
                {
                    DragonGetUI.GetComponent<Image>().color = new Color(DragonGetUI.color.r, DragonGetUI.color.g, DragonGetUI.color.b, 255f);
                }
                break;
        }

        if (questId > 20)
        {
            CheckPoint01.GetComponent<Text>().color = CheckColor;
            EggGetUI.GetComponent<Image>().color = new Color(EggGetUI.color.r, EggGetUI.color.g, EggGetUI.color.b, 255f);
        }
        else if (questId > 30)
        {
            CheckPoint02.GetComponent<Text>().color = CheckColor;
            CornGetUI.GetComponent<Image>().color = new Color(CornGetUI.color.r, CornGetUI.color.g, CornGetUI.color.b, 255f);
        }
        else if (questId > 40)
        {
            CheckPoint03.GetComponent<Text>().color = CheckColor;
            FishGetUI.GetComponent<Image>().color = new Color(FishGetUI.color.r, FishGetUI.color.g, FishGetUI.color.b, 255f);
        }
    }
}
