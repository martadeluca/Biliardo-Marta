using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;
using System;
using Random = UnityEngine.Random;

public class TextManager : MonoBehaviour
{
    public Parameters parameters;

    public BiliardoManager biliardomanager;

    public ChangeParameters changeparameters;

    string url;

    public void Inizializzazione()
    {
        url = Application.absoluteURL;

        if (url.Length > 41)
        {
            ImportURL();
        }
        else
        {
            parameters.ballNumber = Random.Range(1, 10);
        }
    }

    public void ToText()
    {
        string json = JsonUtility.ToJson(parameters, true);
        File.WriteAllText("toText.json", json);
    }

    public void FromText(){
        string json = File.ReadAllText("toText.json");
        JsonUtility.FromJsonOverwrite(json, parameters);
        UpdateAllSliders();
    }

    public void UpdateAllSliders()
    {
        biliardomanager.sliderProf.value = parameters.depthValue;
        biliardomanager.sliderProsp.value = parameters.perspectiveValue;
        biliardomanager.sliderCS.value = parameters.chiaroscuroValue;
        biliardomanager.sliderShadow.value = parameters.shadowValue;
        biliardomanager.sliderDC.value = parameters.depthcueingValue;
        biliardomanager.sliderGravity.value = parameters.gravityValue;
        biliardomanager.sliderColor.value = parameters.colorValue;
    }

    public void ImportURL()
    {
        string[] querystrings = new string[2];

        querystrings = url.Split('?');

        querystrings[1] = querystrings[1].Replace(',', '.');

        int qslenght = querystrings[1].Split('&').Length - 1;

        string url2json = "{";

        for (int i = 0; i < qslenght; i++)
		{
           url2json = url2json + "\"" + querystrings[1].Split('&')[i].Split('=')[0] + "\"" + ":" + querystrings[1].Split('&')[i].Split('=')[1] + ", ";
        }

        url2json = url2json + "\"" + querystrings[1].Split('&')[qslenght].Split('=')[0] + "\"" + ":" + querystrings[1].Split('&')[qslenght].Split('=')[1] + "}";

        biliardomanager.canvas1.SetActive(false);
        biliardomanager.k = 2;

        JsonUtility.FromJsonOverwrite(url2json, parameters);
        UpdateAllSliders();
    }

    public void ExportURL()
    {
        string eurl = "http://biliardomarta.scienceontheweb.net/?";

        eurl = eurl + "ballNumber=" + parameters.ballNumber + "&";
        eurl = eurl + "ballSize=" + parameters.ballSize + "&";
        eurl = eurl + "ballSpeed=" + parameters.ballSpeed + "&";

        eurl = eurl + "depthValue=" + biliardomanager.sliderProf.value + "&";
        eurl = eurl + "perspectiveValue=" + biliardomanager.sliderProsp.value + "&";
        eurl = eurl + "chiaroscuroValue=" + biliardomanager.sliderCS.value + "&";
        eurl = eurl + "shadowValue=" + biliardomanager.sliderShadow.value + "&";
        eurl = eurl + "depthcueingValue=" + biliardomanager.sliderDC.value + "&";
        eurl = eurl + "gravityValue=" + biliardomanager.sliderGravity.value + "&";
        eurl = eurl + "colorValue=" + biliardomanager.sliderColor.value + "&";

        eurl = eurl + "randomSeed=" + parameters.randomSeed;

        GUIUtility.systemCopyBuffer = eurl;
    }

}
