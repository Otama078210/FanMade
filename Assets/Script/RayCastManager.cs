using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RayCastManager : MonoBehaviour
{
    public float original = 0.5f;
    public float length = 0.0f;

    public float laserSpeed = 0.0f;
    public float laserSize = 1.0f;

    float speedKeep = 0.0f;
    float sizeKeep = 0.0f;
    [SerializeField] private float slowDownTime = 5.0f;
    [SerializeField] private float finTime = 10.0f;

    int enemyDestroy = 0;

    Vector3 cameraOrigin;
    Vector3 laserOrigin;

    public GameObject laser;
    public GameObject chaseCamera;

    public float timer = 0.0f;

    public TextMeshProUGUI raySpeedTX;
    public TextMeshProUGUI enDestroyTX;

    bool mainGame;
    bool start;

    void Start()
    {
        speedKeep = laserSpeed;
        sizeKeep = laserSize;
        raySpeedTX.text = laserSpeed.ToString("000.0");
        enDestroyTX.text = enemyDestroy.ToString("000.0");

        cameraOrigin = chaseCamera.transform.position;
        laserOrigin = laser.transform.position;
    }

    void Update()
    {
        if (mainGame)
        {
            GameTimer();

            if (start)
            {
                GrowRay();
            }
        }
    }

    void GrowRay()
    {
        length += Time.deltaTime * laserSpeed;

        laser.transform.position = new Vector3(length / 2, laserOrigin.y, laserOrigin.z);
        laser.transform.localScale = new Vector3(laserSize, length / 2, laserSize);

        chaseCamera.transform.position = new Vector3(length, cameraOrigin.y, cameraOrigin.z);

        Vector3 rayOrigin = new Vector3(original, 0, 0);
        Vector3 rayLength = new Vector3(length, 0, 0);

        var ray = new Ray(rayOrigin, rayLength);

        Debug.DrawRay(ray.origin, rayLength, Color.red, 10.0f, false);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit ,length)) 
        {
            string tag = hit.collider.gameObject.tag; 
            Debug.Log(tag);

            if(tag == "Enemy")
            {
                enemyDestroy += 1;
                enDestroyTX.text = enemyDestroy.ToString("000.0");

                Destroy(hit.collider.gameObject);
                Debug.Log(enemyDestroy);
            }
        }
    }

    void GameTimer()
    {
        timer += Time.deltaTime;

        if(timer <= finTime)
        {
            start = true;

            if(timer + slowDownTime >= finTime)
            {
                laserSize -= (sizeKeep / slowDownTime) * Time.deltaTime;
                laserSpeed -= (speedKeep / slowDownTime) * Time.deltaTime;
                //Debug.Log(laserSpeed);
            }
        }
        else if(timer > finTime)
        {
            start = false;
        }
    }

    public void BeamCharge()
    {
        laserSpeed += 1.0f;
        raySpeedTX.text = laserSpeed.ToString("000.0");
    }

    public void ShotStart()
    {
        mainGame = true;
    }
}
