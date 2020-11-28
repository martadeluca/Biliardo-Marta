using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor;
using UnityEditor.Recorder.Input;

public class RecordScene : MonoBehaviour { 

    RecorderController TestRecorderController;
    RecorderControllerSettings controllerSettings;
    MovieRecorderSettings videoRecorder;

    public void StartRecord()
    {
        controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        TestRecorderController = new RecorderController(controllerSettings);

        videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "My Video Recorder";
        videoRecorder.Enabled = true;
        videoRecorder.VideoBitRateMode = VideoBitrateMode.High;
        videoRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        videoRecorder.AudioInputSettings.PreserveAudio = false;

        string str = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
        videoRecorder.OutputFile = "Registrazione_" + str;

        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 30;

        RecorderOptions.VerboseMode = false;
        TestRecorderController.PrepareRecording();
        TestRecorderController.StartRecording();
    }

    public void StopRecord()
    {
        TestRecorderController.StopRecording();
        controllerSettings.RemoveRecorder(videoRecorder);
    }
}