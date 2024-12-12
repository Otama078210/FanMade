using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RayCastManager : MonoBehaviour
{
    [NonSerialized] public float levelBonus = 1.0f;

    public float original = 0.5f;
    public float length = 0.0f;

    [HideInInspector] public float laserSpeed = 0.0f;
    public float laserSize = 1.0f;

    float speedKeep = 0.0f;
    float sizeKeep = 0.0f;
    public float slowDownTime = 5.0f;
    public float finTime = 10.0f;

    float lengthResult = 0.0f;
    float enemyDestroy = 0.0f;

    Vector3 cameraOrigin;
    Vector3 laserOrigin;

    public GameObject laser;
    public GameObject chaseCamera;

    public float timer = 0.0f;

    public TextMeshProUGUI totalTX;
    public TextMeshProUGUI[] lengthTX;
    public TextMeshProUGUI destroyTX;

    [NonSerialized] public bool countStart;
    bool shotStart;

    void Start()
    {
        sizeKeep = laserSize;
        laserSize = 0.0f;
        totalTX.text = laserSpeed.ToString("000000");
        lengthTX[0].text = length.ToString("00000");
        lengthTX[1].text = length.ToString("0000.0");
        destroyTX.text = enemyDestroy.ToString("00000");

        cameraOrigin = chaseCamera.transform.position;
        laserOrigin = laser.transform.position;
    }

    void Update()
    {
        laserSize = Mathf.Clamp(laserSize, 0, sizeKeep);
        lengthResult = Mathf.Clamp(lengthResult, 0, 99999);
        enemyDestroy = Mathf.Clamp(enemyDestroy, 0, 99999);

        if (countStart)
        {
            LaserSlowDown();

            if (shotStart)
            {
                GrowRay();
            }
        }
    }

    private void GrowRay()
    {
        length += Time.deltaTime * laserSpeed;

        if(timer <= 1)
        {
            laserSize += Time.deltaTime * sizeKeep * 2;
        }

        laser.transform.position = new Vector3(length, laserOrigin.y, laserOrigin.z);
        laser.transform.localScale = new Vector3(laserSize, laserSize, laserSize);

        chaseCamera.transform.position = new Vector3(length, cameraOrigin.y, cameraOrigin.z);

        Vector3 rayOrigin = new Vector3(original, 0, 0);
        Vector3 rayLength = new Vector3(length, 0, 0);

        var ray = new Ray(rayOrigin, rayLength);

        lengthResult = length * 100 * levelBonus;

        lengthTX[0].text = lengthResult.ToString("00000");
        lengthTX[1].text = length.ToString("0000.0");

        Debug.DrawRay(ray.origin, rayLength, Color.red, 10.0f, false);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit ,length)) 
        {
            string tag = hit.collider.gameObject.tag; 
            Debug.Log(tag);

            if(tag == "Enemy")
            {
                Vector3 generatePos = hit.collider.gameObject.transform.position;
                generatePos.y = 1;

                Instantiate(EffectManager.Instance.otherFX[0], generatePos, Quaternion.identity);

                enemyDestroy += 1000 * levelBonus;
                destroyTX.text = enemyDestroy.ToString("00000");

                Destroy(hit.collider.gameObject);
                Debug.Log(enemyDestroy);
            }

            if(tag == "SpeedChange")
            {
                GameManager.Instance.TimeControl(0);
            }
        }

        totalTX.text = (lengthResult + enemyDestroy).ToString("000000");
    }

    private void LaserSlowDown()
    {
        timer += Time.deltaTime;

        if(timer <= finTime)
        {
            shotStart = true;

            if(timer + slowDownTime >= finTime)
            {
                laserSize -= (sizeKeep / slowDownTime) * Time.deltaTime;
                laserSpeed -= (speedKeep / slowDownTime) * Time.deltaTime;
            }
        }
        else if(timer > finTime && shotStart)
        {
            shotStart = false;
            GameManager.Instance.GameFinish();
        }
    }

    public void ShotStart()
    {
        speedKeep = laserSpeed;
        countStart = true;
    }
}
