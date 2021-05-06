using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;
    
    //글자 재생 속도를 위한 변수를 생성(==CPS)
    public int CharPerSeconds;

    //애니메이션 실행 판단을 위한 플래그 변수 생성
    public bool isAnim;

    //표시할 대화 문자열을(원래있던 대화) 따로 변수로 저장
    string targetMsg;

    //Text변수 생성, 초기화 후 시작함수에서 공백처리
    Text msgText;

    //AudioSource 변수를 생성, 초기화 후 재생 함수에서 Play()
    AudioSource audioSource;

    int index;
    float interval; //확실한 소수값을 얻기 위해 분자 1.0f 작성 


    void Awake()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    //대화 문자열을 받는 함수 생성
    public void SetMsg(string msg)
    {
        //플래그 변수를 이용하여 분기점 로직 작성
        if (isAnim)
        {
            //글자다 채우기
            msgText.text = targetMsg;
            CancelInvoke(); //CancelInvoke() == 지금 돌고있는 Invoke() 함수 꺼짐.
            EffectEnd();
        }
        else
        {
            //값이 들어오면 변수에 저장
            targetMsg = msg;
            EffectStart();
        }
    }

    //애니메이션 재생을 위한 시작 - 재생 - 종료 의 세개 함수 생성
    private void EffectStart() //시작
    {
        msgText.text = "";
        index = 0; //인덱스 초기화
        EndCursor.SetActive(false);

       // Start Animation //시간차 반복 호출을 위한 invoke 함수를 사용
        interval = 1.0f / CharPerSeconds;    // 1초 / CharPerSeconds = 1글자가 나오는 딜레이
        isAnim = true;
        Invoke("Effecting", interval);
    }
    private void Effecting()    //하는중
    {
        // End Animation //대화 문자열과 Text 내용이 일치하면 종료
        if (msgText.text == targetMsg)
        {
            EffectEnd(); //끝처리
            return;
        }
        //문자열도 배열처럼 char값에 접근 가능
        msgText.text += targetMsg[index];
        //Sound
        if(targetMsg[index] != ' ' || targetMsg[index] != '.' ) //공백과 마침표는 사운드 재생 제외
        {
            audioSource.Play();
        }
       
        index++; //반복적으로 실행

        Invoke("Effecting",interval); //재귀함수
    }
    private void EffectEnd()        //끝
    {
        isAnim = false;
        //종료함수에서는 대화 마침 아이콘을 활성화 (시작에선 반대)
        EndCursor.SetActive(true);
    }
   
}
