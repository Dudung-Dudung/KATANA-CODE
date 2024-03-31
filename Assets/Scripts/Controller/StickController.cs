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

    // �޽� �ڸ���� 
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

    //��Ʈ�ѷ� ����
    public float vibrationStrength = 0.5f;
    public float vibrationDuration = 0.1f;



    void Start()
    {
        initialPosition = transform.position;
        timingManager = FindObjectOfType<TimingManager>();
        /*       Debug.Log(this.gameObject.tag);*/

        Debug.Log(OVRInput.GetConnectedControllers()+ " ����̽�"); // ����� ��Ʈ�ѷ� Ȯ��
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

                //���� �Ȱ������� �ϴ� �ּ�
                /*else if (!hasHit)
                {
                    Vector3 startRaycast = startSlicePoint.position;
                    Vector3 endRaycast = endSlicePoint.position;
    *//*                startRaycast.y -= 1f; // �������� ť���� ��迡 �� ������ ���� (y �������� -1)
                    endRaycast.x += 1f;   // ������ ť���� ��迡 �� ������ ���� (x �������� +1)*//*
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

                // ���� �Ȱ������� �ϴ� �ּ�
                /*            else if (!hasHit)
                            {
                                Vector3 startRaycast = startSlicePoint.position;
                                Vector3 endRaycast = endSlicePoint.position;
                                *//*startRaycast.y -= 1f; // �������� ť���� ��迡 �� ������ ���� (y �������� -1)
                                endRaycast.x += 1f;   // ������ ť���� ��迡 �� ������ ���� (x �������� +1)*//*
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


/*        Debug.Log(target.name + " �����̽�");*/
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
        // OVRInput.GetConnectedControllers()�� ����Ͽ� ���� ����� ��Ʈ�ѷ��� Ȯ���� ���� �ֽ��ϴ�.
        // ���� ���� ��Ʈ�ѷ��� ������ �ְ� �ʹٸ� ������ ���� Ȯ���� �� �ֽ��ϴ�:
        // if(OVRInput.GetConnectedControllers() == OVRInput.Controller.LTouch)

        // ��Ʈ�ѷ��� ������ �ݴϴ�.
        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.Active);



        // ���� �ð� �� ������ ����ϴ�.
        Invoke("StopVibration", vibrationDuration);
    }

    // ������ ���ߴ� �Լ�
    private void StopVibration()
    {
        // ��Ʈ�ѷ��� ������ ����ϴ�.
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.Active);
    }


}
