using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class ResourcesMenu : MonoBehaviour
{
    [SerializeField] private GameObject imageCanvas;
    [SerializeField] private GameObject pdfCanvas;
    [SerializeField] private GameObject epis;

    public void OpenImage()
    {
        imageCanvas.SetActive(true);
        pdfCanvas.SetActive(false);
        epis.SetActive(false);
    }

    public void OpenPDF()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(true);
        epis.SetActive(false);
    }

    public void OpenVideo()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
        epis.SetActive(false);
        Application.OpenURL(StartOrder.ActiveOperation[StartOrder.counter].Video);

        // The second option it's to download the video to MyVideos folder and access it after
        /*
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "test.mp4");
        var videoPlayer = GetComponent<VideoPlayer>();
 
        Uri u = new Uri(new Uri("file://"), path);
        videoPlayer.url = u.AbsoluteUri;
        */
    }

    public void OpenEpis()
    {
        imageCanvas.SetActive(false);
        pdfCanvas.SetActive(false);
        epis.SetActive(true);
    }
}
