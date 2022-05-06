using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsDisplay : MonoBehaviour
{
    // 
    private void SetResultsOnScreen()
    {
        foreach(Operations op in StartOrder.operationsList)
        {
            Debug.Log(StartOrder.ActiveOrder.OrderCode);
            Debug.Log(op.Result);
        }
    }
}
