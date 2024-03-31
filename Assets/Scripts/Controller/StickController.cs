using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class StickController : MonoBehaviour
{
    public float stickSpeed = 3f;


    private Vector3 initialPosition;
    private bool isSwingingLt = false;
    private bool isSwingingRt = false;

    float maxDistance = 5f;

    private float targetAngle = -135f;
    private float swingTime = 0.3f;
    private float progress = 0f;

    public float score = 0;

    TimingManager timingManager;

    // 메쉬 자르기용 
    public Transform planeDebug;
    public GameObject target;
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;

    public Material crossSectionMaterial;
    public float cutForce = 2000;

    public LayerMask sliceableLayer;

    public AudioClip soundEffect;
    public ParticleSystem particle;

    private XRController xr;

    //컨트롤러 진동
    public float vibrationStrength = 0.5f;
    public float vibrationDuration = 0.1f;



    void Start()
    {
        initialPosition = transform.position;
        timingManager = FindObjectOfType<TimingManager>();
        /*       Debug.Log(this.gameObject.tag);*/

        Debug.Log(OVRInput.GetConnectedControllers()+ " 디바이스"); // 연결된 컨트롤러 확인
/*        xr = (XRController)GameObject.FindObjectOfType(typeof(XRController));*/

    }


    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.A) && !isSwingingLt && gameObject.tag == "Stick_Red")
        {
            targetAngle = -135f;
            StartCoroutine(SwingStickLt());
        }

        else if (Input.GetKeyDown(KeyCode.D) && !isSwingingRt && gameObject.tag == "Stick_Blue")
        {
            targetAngle = 135f;
            StartCoroutine(SwingStickRt());
        }*/
    }


/*    IEnumerator SwingStickLt()
    {
        isSwingingLt = true;
        progress = 0f;

        while (progress <= 1f)
        {
            progress += Time.deltaTime / swingTime * stickSpeed;

            if (progress <= 0.5f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRotation, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
            }

            yield return null;
        }

        isSwingingLt = false;
    }

    IEnumerator SwingStickRt()
    {
        isSwingingRt = true;
        progress = 0f;

        while (progress <= 1f)
        {
            progress += Time.deltaTime / swingTime * stickSpeed;

            if (progress <= 0.5f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRotation, progress * 2);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, targetAngle), targetRotation, (progress - 0.5f) * 2);
            }

            yield return null;
        }

        isSwingingRt = false;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        CubeState state = other.GetComponent<CubeState>();

        if (other.gameObject.CompareTag("Cube_lt") && gameObject.tag == "Stick_Red")
        {
            AudioSource.PlayClipAtPoint(soundEffect, gameObject.transform.position);

            particle.gameObject.transform.position = other.gameObject.transform.position;
            particle.Stop();
            particle.Play();
            if (state.isEnter == false)
            {
/*                VibrateController();*/
                state.isEnter = true;
                timingManager.CheckTiming();
                bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

                if (hasHit)
                {
                    GameObject target = hit.transform.gameObject;
                    Slice(target);
                }

                //판정 안고쳐져서 일단 주석
                /*else if (!hasHit)
                {
                    Vector3 startRaycast = startSlicePoint.position;
                    Vector3 endRaycast = endSlicePoint.position;
    *//*                startRaycast.y -= 1f; // 시작점을 큐브의 경계에 더 가깝게 조정 (y 방향으로 -1)
                    endRaycast.x += 1f;   // 끝점을 큐브의 경계에 더 가깝게 조정 (x 방향으로 +1)*//*
                    hasHit = Physics.Linecast(startRaycast, endRaycast, out RaycastHit hitt, sliceableLayer);

                    if (hasHit)
                    {
                        GameObject target = hit.transform.gameObject;
                        Slice(target);
                        Destroy(other.gameObject);
                    }
                }*/

                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.CompareTag("Cube_rt") && gameObject.tag == "Stick_Blue")
        {
            if(state.isEnter == false)
            {
 /*               VibrateController();*/
                particle.gameObject.transform.position = other.gameObject.transform.position;
                particle.Stop();
                particle.Play();

                state.isEnter = true;
                timingManager.CheckTiming();
                bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

                if (hasHit)
                {
                    GameObject target = hit.transform.gameObject;
                    Slice(target);
                }

                // 판정 안고쳐져서 일단 주석
                /*            else if (!hasHit)
                            {
                                Vector3 startRaycast = startSlicePoint.position;
                                Vector3 endRaycast = endSlicePoint.position;
                                *//*startRaycast.y -= 1f; // 시작점을 큐브의 경계에 더 가깝게 조정 (y 방향으로 -1)
                                endRaycast.x += 1f;   // 끝점을 큐브의 경계에 더 가깝게 조정 (x 방향으로 +1)*//*
                                hasHit = Physics.Linecast(startRaycast, endRaycast, out RaycastHit hitt, sliceableLayer);

                                if (hasHit)
                                {
                                    GameObject target = hit.transform.gameObject;
                                    Slice(target);
                                    Destroy(other.gameObject);
                                }
                            }*/

                Destroy(other.gameObject);
            }
        }
    }

    public void Slice(GameObject target)
    {


/*        Debug.Log(target.name + " 슬라이스");*/
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeDebug.up);

        if(hull != null)
        {

            GameObject uppderHull = hull.CreateUpperHull(target,crossSectionMaterial);
            SetupSliceComponent(uppderHull);

            GameObject lowerHull = hull.CreateLowerHull(target,crossSectionMaterial);
            SetupSliceComponent(lowerHull);
        }
    }

    public void SetupSliceComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        Destroy(slicedObject, 3f);
    }

    private void VibrateController()
    {
        // OVRInput.GetConnectedControllers()를 사용하여 현재 연결된 컨트롤러를 확인할 수도 있습니다.
        // 만약 왼쪽 컨트롤러만 진동을 주고 싶다면 다음과 같이 확인할 수 있습니다:
        // if(OVRInput.GetConnectedControllers() == OVRInput.Controller.LTouch)

        // 컨트롤러에 진동을 줍니다.
        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.Active);



        // 일정 시간 후 진동을 멈춥니다.
        Invoke("StopVibration", vibrationDuration);
    }

    // 진동을 멈추는 함수
    private void StopVibration()
    {
        // 컨트롤러의 진동을 멈춥니다.
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.Active);
    }


}
