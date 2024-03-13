using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
public class InteractiveCanvas : MonoBehaviour
{
    public Canvas canvas;
    public GameObject Fade;
    private TrackedDeviceGraphicRaycaster trackedDeviceGraphicRaycaster;
    private Fade fade;
    private void Awake()
    {
        trackedDeviceGraphicRaycaster = canvas.GetComponent<TrackedDeviceGraphicRaycaster>();
        trackedDeviceGraphicRaycaster.enabled = false;
        fade = Fade.GetComponent<Fade>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateInteractiveCanvas", fade.fadeDurationTime + fade.fadeInTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateInteractiveCanvas()
    {
        trackedDeviceGraphicRaycaster.enabled = true;
    }

}
