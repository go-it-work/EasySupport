using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class GetHeadPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHeadRay()
    {
        var headRay = InputRayUtils.GetHeadGazeRay();

        foreach(var controller in CoreServices.InputSystem.DetectedControllers)
        {
            // Interactions for a controller is the list of inputs that this controller exposes
            foreach(MixedRealityInteractionMapping inputMapping in controller.Interactions)
            {
                // 6DOF controllers support the "SpatialPointer" type (pointing direction)
                // or "GripPointer" type (direction of the 6DOF controller)
                if (inputMapping.InputType == DeviceInputType.SpatialPointer)
                {
                    Debug.Log("spatial pointer PositionData: " + inputMapping.PositionData);
                    Debug.Log("spatial pointer RotationData: " + inputMapping.RotationData);
                }

                if (inputMapping.InputType == DeviceInputType.SpatialGrip)
                {
                    Debug.Log("spatial grip PositionData: " + inputMapping.PositionData);
                    Debug.Log("spatial grip RotationData: " + inputMapping.RotationData);
                }
            }
        }
    }
}
