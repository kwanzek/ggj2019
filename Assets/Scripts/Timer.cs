﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Timer : MonoBehaviour
{
    public int timeLeft;
    public Text textObject;

    void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
        timeLeft = 100;
    }

    void Update()
    {
        textObject.text = (timeLeft.ToString());
    }

    IEnumerator LoseTime()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    public int getTimerTime()
    {
        return timeLeft;
    }
}
