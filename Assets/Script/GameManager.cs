using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    static int reRoadCheck = 0;
    [NonSerialized] public bool reLoad = false;

    public GameObject[] cameras;

    public PlayableDirector[] directors;
    public GameObject[] timelines;

    public Material material;

    [NonSerialized] public bool mainGame = false;
    bool skip = false;

    [NonSerialized] public float gameSpeed = 1.0f;

    bool pose = false;

    void Start()
    {
        TimeControl(1);

        Debug.Log(reRoadCheck);

        if(reRoadCheck <= 0)
        {
            ReStart(0);
        }
        else if(reRoadCheck == 1)
        {
            ReStart(1);
        }
        else if (reRoadCheck == 2)
        {
            ReStart(2);
        }
        else if (reRoadCheck == 3)
        {
            ReStart(3);
        }
    }

    void Update()
    {
        DemoSkip();
        Pose();
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

    public void DemoStart()
    {
        PlayTimeline(0);
    }

    public void DemoFinish()
    {
        PlayTimeline(1);
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

    void Pose()
    {
        if(!pose && mainGame && Input.GetKeyDown(KeyCode.Escape))
        {
            TimeControl(0);
            MainMenuManager.Instance.canvas[8].SetActive(true);
            ButtonFocus(8);

            pose = true;
        }
        else if (pose && mainGame && Input.GetKeyDown(KeyCode.Escape))
        {
            TimeControl(1);
            MainMenuManager.Instance.canvas[8].SetActive(false);

            pose = false;
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
        PlayTimeline(5);

        MainMenuManager.Instance.canvas[6].SetActive(true);
    }

    public void ButtonFocus(int focusNum)
    {
        EventSystem.current.SetSelectedGameObject(MainMenuManager.Instance.focusObject[focusNum]);
    }

    public void SceneReLoad(int select)
    {
        switch (select)
        {
            case 0:
                break;

            case 1:
                reRoadCheck = 1;
                FadeManager.Instance.LoadScene("MainScene", 1);
                break;

            case 2:
                reRoadCheck = 2;
                FadeManager.Instance.LoadScene("MainScene", 1);
                break;

            case 3:
                reRoadCheck = 3;
                FadeManager.Instance.LoadScene("MainScene", 1);
                break;
        }
    }

    void ReStart(int select)
    {
        switch (select)
        {
            case 0:
                TimelineInit();
                CameraChange(0);
                break;

            case 1:
                reLoad = true;

                MainMenuManager.Instance.CanvasInit();

                TimelineInit();
                CameraChange(1);

                PlayTimeline(0);

                if (directors[0].state == PlayState.Playing)
                {
                    directors[0].time = 4;
                }

                PlayTimeline(1);
                break;

            case 2:
                reLoad = true;

                TimelineInit();
                CameraChange(0);

                MainMenuManager.Instance.CanvasInit();

                MainMenuManager.Instance.canvas[0].SetActive(true);
                EventSystem.current.SetSelectedGameObject(MainMenuManager.Instance.focusObject[0]);
                break;

            case 3:
                reLoad = true;

                TimelineInit();
                CameraChange(0);

                MainMenuManager.Instance.CanvasInit();

                MainMenuManager.Instance.canvas[1].SetActive(true);
                EventSystem.current.SetSelectedGameObject(MainMenuManager.Instance.focusObject[1]);
                break;
        }
    }

    public void TimeControl(float speedChange)
    {
        gameSpeed = speedChange;
        Time.timeScale = gameSpeed;
    }
}
