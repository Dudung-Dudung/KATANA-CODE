using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

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

    void Start()
    {
        initialPosition = transform.position;
        timingManager = FindObjectOfType<TimingManager>();
 /*       Debug.Log(this.gameObject.tag);*/
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


    IEnumerator SwingStickLt()
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube_lt") && gameObject.tag == "Stick_Red")
        {
            /*            Rigidbody cubeRb = other.gameObject.GetComponent<Rigidbody>();*/
            timingManager.CheckTiming();
            bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

            if (hasHit)
            {
                GameObject target = hit.transform.gameObject;
                Slice(target);
                Destroy(other.gameObject);
            }

            else if (!hasHit)
            {
                Vector3 startRaycast = startSlicePoint.position;
                Vector3 endRaycast = endSlicePoint.position;
/*                startRaycast.y -= 1f; // 시작점을 큐브의 경계에 더 가깝게 조정 (y 방향으로 -1)
                endRaycast.x += 1f;   // 끝점을 큐브의 경계에 더 가깝게 조정 (x 방향으로 +1)*/
                hasHit = Physics.Linecast(startRaycast, endRaycast, out RaycastHit hitt, sliceableLayer);

                if (hasHit)
                {
                    GameObject target = hit.transform.gameObject;
                    Slice(target);
                    Destroy(other.gameObject);
                }
            }

        }
        else if (other.gameObject.CompareTag("Cube_rt") && gameObject.tag == "Stick_Blue")
        {
            /*            Rigidbody cubeRb = other.gameObject.GetComponent<Rigidbody>();*/
            timingManager.CheckTiming();
            bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

            if (hasHit)
            {
                GameObject target = hit.transform.gameObject;
                Slice(target);
                Destroy(other.gameObject);
            }

            else if (!hasHit)
            {
                Vector3 startRaycast = startSlicePoint.position;
                Vector3 endRaycast = endSlicePoint.position;
                startRaycast.y -= 1f; // 시작점을 큐브의 경계에 더 가깝게 조정 (y 방향으로 -1)
                endRaycast.x += 1f;   // 끝점을 큐브의 경계에 더 가깝게 조정 (x 방향으로 +1)
                hasHit = Physics.Linecast(startRaycast, endRaycast, out RaycastHit hitt, sliceableLayer);

                if (hasHit)
                {
                    GameObject target = hit.transform.gameObject;
                    Slice(target);
                    Destroy(other.gameObject);
                }
            }

        }
    }

    public void Slice(GameObject target)
    {
        Debug.Log(target.name + " 슬라이스");
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

        else
        {
            Debug.Log("안짤렷음");
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


}
