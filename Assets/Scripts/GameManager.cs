using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GroundPiece[] allGroundPieces;

    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        } else if(Instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        SetUpNewLevel();
    }

    void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    void OnEnable() 
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;    
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if(allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if(isFinished)
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 50)
        {
            SceneManager.LoadScene(0);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
   
}
