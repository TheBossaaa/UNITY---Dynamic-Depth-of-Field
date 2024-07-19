using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FieldOfDepth : MonoBehaviour
{
    [Header("DEPTH OF FIELD FOR PLAYER CAMERA")]

    [Header("Assign the Depth of Field Specific Global Volume")]
    public VolumeProfile volumeProfile;

    DepthOfField depthOfField;

    [Header("Focus Distance Settings")]
    [Tooltip("Set the maximum distance you want the camera to focus")]
    public float maxFocusDistance;
    [Tooltip("Set the minimum distance you want the camera to focus")]
    public float minFocusDistance;
    [Tooltip("Set the speed for focus change")]
    public float speedFocusChange;
    [Tooltip("Set the amount to change")]
    public float changeDepthOfField;

    [Header("Raycast Variables")]
    [Tooltip("Variable to calculate distance between camera and raycast hit distance")]
    public float distanceBetween; //Global variable to store the Vector3.Distance variable to calculate distance between camera and raycast hit.distance
    [Tooltip("Set the layers that you want to include")]
    public LayerMask layerMask;
    [Tooltip("Set the maximum distance for the raycast in gizmos")]
    [SerializeField] float maxDistance = 100f;

    static float t = 0.0f; //starting value for the Lerp

    void Start()
    {

        if (volumeProfile.TryGet<DepthOfField>(out depthOfField))
        {
            //Debug.Log("Depth of Field Component Initiated");
            depthOfField.active = true;
            depthOfField.mode.overrideState = true;
            depthOfField.focalLength.overrideState = true;
            depthOfField.focusDistance.overrideState = true;
        }
    }

    void Update() //Update the raycast
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Raycast hit: " + hit.transform.name);

            //Calculate the distance between camera and the raycast hit
            distanceBetween = Vector3.Distance(Camera.main.transform.position, hit.point);

            depthOfField.focusDistance.value = distanceBetween;

        }
    }

    private void OnDrawGizmos() //Draw the gizmos line for visualisation
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask))
        {
            // Draw the raycast
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance);

            // Draw a sphere at the hit point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }

    }


}
