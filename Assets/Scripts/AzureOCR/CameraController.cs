using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    // UI RawImage we're applying the web cam texture to
    public RawImage cameraProjection;

    // texture which displays what our camera is seeing
    private WebCamTexture camTex;

    void Start ()
    {
        // create the camera texture
        camTex = new WebCamTexture(Screen.width, Screen.height);
        cameraProjection.texture = camTex;
        camTex.Play();
    }

    void Update ()
    {
        // click / touch input to take a picture
        //if (Input.GetMouseButtonDown(0))
        //    StartCoroutine(TakePicture());
        //else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        //    StartCoroutine(TakePicture());
    }

    // takes a picture and converts the data to a byte array
    // then triggers the AppManager.GetImageData method
    IEnumerator TakePicture ()
    {
        yield return new WaitForEndOfFrame();

        // create a new texture the size of the web cam texture
        Texture2D screenTex = new Texture2D(camTex.width, camTex.height);

        // read the pixels on the web cam texture and apply them
        screenTex.SetPixels(camTex.GetPixels());
        screenTex.Apply();

        // convert the texture to PNG, then get the data as a byte array
        byte[] byteData = screenTex.EncodeToPNG();

        // send the image data off to the Computer Vision API
        AppManager.instance.StartCoroutine("GetImageData", byteData);
        Debug.Log("Uploading Image");
    }

    public void TakePictureOCR()
    {
        Debug.Log("Picture taken");
        StartCoroutine(TakePicture());
    }
}