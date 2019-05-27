//-----------------------------------------------------------------------
// <copyright file="AndyPlacementManipulator.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

    using GoogleARCore;
    using GoogleARCore.Examples.ObjectManipulation;
    using UnityEngine;

    /// <summary>
    /// Controls the placement of Andy objects via a tap gesture.
    /// </summary>
    public class ObjectPlacementManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        private GameObject prefab;
    public GameObject WallPrefab;

    public GameObject ballPrefab;

    public GameObject PlayerPrefab;

    public GameObject HolePrefab;


    public static int CurrentNumberOfGameObjects = 0;
    private int numberOfGameObjectsAllowed = 4;

    /// <summary>
    /// Manipulator prefab to attach placed objects to.
    /// </summary>
    public GameObject ManipulatorPrefab;

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }

    /// <summary>
    /// Function called when the manipulation is ended.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
        {
            return;
        }

        // If gesture is targeting an existing object we are done.
        if (gesture.TargetObject != null)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(
            gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
        {
            if (CurrentNumberOfGameObjects < numberOfGameObjectsAllowed)
            {// Use hit pose and camera pose to check if hittest is from the
             // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {

                    //MA put wall
                    if (CurrentNumberOfGameObjects == 0)
                    {

                        var wallObject = Instantiate(WallPrefab, hit.Pose.position, hit.Pose.rotation);
                        hit.Trackable.CreateAnchor(hit.Pose);
                        wallObject.tag = "Wall";
                        CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;
                        wallObject.AddComponent<BallController>();


                    }
                    else if (CurrentNumberOfGameObjects == 1)
                    {

                        var holeObject = Instantiate(HolePrefab, hit.Pose.position, hit.Pose.rotation);
                        hit.Trackable.CreateAnchor(hit.Pose);
                        HolePrefab.tag = "Hole";
                        CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;
                        holeObject.AddComponent<HoleController>();
                    }

                    //MA choose object to place
                    else if (CurrentNumberOfGameObjects == 2)
                    {

                        var ballObject = Instantiate(ballPrefab, hit.Pose.position, hit.Pose.rotation);
                        hit.Trackable.CreateAnchor(hit.Pose);
                        ballObject.tag = "Ball";
                        CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;


                    }
                    else if (CurrentNumberOfGameObjects == 3)
                    {
                       
                        // Instantiate Andy model at the hit pose.
                        var PlayerObject = Instantiate(PlayerPrefab, hit.Pose.position, hit.Pose.rotation);
                       
                        // Instantiate manipulator.
                        var manipulator =
                        Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                        // Make Andy model a child of the manipulator.
                        PlayerObject.transform.parent = manipulator.transform;




                        // Create an anchor to allow ARCore to track the hitpoint as understanding of
                        // the physical world evolves.
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make manipulator a child of the anchor.
                        manipulator.transform.parent = anchor.transform;

                        // Select the placed object.
                        manipulator.GetComponent<Manipulator>().Select();
                        PlayerObject.AddComponent<PlayerScript>();
                        CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;
                    }

                }
            }
        }
    }
}

