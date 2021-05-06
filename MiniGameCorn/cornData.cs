using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cornData : MonoBehaviour
{
    private Vector2 conPosMin;
    private Vector2 conPosMax;

    public GameObject GetCorn; //파티클

    public SpeakManager speakManager;

    private void Awake()
    {
        conPosMin = new Vector3(11.17f, -12.82f, 0);
        conPosMax = new Vector3(18.7f, -7.59f, 0);
    }
    void Start()
    {
        transform.position = new Vector3(Random.Range(conPosMin.x, conPosMax.x), Random.Range(conPosMin.y, conPosMax.y), 0);
        GetCorn.transform.position = transform.position;
    }  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            speakManager.GameSave();
            StartCoroutine(LoadCornGame());
        }
    }
    
    IEnumerator LoadCornGame()
    {
        GetCorn.SetActive(true);
        yield return new WaitForSeconds(2f);
        GetCorn.SetActive(false);
        SceneManager.LoadScene("cornGame");
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
    
