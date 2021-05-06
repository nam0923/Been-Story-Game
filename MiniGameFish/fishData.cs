using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fishData : MonoBehaviour
{
    private Vector2 fishPosMin;
    private Vector2 fishPosMax;

    public GameObject winPannel;
    public GameObject GameOverPannel;

    public GameObject CornPos;

    public SpeakManager speakManager;
    private void Awake()
    {
        fishPosMin = new Vector3(-3.46f, -12.38f, 0);
        fishPosMax = new Vector3(4.6f, -8.45f, 0);
    }
    void Start()
    {
        transform.position = new Vector3(Random.Range(fishPosMin.x, fishPosMax.x), Random.Range(fishPosMin.y, fishPosMax.y), 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speakManager.GameSave();
            StartCoroutine(LoadFishGame());
        }
    }

    IEnumerator LoadFishGame()
    {
        CornPos.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("fishGame");
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("SampleScene");
        speakManager.GameLoad();
    }

    public void LoadFishGameAgain()
    {
        AllReset();
        SceneManager.LoadScene("fishGame");    
    }

    public void AllReset()
    {
        winPannel.SetActive(false);
        GameOverPannel.SetActive(false);
    }
}

