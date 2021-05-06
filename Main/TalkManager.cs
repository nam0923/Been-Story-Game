using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{   //대화데이터를 저장할 Dictionary변수 생성 // Key와 Value값으로 구성되어있는 Dictionary 사용
    //obj의 id를 담을 int형, obj의 이름을 담을 string형 배열
    Dictionary<int, string[]> talkData;

    //npc얼굴바뀌게  초상화 데이터 저장할 Dictionary 변수 생성
    Dictionary<int, Sprite> portraitData;

    //스프라이트는 바로 못가져와!! 초상화 스프라이트를 저장할 배열 생성
    public Sprite[] portraitArr; 

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();

        GenerateData();
    }
    
    //데이터 생성함수
    void GenerateData()
    {
        // Talk Data
        // NPC_01 : 1000, NPC_02 : 2000  돌: 3000, 칠판 : 4000, 계란: 5000

        //딕셔너리에 데이터 넣는 방법 ==> Add함수를 사용. 대화 데이터 추가 
        //npc표정은 문장과 1:1매칭 --> 구분자(:)와 함께 초상화 Index를 문장뒤에 추가
        talkData.Add(1000, new string[] { "음,,안녕!!:0", "못보던 얼굴인데? :1" }); //npc_01의 아이디 1000과 그가 할 말 //대화 하나에는 여러 문장이 들어있으므로 string[]배열 사용
        talkData.Add(2000, new string[] { "여허! 내 오랜 친구. :2", "이야, 섬에서 나온거야? :0" });
        talkData.Add(3000, new string[] { "특이하게 생긴 돌이다."});
        talkData.Add(4000, new string[] { "누군가 게시물을 붙여둔 칠판이다.", "호수에 가면 물고기를 만날 수 있다...", "용은 물고기를 좋아한다..?"});
        talkData.Add(5000, new string[] { "내 신세!! 어쩌다 금값이 되어 쫒기고 있는가..."});
        talkData.Add(6000, new string[] { "물고기가 하나도 안보이는군...", "내가 좋아하는 물고기..."});
        talkData.Add(7000, new string[] { "지금은 흔적도 없는 옥수수 밭" ,"어딘가엔 옥수수 알갱이가 떨어져있을 수 있다."});
        talkData.Add(8000, new string[] { "축복 그 자체. 짐이 축복이고 축복이 짐이다."});
        talkData.Add(9000, new string[] { "뭐야, 내가 보여?"});


        //Quest Talk        // 퀘스트 번호 + NPC Id에 해당하는 대화 데이터 작성
        talkData.Add(10 + 1000, new string[] { "혹시 옆 동네에서 왓니? :2", "거기 계란파동이라며, 여기 떠돌이 계란이 있어...:3", "나한테 가져오면 돈을 주지!:0", "일단 어제 계란을 봣다던 노랑이한테 가보라구. :1"});
        //questManager.checkQuest()함수를 통해 퀘스트 번호 인덱스 1증가
        talkData.Add(11 + 2000, new string[] { "여허! 내 오랜 친구. :2", "아, 너도 소문듣고 왔구나. 떠돌이 계란... :0", "호수 근처에 있던데. 친구니까 알려준다. 어서가봐. :2"});

        talkData.Add(20 + 5000, new string[] { "계란이다!!", "오늘은 소고기를 먹겠어."});
        talkData.Add(21 + 1000, new string[] { "찾았구나! 믿고있었어.:2"});

        talkData.Add(30 + 3000, new string[] { "넌 뭐야? 나에게 말을 건 외지인은 너가 처음이야.", "미션을 주지, 꽃밭에 가서 노란 보석을 찾아와.", "찾으면 이 동네에 비슷하게 생긴 애한테 가져다줘." });
        talkData.Add(31 + 2000, new string[] { "용캐 발견했네... :0", "받기까지 했는데, 뭔가를 줘야겠지. :1","좀 생각해볼테니 기다려봐. :1" , "500원만 주려했는데, 옆에 친구가 더큰걸 주겠데. 가봐. :1"});

        talkData.Add(40 + 1000, new string[] { "지난번에 너가 계란 찾아주고 못받은 돈 대신 알려줄께 :1","괜찮지? 그럼 용의 선물이 있는 곳을 알려줄께. :2", "용의 선물은 돌이 지키는 풀밭에 있어. 잘 찾아봐봐.:0" });
        talkData.Add(41 + 3000, new string[] { "여긴 아무것도 없어!!", "물고기 같은건 전혀 없다구..!!" });
        talkData.Add(42 + 6000, new string[] { "내가 좋아하는 물고기 냄새!!", "넘겨! 축복을 주겠다." });

        talkData.Add(50 + 6000, new string[] { "축복은 너희 집에 보내두었으니 가. 이곳에서 나갈 수 있을꺼야"});
        talkData.Add(51 + 8000, new string[] { "미션 클리어." });
      
        talkData.Add(61 + 8000, new string[] { "미션 클리어." });

        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);

        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);
    }

    //지정된 대화 문장을 반환하는 함수 하나 생성
    public string GetTalk(int id, int talkIndex) // string []배열에 담긴 문장중 첫번째 문장을 가져올지 두번째 문장을 가져올지 결정한 talkIndex
    {
        //ContainsKey() : Dictionary에 Key가 존재하는 지 검사
        if (!talkData.ContainsKey(id)) //데이터 없을때
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                //Get First Talk            
                // 반환값이 있는 재귀함수는 return까지 꼭 써주어야 함
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {
                //Get First Quest Talk      
                return GetTalk(id - id % 10, talkIndex);
            }
        }

        //talkIndex와 대화의 문장 갯수를 비교하여 끝을 확인
        if (talkIndex == talkData[id].Length)
            return null; //더이상 남아있는 문장이 없다. 이대화는 끝났다!!
        else
             //id로 대화 Get -> talkIndex로 대화의 한문장을 Get
            return talkData[id][talkIndex];
    }

    //지정된 초상화 스프라이트를 반환랄 함수 생성
    public Sprite GetPortrait(int id, int portraitIndex) //portraitIndex == 초상화 Arr Index
    {
        return portraitData[id + portraitIndex];
    }

}
