using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WebcamView : MonoBehaviour
{
    public static WebcamView instance;
    public static WebCamTexture _webcamTexture;
    [SerializeField] private RawImage rawImage;
    private Material webcam;

    public void Start()
    {
        try
        {
            // Creating the instance and getting the RawImage component
            instance = this;
            rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();

            // Starting the webcam texture
            _webcamTexture = new WebCamTexture(512, 512);
            _webcamTexture.Play();

            var renderer = rawImage.GetComponent<RawImage>();
            renderer.material.mainTexture = _webcamTexture;
            webcam.mainTexture = _webcamTexture;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void StartWebcam()
    {
        _webcamTexture.Play();
    }

    public static void StopWebcam()
    {
        _webcamTexture.Stop();
    }

    public static void PauseWebcam()
    {
        _webcamTexture.Pause();
    }

    public static Texture2D Convert_WebCamTexture_To_Texture2d(WebCamTexture _webCamTexture)
    {
        Texture2D _texture2D = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        _texture2D.SetPixels32(_webCamTexture.GetPixels32());
 
        return _texture2D;
    }

    public void TryAgain()
    {
        // Creating the instance and getting the RawImage component
        rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();

        // Starting the webcam texture
        _webcamTexture = new WebCamTexture(512, 512);
        _webcamTexture.Play();

        var renderer = rawImage.GetComponent<RawImage>();
        renderer.material.mainTexture = _webcamTexture;
    }
}
