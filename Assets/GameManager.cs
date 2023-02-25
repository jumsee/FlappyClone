using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public GameObject tubePrefab;
    public float tubeDistance;
    public float minTubeDistance;
    public float spawnTimer;
    public List<GameObject> tubes;
    public int score = 0;
    public int highScore = 0; 
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text controlsText;
    private AudioSource scoreDing;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        scoreDing = GetComponent<AudioSource>();
        ResetGame();
    }

    private void SpawnTubes()
    {
        tubes.Add(Instantiate(tubePrefab));
    }

    public void ResetGame()
    {
        score = 0;
        
        // Destroy and reset tubes
        foreach (GameObject tube in tubes)
        {
            Destroy(tube);
        }

        CancelInvoke();
        
        GameObject t1 = Instantiate(tubePrefab);
        t1.transform.position = new Vector3(10, t1.transform.position.y, t1.transform.position.z);
        GameObject t2 = Instantiate(tubePrefab);
        t2.transform.position = new Vector3(2, t2.transform.position.y, t2.transform.position.z);
        tubes.Add(t1);
        tubes.Add(t2);

        controlsText.enabled = true;
    }

    public void StartGame()
    {
        InvokeRepeating(nameof(SpawnTubes), 0, spawnTimer);
        controlsText.enabled = false;
    }

    public void IncrementScore()
    {
        scoreDing.Play();
        
        score++;
        scoreText.text = "Score: " + score;
        
        // Reset score and save high score
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;
        }
    }
}