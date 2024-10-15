using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GaugeManager : MonoBehaviour
{
    //Main
    GameManager gameManager;
    RayCastManager rayCast;

    public GameObject[] activeCanvas;

    private int gameNum = 0; 

    public float onceLap = 1.0f;
    public float timelineDelay = 3.0f;

    float fireTimer = 0.0f;
    float heartTimer = 0.0f;

    float result = 0.0f;
    float delayTimer = 0.0f;

    public PlayableDirector[] directors;
    public GameObject[] timelines;

    bool gameOne, gameTwo;
    bool mainGame;
    bool count;

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
        
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        rayCast = GetComponent<RayCastManager>();

        handXPos = new Vector3(-startX, -startY, 0);
        fireXPos = new Vector3(startX, startY, 0);

        imageWidth = heartGauge.rectTransform.sizeDelta.x;
        imageHeight = heartGauge.rectTransform.sizeDelta.y;

        mainGame = true;
        gameOne = true;
    }
   
    void Update()
    {
        width = Mathf.Clamp(width, 0, imageWidth);
        height = Mathf.Clamp(width, 0, imageHeight);

        if(gameNum == 0)
        {
            gameManager.ActiveChange(1);
            activeCanvas[1].SetActive(false);
            activeCanvas[0].SetActive(true);
        }
        else if(gameNum == 1)
        {
            gameManager.ActiveChange(0);
            activeCanvas[1].SetActive(true);
            activeCanvas[0].SetActive(false);
        }
        
        if (mainGame)
        {
            GameStop();

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
        }
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
            Debug.Log(heartTimer);
            width = 0.0f;
            height = 0.0f;

            heartGauge.rectTransform.sizeDelta = new Vector2(width, height);

            heartTimer = 0.0f;
        }
    }

    private void GameStop()
    {
        if (Input.GetButtonDown("Stop"))
        {
            if (gameNum == 0)
            {
                count = true;

                float distance = Mathf.Abs(fireImage.transform.position.x - handImage.transform.position.x) / 1200;

                result += 1 - distance;
                gameOne = false;

                timelines[0].SetActive(true);
                directors[0].Play();
            }
            else if(gameNum == 1)
            {
                count = true;

                timelines[1].SetActive(true);
                directors[1].Play();

                gameTwo = false;
                result += width / imageWidth;
            }
        }

        if (count && gameNum == 0)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= timelineDelay)
            {
                gameNum = 1;
                delayTimer = 0;
                gameTwo = true;
                count = false;
            }
        }
        else if (count && gameNum == 1)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= timelineDelay)
            {
                rayCast.laserSpeed = result * 100.0f;
                rayCast.BeamCharge();
                rayCast.ShotStart();
                //Debug.Log(result + " " + width + " " + height);
            }

        }
    }
}
