using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager>
{
    static int reRoadCheck = 0;
    [NonSerialized] public bool reRoad = false;

    public GameObject[] cameras;

    public PlayableDirector[] directors;
    public GameObject[] timelines;

    public Material material;

    [NonSerialized] public bool mainGame = false;
    bool skip = false;

    public float gameSpeed = 1.0f;

    void Start()
    {
        if(reRoadCheck <= 0)
        {
            reRoad = false;
        }
        else
        {
            reRoad = true;
        }

        if(!reRoad)
        {
            TimelineInit();
            CameraChange(0);
        }
        else
        {
            TimelineInit();
            CameraChange(1);

            PlayTimeline(0);

            if (directors[0].state == PlayState.Playing)
            {
                directors[0].time = 4;
            }

            PlayTimeline(1);
        }
    }

    void Update()
    {
        DemoSkip();
        TimeControl();
    }

    public void CameraChange(int cameraNum)
    {
        CameraInit();

        cameras[cameraNum].SetActive(true);
    }

    private void CameraInit()
    {
        for(int i = 0 ; i < cameras.Length ; i++)
        {
            cameras[i].SetActive(false);
        }
    }

    public void PlayTimeline(int timelineNum)
    {
        timelines[timelineNum].SetActive(true);
        directors[timelineNum].Play();
    }

    private void TimelineInit()
    {
        for (int i = 0; i < timelines.Length; i++)
        {
            timelines[i].SetActive(false);
        }
    }

    public void Tutorial()
    {
        MainMenuManager.Instance.Transition_Menu(MainMenuManager.Instance.tutorialCanNum);
        skip = false;
    }

    public void DemoSkip()
    {
        if(directors[0].state == PlayState.Playing && Input.GetKeyDown(KeyCode.Space) && !skip)
        {
            directors[0].time = 4;
            skip = true;
        }

        if (directors[1].state == PlayState.Playing && Input.GetKeyDown(KeyCode.Space) && !skip)
        {
            directors[1].time = 3;
            skip = true;
        }
    }

    public void GameStart()
    {
        mainGame = true;

        CameraChange(1);
        MainMenuManager.Instance.Transition_Menu(MainMenuManager.Instance.gameCanNum[0]);
    }

    public void GameFinish()
    {
        mainGame = false;

        MainMenuManager.Instance.Transition_Menu(7);
    }

    public void SceneReset()
    {
        reRoadCheck += 1;
        FadeManager.Instance.LoadScene("MainScene", 1);
    }

    void TimeControl()
    {
        Time.timeScale = gameSpeed;
    }
}
