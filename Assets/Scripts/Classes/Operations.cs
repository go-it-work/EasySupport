using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operations : MonoBehaviour, IEnumerator, IEnumerable
{
    private readonly List<Operations> operationsList = new List<Operations>();
    public string MaintenanceOrderCode { get; set; }
    public string Descricao { get; set; }
    public string OperationCode { get; set; }
    public string Instruction { get; set; }
    public string Image { get; set; }
    public string PDF { get; set; }
    public int PDFName { get; set; }
    public string FBX { get; set; }
    public string Video { get; set; }
    public bool OCR { get; set; }
    public string OCRParameter { get; set; }
    public bool QRCode { get; set; }
    public string QRCodeParameter { get; set; }
    public bool Measure { get; set; }
    public string MeasureParameter { get; set; }
    public string Tools { get; set; }
    public string PPE { get; set; }
    public string Trainings { get; set; }
    public string Result { get; set; }
    public string Status { get; set; }

    public Operations()
    {
    }

    public Operations(string mo, string desc, string opCode, string instruction, bool ocr, bool qr, bool measure)
    {
        this.MaintenanceOrderCode = mo;
        this.Descricao = desc;
        this.OperationCode = opCode;
        this.Instruction = instruction;
        this.OCR = ocr;
        this.QRCode = qr;
        this.Measure = measure;
    }

    public Operations this[int index]
    {
        get => operationsList[index];
        set => operationsList.Add(value);
    }

    public int Size
    {
        get
        {
            var tam = operationsList.Count;
            return tam;
        }
    }

    public string[] Epis
    {
        get
        {
            var s = this.Epis.ToString().Split('/');
            return s;
        }
    }

    public object Current => throw new System.NotImplementedException();

    public bool MoveNext()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
