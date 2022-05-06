using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    public float x_Start, y_Start;
    public int ColumnLength;
    public int RowLength;
    public float x_Space, y_Space;

    private string[] strArray = new[] { "Boots", "Phones", "Mask", "Helmet" };

    void Start()
    {
        for (int i = 0; i < strArray.Length; i++)
        {
            var prefab = Resources.Load(strArray[i]) as GameObject;

            Vector3 position = new Vector3(x_Start + (x_Space * (i % ColumnLength)), y_Start + (-y_Space * (i / ColumnLength)), 0.5f);
            Instantiate(Resources.Load(strArray[i]), position, prefab.transform.rotation);
        }
    }

    public void InstantiateEpis()
    {
        foreach (var prefab in StartOrder.epiList[StartOrder.counter])
        {
            for (int i = 0; i < StartOrder.epiList[StartOrder.counter].Length; i++)
            {
                Vector3 position;
                position = new Vector3(x_Start + (x_Space * (i % ColumnLength)), y_Start + (-y_Space * (i / ColumnLength)));
                Instantiate(Resources.Load(prefab), position, Quaternion.identity);
            }
        }
    }
}
