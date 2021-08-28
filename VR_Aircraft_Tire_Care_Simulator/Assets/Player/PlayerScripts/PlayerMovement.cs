using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public XRNode inputSource;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float speedMovement;
    public float jumpStrength;
    private CharacterController characterController;
    private XRRig xrRig;
    private Vector2 axisInput;
    private Vector3 veclocity;
    private float groundDistance;
    private float gForce;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        gForce = -9.8f;
        groundDistance = 0.5f;
        characterController = GetComponent<CharacterController>();
        xrRig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);
        device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);
        if (primaryValue)
        {
            //Debug.Log("Abutton");
        }
    }
    private void FixedUpdate()
    {
        Quaternion camRotate = Quaternion.Euler(0, xrRig.cameraGameObject.transform.eulerAngles.y, 0);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded && veclocity.y<0)
        {
            veclocity.y = -2;
        }
        veclocity.y = gForce * Time.deltaTime;
        characterController.Move(veclocity);
        Vector3 direct = camRotate * new Vector3(axisInput.x, 0, axisInput.y);
        characterController.Move(direct * speedMovement * Time.deltaTime);
    }
    public void jump()
    {
        if (isGrounded)
        {
            Debug.Log("jump");
            veclocity.y = Mathf.Sqrt(jumpStrength * -2f * gForce);
            //veclocity.y = gForce * Time.deltaTime;
            characterController.Move(veclocity);
        }
    }
}
