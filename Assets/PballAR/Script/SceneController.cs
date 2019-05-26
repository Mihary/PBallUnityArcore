using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;

public class SceneController : MonoBehaviour
{


    public Camera arcoreCamera;
    public GameObject DetectedPlanePrefab;
    public GameObject ARGround;
    private GameObject ARObject;
    public static int CurrentNumberOfGameObjects = 0;
    private int numberOfGameObjectsAllowed = 5;
    private bool m_IsQuitting = false;
    // Start is called before the first frame update

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _UpdateApplicationLifecycle();
        _SpawnARObject();
    }



    public void _SpawnARObject()
    {
        Touch touch;
        touch = Input.GetTouch(0);
        Debug.Log("touch count is " + Input.touchCount);
        TrackableHit hit;      // Raycast against the location the player touched to search for planes.
        //TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;// |
                                                                           

        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("Touch Began");

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                if (CurrentNumberOfGameObjects < numberOfGameObjectsAllowed)
                {
                    Debug.Log("Screen Touched");
                    //Destroy(ARObject);
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(arcoreCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {



                        ARObject = Instantiate(ARGround, hit.Pose.position, hit.Pose.rotation);// Instantiate Andy model at the hit pose.                                                                                 
                                                                                               //ARObject.transform.Rotate(-90, 0, 0, Space.Self);// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        ARObject.transform.LookAt(arcoreCamera.transform);
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        ARObject.transform.parent = anchor.transform;
                        CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;

                        // Hide Plane once ARObject is Instantiated 

                    }    //OnTogglePlanes(false);

                    //int layerMask = 1 << LayerMask.NameToLayer("Surfaces");
                }

            }

            //if  objet touché dans la surface ground,
            //add a hole and ball


        }

    }




    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
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

    /// <summary>
    /// Actually quit the application.
    /// </summary>
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




    //Added to stop plane detection  and keep the renderer the last surface detected
    //once gameobject instatntiated

    public void OnTogglePlanes(bool flag)
    {
       
        foreach (GameObject Temp in DetectedPLaneManager.instance.PLANES) //MA
        {
           
            DetectedPlanePrefab.SetActive(flag);
        }
    }

}
