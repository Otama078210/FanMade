using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] cameras;

    void Start()
    {
        ActiveChange(0);
    }

    void Update()
    {
        
    }

    public void ActiveChange(int cameraNum)
    {
        AllCamera();

        cameras[cameraNum].SetActive(true);
    }

    void AllCamera()
    {
        for(int i = 0 ; i < cameras.Length ; i++)
        {
            cameras[i].SetActive(false);
            Debug.Log(i);
        }
    }
}
