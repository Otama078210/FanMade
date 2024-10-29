using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] GameObject[] canvas;

    [SerializeField] GameObject[] focusObject;

    GameObject currentFocus;
    GameObject previousFocus;

    [NonSerialized] public int[] gameCanNum = {};
    [NonSerialized] public int tutorialCanNum = 0;

    void Start()
    {
        if (!GameManager.Instance.reRoad)
        {
            CanvasInit();

            canvas[0].SetActive(true);

            EventSystem.current.SetSelectedGameObject(focusObject[0]);

            GameCanvasCheck();
        }
        else
        {
            GameCanvasCheck();
        }
    }

    void Update()
    {
        FocusCheck();
    }


    void GameCanvasCheck()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            if(canvas[i].name == "FireCanvas" || canvas[i].name == "HeartCanvas")
            {
                Array.Resize(ref gameCanNum, gameCanNum.Length + 1);
                gameCanNum[gameCanNum.Length - 1] = i;
            }
        }

        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].name == "Tutorial")
            {
                tutorialCanNum = i;
            }
        }
    }

    public void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    public void Transition_Menu(int nextMenu)
    {
        CanvasInit();

        canvas[nextMenu].SetActive(true);

        if (nextMenu != gameCanNum[0] && nextMenu != gameCanNum[1])
        {
            EventSystem.current.SetSelectedGameObject(focusObject[nextMenu]);
        }
    }

    void FocusCheck()
    {
        currentFocus = EventSystem.current.currentSelectedGameObject;

        if (currentFocus == previousFocus) return;

        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
