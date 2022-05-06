using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;
public class CodeScanner : MonoBehaviour
{
    WebCamTexture _webcamTexture;
    string _qrCode = string.Empty;
    [SerializeField] TextMeshProUGUI _tmp;

    void Start()
    {
        var renderer = GetComponent<RawImage>();
        _webcamTexture = new WebCamTexture(512, 512);
        renderer.material.mainTexture = _webcamTexture;
        StartCoroutine(GetQRCodeIE());
    }

    IEnumerator GetQRCodeIE()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        _webcamTexture.Play();
        var snap = new Texture2D(_webcamTexture.width, _webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(_qrCode))
        {
            try
            {
                if (Snap(snap, barCodeReader)) break;
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
    }

    bool Snap(Texture2D snap, IBarcodeReader barCodeReader)
    {
        snap.SetPixels32(_webcamTexture.GetPixels32());
        var Result = barCodeReader.Decode(snap.GetRawTextureData(), _webcamTexture.width, _webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
        if (Result != null)
        {
            _qrCode = Result.Text;
            if (!string.IsNullOrEmpty(_qrCode))
            {
                Debug.Log("Qr Decode: " + _qrCode);
                if (_tmp) _tmp.text = _qrCode; else Debug.LogError("Setar TextMeshproGui");
                return true;
            }
        }
        return false;
    }
}
