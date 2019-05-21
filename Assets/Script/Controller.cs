using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System;
using Input = GoogleARCore.InstantPreviewInput;

public class Controller : MonoBehaviour
{

    public Camera arcoreCamera;

    public GameObject Ball;
    private bool m_IsQuitting = false;
    public GameObject Wall;


    public int numberOfObjectsAllowed = 1;
    private int currentNumberOfObjects = 0;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        _UpdateApplicationLifecycle();
        //var planeLayer = GetComponent<>().planeLayer;
        //int layerMask = 1 << planeLayer;

        //Wall.SetActive(true);

        //face to camera
        //Vector3 cameraPosition = arcoreCamera.transform.position;
        //rotate the table to face the camera


        //Wall.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);


        ProcessTouches();
        Debug.Log("Unity Update Method");
    }

    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// Actually quit the application.
    /// 
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    //detect and select a plan tapped 
    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            /*select a plan tapped
            var detectedPlane = hit.Trackable as DetectedPlane;

            Ball.SetActive(true);

            //creata anchor
            Anchor anchor = detectedPlane.CreateAnchor(hit.Pose);
            Ball.transform.position = hit.Pose.position;
            Ball.transform.rotation = hit.Pose.rotation;



                    
            cameraPosition.y = hit.Pose.position.x; */


            var detectedPlane = hit.Trackable as DetectedPlane;
            //store the parent transform
            Transform rootTransform = arcoreCamera.transform.parent;
           //store hitposition for scaling
            Vector3 cameraPosition = hit.Pose.position;
            if (rootTransform != null) {
                //Scale the hitPosition by the scale of the root transform
                cameraPosition.Scale(rootTransform.transform.localScale);
                //Position the root transform by negative the hitPosition
                rootTransform.localPosition = cameraPosition * -1;
            }

            /*if (currentNumberOfObjects < numberOfObjectsAllowed)
            {


                currentNumberOfObjects = currentNumberOfObjects + 1;*/
                //var objectBall = Instantiate(Ball, hit.Pose.position, hit.Pose.rotation);
               
                // Andy should look at the camera but still be flush with the plane.
                if ((hit.Flags & TrackableHitFlags.PlaneWithinPolygon) != TrackableHitFlags.None)
                {
                    // Get the camera position and match the y-component with the hit position.

                    // Have Andy look toward the camera respecting his "up" perspective, which may be from ceiling.
                    //objectBall.transform.LookAt(cameraPositionSameY, objectBall.transform.up);
                    //creata anchor
                    Wall.transform.LookAt(cameraPosition, Wall.transform.up);

                    Wall.transform.position = hit.Pose.position;
                    Wall.transform.rotation = hit.Pose.rotation;
                    //Wall.transform.localScale = hi

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    Anchor anchor = detectedPlane.CreateAnchor(hit.Pose);
                    // Make Andy model a child of the anchor.
                    Wall.transform.parent = anchor.transform;
                }




            
        }
    }
}


