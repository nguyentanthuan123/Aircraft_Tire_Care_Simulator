using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    //public XRNode inputSource;
    public ActionBasedController controller;
    public GameObject handModelPrefab;
    public Material hightLightMaterial;
    public LayerMask grabableLayer;
    public LayerMask hookLayer;
    public float followSpeed;
    public float rotateSpeed;
    public float jointDistance;
    public float getObjDistance;
    public bool isShowHand;
    public InputDeviceCharacteristics controllerCharacteristics;

    private Animator handAnimator;
    private Transform followControllerTrans;
    private GameObject handModel;
    private GameObject objGrabbing;
    private GameObject objSelected;
    private FixedJoint jointHandToObj, jointObjToHand;
    private Rigidbody rigi;
    private Transform grabPoint;
    private Collider handCollider;
    //bool isPressed;
    private InputDevice handInput;
    private float twistingDelay;
    private bool isTwistable;
    private bool isGrabbing;
    private bool isSelected;
    private bool isUsingTool;
    private bool isTwistedOut = false;
    private string objSelectedName;
    [SerializeField] private Transform palm;
    [SerializeField] private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        isUsingTool = false;
        isTwistable = true;
        isGrabbing = false;
        isShowHand = false;
        //isPressed = false;
        List<InputDevice> devices = new List<InputDevice>();
        //InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            handInput = devices[0];
        }
        //spawn hand model
        if (handModel == null)
        {
            handModel = Instantiate(handModelPrefab, transform);
        }
        handAnimator = handModel.GetComponent<Animator>();
        isShowHand = true;

        //Find camera for raycast
        if(mainCam == null)
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        //Find controller
        String s = controllerCharacteristics.ToString();
        if(s == "Left")
        {
            controller = GameObject.FindGameObjectWithTag("LeftHandController").GetComponent<ActionBasedController>();
        }
        if(s == "Right")
        {
            controller = GameObject.FindGameObjectWithTag("RightHandController").GetComponent<ActionBasedController>();
        }
        
        //set rigibody
        followControllerTrans = controller.gameObject.transform;
        rigi = GetComponent<Rigidbody>();
        rigi.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigi.mass = 20;
        rigi.maxAngularVelocity = 20f;
        rigi.interpolation = RigidbodyInterpolation.Interpolate;
        //teleport hand to controller
        rigi.position = followControllerTrans.position;
        rigi.rotation = followControllerTrans.rotation;
        handModel.transform.position = this.transform.position;

        //find palm point
        palm = handModel.transform.GetChild(2).transform;

        handCollider = handModel.GetComponent<Collider>();
        if (!handCollider)
        {
            handCollider = handModel.GetComponentInChildren<Collider>();
        }

        //set input for action
        controller.hapticDeviceAction.action.started += TwistBtnInput;
        controller.hapticDeviceAction.action.started += UseTool;
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.started += HookHand;
        controller.selectAction.action.canceled += Drop;
        controller.activateAction.action.started += GrabByRaycast;
        controller.activateAction.action.canceled += Drop;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowHand)
        {
            UpdateAnimation();
        }
        if(controller == null)
        {
            Start();
        }

        FollowController();

        HightlightObj();

        //handInput.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);
        /*if (primaryValue && isPressed == false)
        {
            isPressed = true;
        }
        if(primaryValue == false && isPressed)
        {
            isPressed = false;
        }*/
    }
    private void TwistBtnInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isTwistable) return;

        if (objGrabbing == null) return;

        var twistScript = objGrabbing.GetComponent<TwistObj>();
        if (!twistScript) return;

        objGrabbing.GetComponent<Collider>().enabled = false;
        handCollider.enabled = false;

        twistingDelay = twistScript.delayTwistTime;

        // must be in order
        twistScript.GettingTwistOut();
        twistScript.GettingTwistIn();

        if(twistScript.isFinishedTwistOut && twistScript.twistRealtime<=0)
        {
            if (!isTwistedOut)
            {
                rigi.freezeRotation = false;
                if (controller != null)
                {
                    followControllerTrans = controller.transform;
                }
                isTwistedOut = true;
            }
            else
            {
                twistScript.MoveToPlacement();
                isTwistedOut = false;
            }
        }
        StartCoroutine(TwistObjDelay());
    }
    IEnumerator TwistObjDelay()
    {
        isTwistable = false;
        yield return new WaitForSeconds(twistingDelay);
        isTwistable = true;
    }
    private void HookHand(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isGrabbing) return;

        Collider[] hookCollider = Physics.OverlapSphere(palm.position, jointDistance, hookLayer);
        if (hookCollider.Length < 1) return;

        objGrabbing = hookCollider[0].transform.gameObject;
        var hookRigi = objGrabbing.GetComponent<Rigidbody>();
        StartCoroutine(AddJoint(hookCollider[0], hookRigi));

        rigi.freezeRotation = true;
        followControllerTrans = null;
    }
    private void UseTool(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isGrabbing) return;

        var toolScript = objGrabbing.GetComponent<UtimateTool>();
        if (!toolScript) return;

        if (!isUsingTool)
        {
            toolScript.ActivateTool();
            objGrabbing.GetComponent<Collider>().enabled = false;
            handCollider.enabled = false;
            isUsingTool = true;
        }
        else
        {
            toolScript.DeactivateTool();
            isUsingTool = false;
        }
        
    }
    private void HightlightObj()
    {
        //if (objGrabbing && isGrabbing) return;
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, getObjDistance))
        {
            if (hit.transform.gameObject.layer == 6)
            {
                if (!isSelected)
                {
                    ResetSelectedObj();
                    changeMaterial(hit);
                }
                else if(objSelectedName!= null)
                {
                    if(objSelectedName != hit.transform.gameObject.name)
                    {
                        ResetSelectedObj();
                        changeMaterial(hit);
                    }
                }
            }
            else
            {
                ResetSelectedObj();
                isSelected = false;
            }
        }
    }
    private void changeMaterial(RaycastHit hit)
    {
        var hightlightCompoment = hit.transform.GetComponent<HightLightSelected>();
        if (!hightlightCompoment) return;

        objSelected = hit.transform.gameObject;
        objSelectedName = hit.transform.gameObject.name;
        hightlightCompoment.isChanged = true;
        hit.transform.GetComponent<Renderer>().material = hightLightMaterial;
        isSelected = true;
    }

    private void ResetSelectedObj()
    {
        if(objSelected != null)
        {
            objSelected.transform.GetComponent<HightLightSelected>().isHightLight = false;
            objSelected.transform.GetComponent<HightLightSelected>().isChanged = false;
            objSelected = null;
        }
    }

    private void GrabByRaycast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (objGrabbing && isGrabbing) return;
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position,mainCam.transform.forward,out hit, getObjDistance))
        {
            if(hit.transform.gameObject.layer == 6)
            {
                var hitRigi = hit.collider.GetComponent<Rigidbody>();
                if (!hitRigi) return;
                objGrabbing = hit.transform.gameObject;
                hit.transform.position = this.transform.position;
                StartCoroutine(AddJoint(hit.collider,hitRigi));
            }
        }
    }

    private void FollowController()
    {
        if (followControllerTrans == null) return;

        var distance = Vector3.Distance(transform.position, followControllerTrans.position);
        rigi.velocity = (followControllerTrans.position - transform.position).normalized * (followSpeed * distance);
        //transform.position = Vector3.Lerp(transform.position, followControllerTrans.position, followSpeed * distance);

        var quaternion = followControllerTrans.rotation * Quaternion.Inverse(rigi.rotation);
        quaternion.ToAngleAxis(out float angle, out Vector3 axis);
        rigi.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }
    private void Grab(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isGrabbing && objGrabbing) return;

        //ResetPalm();
        Collider[] objGrabbingCollider = Physics.OverlapSphere(palm.position, jointDistance, grabableLayer);
        if (objGrabbingCollider.Length < 1) return;

        var targetGrabbing = objGrabbingCollider[0].transform.gameObject;
        var targetRigi = targetGrabbing.GetComponent<Rigidbody>();
        if (targetRigi == null)
        {
            targetRigi = targetGrabbing.GetComponentInParent<Rigidbody>();
            if(targetRigi == null)
            {
                return;
            }
            else
            {
                objGrabbing = targetRigi.gameObject;
            }
        }
        else
        {
            objGrabbing = targetRigi.gameObject;
        }

        StartCoroutine(AddJoint(objGrabbingCollider[0],targetRigi));
    }
    private void Drop(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        handCollider.enabled = true;
        rigi.freezeRotation = false;
        if(jointHandToObj != null)
        {
            Destroy(jointHandToObj);
        }
        if(jointObjToHand != null)
        {
            Destroy(jointObjToHand);
        }
        if(grabPoint != null)
        {
            Destroy(grabPoint.gameObject);
        }
        if(objGrabbing != null)
        {
            var objGrabRigi = objGrabbing.GetComponent<Rigidbody>();
            objGrabRigi.collisionDetectionMode = CollisionDetectionMode.Discrete;
            objGrabRigi.interpolation = RigidbodyInterpolation.None;
            objGrabbing.GetComponent<Collider>().enabled = true;
            objGrabbing = null;
        }
        if(controller != null)
        {
            followControllerTrans = controller.gameObject.transform;
        }
        isGrabbing = false;
    }

   /* private void ResetPalm()
    {
        palm = null;
        palm = handModel.transform.GetChild(2).transform;
    }*/

    private void UpdateAnimation()
    {
        //handInput = InputDevices.GetDeviceAtXRNode(inputSource);
        if (handInput.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            //Debug.Log("trigger");
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

    private IEnumerator AddJoint(Collider collider, Rigidbody rigidbody)
    {
        isGrabbing = true;

        //init grabPoint variable
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = objGrabbing.transform;

        //Move hand to grabpoint
        followControllerTrans = grabPoint;

        //Wait for hand get to grabPoint
        while(grabPoint != null &&Vector3.Distance(grabPoint.position,palm.position) > jointDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //Freeze hand & objGrab motion 
        rigi.velocity = Vector3.zero;
        rigi.angularVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        //set rigibody mode for objGrab
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        //Add joint to hand
        jointHandToObj = gameObject.AddComponent<FixedJoint>();
        jointHandToObj.connectedBody = rigidbody;
        jointHandToObj.breakForce = float.PositiveInfinity;
        jointHandToObj.breakTorque = float.PositiveInfinity;

        //set hand joint
        jointHandToObj.massScale = 1;
        jointHandToObj.connectedMassScale = 1;
        jointHandToObj.enableCollision = false;
        jointHandToObj.enablePreprocessing = false;

        //Add joint to objGrab
        if(objGrabbing!= null)
        {
            
            jointObjToHand = objGrabbing.AddComponent<FixedJoint>();
            jointObjToHand.connectedBody = rigi;
            jointObjToHand.breakForce = float.PositiveInfinity;
            jointObjToHand.breakTorque = float.PositiveInfinity;

            //set objGrab joint
            jointObjToHand.massScale = 1;
            jointObjToHand.connectedMassScale = 1;
            jointObjToHand.enableCollision = false;
            jointObjToHand.enablePreprocessing = false;

        }
        //follow controller
        if (controller != null)
        {
            followControllerTrans = controller.gameObject.transform;
        }
    }
}
