using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GaugeManager : MonoBehaviour
{
    //Main
    static int levelkeep = 0;

    RayCastManager rayCast;

    private int gameNum = 0;

    float onceLap = 1.0f;

    float fireTimer = 0.0f;
    float heartTimer = 0.0f;

    float result = 0.0f;

    bool gameOne, gameTwo;
    bool shot;
    bool stop;

    //FireMove
    public GameObject effect;
    public GameObject fireImage;
    public GameObject handImage;

    public float startX, startY;
    Vector3 handXPos;
    Vector3 fireXPos;

    float loopTimer = 0.0f;

    bool loop;

    //HeartGame
    public Image heartGauge;

    float width = 0.0f;
    float height = 0.0f;

    float imageWidth = 0.0f;
    float imageHeight = 0.0f;

    //PowerGauge
    public Image powerGauge;
    float growUpTimer = 0.0f;
    float growUpFinTime = 0.5f;
    float resultPower = 0.0f;

    bool gaugeUpStart;

    void Start()
    {
        rayCast = GetComponent<RayCastManager>();

        handXPos = new Vector3(-startX, -startY, 0);
        fireXPos = new Vector3(startX, startY, 0);

        imageWidth = heartGauge.rectTransform.sizeDelta.x;
        imageHeight = heartGauge.rectTransform.sizeDelta.y;

        gameOne = true;

        if (GameManager.Instance.reLoad)
        {
            GameLevel(levelkeep);
        }
    }
   
    void Update()
    {
        width = Mathf.Clamp(width, 0, imageWidth);
        height = Mathf.Clamp(width, 0, imageHeight);
        
        if (GameManager.Instance.mainGame && !stop)
        {
            if (gameNum == 0)
            {
                if (gameOne)
                {
                    FireMove();
                }
            }
            else if (gameNum == 1)
            {
                if (gameTwo)
                {
                    HeartGauge();
                }
            }

            GameStop();
        }

        PowerGauge();
    }

    void FireMove()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer <= onceLap)
        {
            loopTimer += Time.deltaTime / onceLap;

            if (loop)
            {
                fireImage.transform.localPosition = Vector3.Lerp(-fireXPos, fireXPos, loopTimer);
                handImage.transform.localPosition = Vector3.Lerp(-handXPos, handXPos, loopTimer);
            }
            else if (!loop)
            {
                fireImage.transform.localPosition = Vector3.Lerp(fireXPos, -fireXPos, loopTimer);
                handImage.transform.localPosition = Vector3.Lerp(handXPos, -handXPos, loopTimer);
            }
        }
        else if(fireTimer > onceLap)
        {
            if(handImage.transform.localPosition.x >= 0)
            {
                loop = true;
            }
            else if (handImage.transform.localPosition.x <= 0)
            {
                loop = false;
            }

            fireTimer = 0;
            loopTimer = 0;
        }
    }

    void HeartGauge()
    {
        heartTimer += Time.deltaTime;

        if(heartTimer <= onceLap)
        {
            width += (imageWidth / onceLap) * Time.deltaTime;
            height += (imageHeight / onceLap) * Time.deltaTime;

            heartGauge.rectTransform.sizeDelta = new Vector2(width, height);
        }
        
        if(heartTimer > onceLap)
        {
            width = 0.0f;
            height = 0.0f;

            heartGauge.rectTransform.sizeDelta = new Vector2(width, height);

            heartTimer = 0.0f;
        }
    }

    void GameStop()
    {
        if (Input.GetButtonDown("Stop"))
        {
            if (gameNum == 0)
            {
                float distance = Mathf.Abs(fireImage.transform.position.x - handImage.transform.position.x) / 1200;

                if(distance > 1)
                {
                    distance = 1;
                }

                result += 1 - distance;

                resultPower = 1 - distance;
                gaugeUpStart = true;

                gameOne = false;

                GameManager.Instance.PlayTimeline(2);

                stop = true;
            }
            else if(gameNum == 1)
            {
                GameManager.Instance.PlayTimeline(3);

                gameTwo = false;
                result += width / imageWidth;
                Debug.Log(result);

                resultPower = width / imageWidth;
                gaugeUpStart = true;

                stop = true;
            }
        }
    }

    void PowerGauge()
    {
        if (gaugeUpStart)
        {
            growUpTimer += Time.deltaTime;

            if (growUpTimer <= growUpFinTime)
            {
                powerGauge.fillAmount += ((resultPower / growUpFinTime) / 2) * Time.deltaTime;
            }
            else
            {
                growUpTimer = 0.0f;
                gaugeUpStart = false;
            }
        }

        if (rayCast.countStart)
        {
            if (rayCast.timer <= rayCast.finTime)
            {
                powerGauge.fillAmount -= ((result / rayCast.finTime) / 2) * Time.deltaTime;
            }
        }
    }

    public void TimelineFinish()
    {
        if (gameNum == 0)
        {
            gameNum = 1;

            GameManager.Instance.CameraChange(2);
            MainMenuManager.Instance.Transition_Menu(MainMenuManager.Instance.gameCanNum[1]);

            stop = false;
            heartGauge.enabled = true;
            gameTwo = true;
        }
        else if (gameNum == 1 && !shot)
        {
            GameManager.Instance.PlayTimeline(4);

            rayCast.laserSpeed = result * 130.0f;

            shot = true;
        }
    }

    public void GameLevel(int level)
    {
        levelkeep = level;
        
        switch (levelkeep)
        {
            case 0:
                onceLap = 1.0f;
                rayCast.levelBonus = 1.0f;
                break;

            case 1:
                onceLap = 0.75f;
                rayCast.levelBonus = 1.25f;
                break;

            case 2:
                onceLap = 0.5f;
                rayCast.levelBonus = 1.5f;
                break;
        }
    }
}
