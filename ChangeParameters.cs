using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class ChangeParameters : MonoBehaviour
{
    public Parameters parameters;
    public BiliardoManager biliardomanager;

    public GameObject ballnumber;
    public GameObject ballsize;
    public GameObject ballspeed;
    public GameObject randomseed;

    public void NumberChange(string n)
    {
        parameters.ballNumber = int.Parse(n);
    }

    public void SizeChange(string n)
    {
        parameters.ballSize = float.Parse(n);
        biliardomanager.SliderProf(biliardomanager.sliderProf.value);
    }

    public void SpeedChange(string n)
    {
        parameters.ballSpeed = float.Parse(n);

        foreach(GameObject b in biliardomanager.ball)
          b.GetComponent<MyBall>().velocity = b.GetComponent<MyBall>().velocity.normalized * parameters.ballSpeed;
    }

    public void RandomSeedChange(string n)
    {
        parameters.randomSeed = int.Parse(n);
    }

    public void RandomFill()
    {
        parameters.ballNumber = Random.Range(1, 40);
        parameters.ballSize = Mathf.Round(Random.Range(5.0f, 50.0f) * 100f) / 100f;

        parameters.ballSpeed = Mathf.Round(Random.Range(30.0f, 100.0f) * 100f) / 100f;
        parameters.randomSeed = Random.Range(1, 200);

        biliardomanager.sliderProf.value = Random.Range(0.0f, 1.0f);
        biliardomanager.sliderProsp.value = Random.Range(0.0f, 1.0f);
        biliardomanager.sliderCS.value = Random.Range(0.0f, 1.0f);
        biliardomanager.sliderShadow.value = Random.Range(0.0f, 1.0f);
        biliardomanager.sliderDC.value = Random.Range(0.0f, 1.0f);

        biliardomanager.sliderGravity.value = Random.Range(0.0f, 1.0f);
        biliardomanager.sliderColor.value = Random.Range(0.0f, 1.0f);

        UpdateBox();
    }

    public void UpdateBox()
    {
        if (ballnumber.GetComponent<InputField>().text.Length > 0)
            ballnumber.GetComponent<InputField>().text = parameters.ballNumber.ToString();
        else
            ballnumber.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Number: " + parameters.ballNumber;

        if (ballsize.GetComponent<InputField>().text.Length > 0)
            ballsize.GetComponent<InputField>().text = parameters.ballSize.ToString();
        else
            ballsize.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Size: " + parameters.ballSize;

        if (ballspeed.GetComponent<InputField>().text.Length > 0)
            ballspeed.GetComponent<InputField>().text = parameters.ballSpeed.ToString();
        else
            ballspeed.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Speed: " + parameters.ballSpeed;

        if (randomseed.GetComponent<InputField>().text.Length > 0)
            randomseed.GetComponent<InputField>().text = parameters.randomSeed.ToString();
        else
            randomseed.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Rnd seed: " + parameters.randomSeed;
    }
}