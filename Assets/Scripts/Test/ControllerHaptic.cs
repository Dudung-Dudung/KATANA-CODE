using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ControllerHaptic : MonoBehaviour
{
    public GameObject box;
    public XRController rightController;
    public float vibrationDuration = 0.1f;
    public float vibrationAmplitude = 0.5f;
    private bool isVibrateRunning = false;
    private void OnEnable()
    {
        /*_InputAsset.Enable();
        _InputAsset.FindActionMap("XRI RightHand Interaction").FindAction("jump").performed += OnJump;*/
        
    }

    private IEnumerator VibrateController()
    {
        isVibrateRunning = true;
        // Trigger controller vibration
        rightController.SendHapticImpulse(vibrationAmplitude, vibrationDuration);

        // Wait for the specified duration
        yield return new WaitForSeconds(vibrationDuration);

        // Stop controller vibration
        rightController.SendHapticImpulse(0, 0);
        isVibrateRunning = false;
    }


    private void Update()
    {
        PressedPrimaryButton();
    }

    private void PressedPrimaryButton()
    {
        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Primary Button Pressed");
            if (!isVibrateRunning)
            {
                StartCoroutine(VibrateController());
            }
            
        }
    }
}

