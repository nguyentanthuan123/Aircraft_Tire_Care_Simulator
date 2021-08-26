using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    private InputDevice handInput;
    public GameObject handModelPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;
    Animator handAnimator;
    GameObject handModel;
    bool isPressed;
    bool isShowHand;
    // Start is called before the first frame update
    void Start()
    {
        isShowHand = false;
        isPressed = false;
        List<InputDevice> devices = new List<InputDevice>();
        //InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            handInput = devices[0];
        }
        handModel = Instantiate(handModelPrefab, transform);
        if (handModel)
        {
            handAnimator = handModel.GetComponent<Animator>();
            isShowHand = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowHand)
        {
            UpdateAnimation();
        }
        
        /*handInput.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);
        if (primaryValue && isPressed == false)
        {
            isPressed = true;
        }
        if(primaryValue == false && isPressed)
        {
            isPressed = false;
        }*/
    }
    void UpdateAnimation()
    {
        if (handInput.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("TriggerInput", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("TriggerInput", 0);
        }
        if (handInput.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("GripInput", gripValue);
        }
        else
        {
            handAnimator.SetFloat("GripInput", 0);
        }
    }
}
