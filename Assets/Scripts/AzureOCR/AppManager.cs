using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System.IO;

public class AppManager : MonoBehaviour
{
    // Computer Vision subscription key
    public string subKey = "ab3b03019dce45baa92452ff50649eee";

    // Computer Vision API url
    public string url = "https://goitmain.cognitiveservices.azure.com/";

    // on-screen text which shows the text we've analyzed
    [SerializeField] public TextMeshPro uiText;

    // instance
    public static AppManager instance;

    void Awake()
    {
        // set the instance
        instance = this;

        uiText.text = "Diga OK para realizar a leitura do OCR";
    }

    // sends the image to the Computer Vision API and returns a JSON file
    public IEnumerator GetImageData(byte[] imageData)
    {
        string urlFree = url + "vision/v3.2/ocr";

        uiText.text = "Processando imagem ...";
        Debug.Log("Get Image Started");
        // create a new web request and set the method to POST
        UnityWebRequest webReq = new UnityWebRequest(urlFree);
        webReq.method = UnityWebRequest.kHttpVerbPOST;

        // create a download handler to receive the JSON file
        webReq.downloadHandler = new DownloadHandlerBuffer();

        // upload the image data
        webReq.uploadHandler = new UploadHandlerRaw(imageData);
        webReq.uploadHandler.contentType = "application/octet-stream";

        // set the header
        webReq.SetRequestHeader("Ocp-Apim-Subscription-Key", subKey);

        // send the content to the API and wait for a response
        yield return webReq.SendWebRequest();
        Debug.Log(webReq.downloadHandler.text);
        // convert the content string to a JSON file
        JSONNode jsonData = JSON.Parse(webReq.downloadHandler.text);

        // get just the text from the JSON file and display on-screen
        uiText.text = GetTextFromJSON(jsonData).ToString();
        // uiText.text = webReq.downloadHandler.text;

        // send the text to the text to speech API
        //TextToSpeech.instance.StartCoroutine("GetSpeech", imageText);

        if (webReq.downloadHandler.text.Contains(StartOrder.operationsList[StartOrder.counter].OCRParameter))
        {
            uiText.text = uiText.text + "\n\nO código foi encontrado";
            StartOrder.operationsList[StartOrder.counter].Result = uiText.text;
            StartOrder.operationsList[StartOrder.counter].Status = "Success";
        }
        else
        {
            uiText.text = uiText.text + "\n\nO código não foi encontrado";
            StartOrder.operationsList[StartOrder.counter].Result = uiText.text;
            StartOrder.operationsList[StartOrder.counter].Status = "Failure";
        }
    }

    // returns the text from the JSON data
    string GetTextFromJSON(JSONNode jsonData)
    {
        string text = "";
        JSONNode lines = jsonData["regions"][0]["lines"];

        // loop through each line
        foreach (JSONNode line in lines.Children)
        {
            // loop through each word in the line
            foreach (JSONNode word in line["words"].Children)
            {
                // add the text
                text += word["text"] + " ";
            }
        }

        return text;
    }

    public void ReadOCRImage()
    {
        StartCoroutine(UploadPNG());
    }

    IEnumerator UploadPNG()
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        Texture2D myTex = WebcamView.Convert_WebCamTexture_To_Texture2d(WebcamView._webcamTexture);
        myTex.ReadPixels(new Rect(0, 0, WebcamView._webcamTexture.width, WebcamView._webcamTexture.height), 0, 0);
        byte[] byt = myTex.EncodeToPNG();
        Object.Destroy(myTex);

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        StartCoroutine(GetImageData(byt));
    }
}