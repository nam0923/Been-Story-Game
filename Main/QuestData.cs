using System.Collections;
using System.Collections.Generic;
//using UnityEngine;  <- MonoBehaviour 사용안하니까 지움

public class QuestData //코드에서 불러서 사용할것이니까 MonoBehaviour 지움
{
    public string questName;
    public int[] npcId; //퀘스트와 연관되있는 npc의 id저장할 배열변수필요

    //구조체 생성을 위해 매개변수 생성자를 작성
    public QuestData (string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }

    void Start()
    {
    }
    void Update()
    {
    }
}
