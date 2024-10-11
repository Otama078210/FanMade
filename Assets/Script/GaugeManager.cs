using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GaugeManager : MonoBehaviour
{
    GameManager gameManager;
    RayCastManager rayCast;

    int miniGameNum = 0; 

    public float onceLap = 1.0f;

    public float timer = 0.0f;

    //SpiritFire
    public GameObject[] activeObject;
    public GameObject fireEffect;

    //HeartGauge
    public Image heartGauge;

    float width = 0.0f;
    float height = 0.0f;

    float imageWidth = 0.0f;
    float imageHeight = 0.0f;

    float result = 0.0f;

    bool gaugeActive;

    float tien = 0.0f;

    public PlayableDirector play;
    public GameObject aiueo;

    bool Count;
        
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        rayCast = GetComponent<RayCastManager>();

        imageWidth = heartGauge.rectTransform.sizeDelta.x;
        imageHeight = heartGauge.rectTransform.sizeDelta.y;

        gaugeActive = true;
    }
   
    void Update()
    {
        width = Mathf.Clamp(width, 0, imageWidth);
        height = Mathf.Clamp(width, 0, imageHeight);

        if(miniGameNum == 0)
        {
            SpiritFire();
        }
        else if (miniGameNum == 1)
        {
            GageStop();

            if (gaugeActive)
            {
                HeartGauge();
            }
        }
    }

    void SpiritFire()
    {
        fireEffect.transform.position = new Vector3(1, 0, 0);

        gameManager.ActiveChange(1);
        activeObject[0].SetActive(false);
        activeObject[1].SetActive(true);
    }

    void HeartGauge()
    {
        timer += Time.deltaTime;

        if(timer <= onceLap)
        {
            width += (imageWidth / onceLap) * Time.deltaTime;
            height += (imageHeight / onceLap) * Time.deltaTime;

            heartGauge.rectTransform.sizeDelta = new Vector2(width, height);
        }
        
        if(timer > onceLap)
        {
            Debug.Log(timer);
            width = 0.0f;
            height = 0.0f;

            heartGauge.rectTransform.sizeDelta = new Vector2(width, height);

            timer = 0.0f;
        }
    }

    private void GageStop()
    {
        if (Input.GetButtonDown("Stop"))
        {
            Count = true;

            aiueo.SetActive(true);
            play.Play();

            gaugeActive = false;
            result = (width / imageWidth) + (height / imageHeight);            
        }

        if (Count)
        {
            tien += Time.deltaTime;

            if (tien >= 3)
            {
                rayCast.laserSpeed = result * 100.0f;
                rayCast.BeamCharge();
                rayCast.ShotStart();
                //Debug.Log(result + " " + width + " " + height);
            }
        }
       
    }
}
