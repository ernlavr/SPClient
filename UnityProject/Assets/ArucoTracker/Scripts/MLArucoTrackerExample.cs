// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2019-present, Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Developer Agreement, located
// here: https://auth.magicleap.com/terms/developer
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

// Modifications by Ernests Lavrinovics AAU 2021
namespace MagicLeap
{
    using System.Collections.Generic;
    using MagicLeap.Core;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.XR.MagicLeap;
    using SP;

    public class MLArucoTrackerExample : MonoBehaviour {
        /// <summary>
        /// 
        /// </summary>
        public MLArucoTracker.Settings trackerSettings = MLArucoTracker.Settings.Create();
        /// <summary>
        /// Prefab of X-Y-Z axis for visualization purposes
        /// </summary>
        public GameObject MLArucoMarkerPrefab;

        public GameObject globalOrigin;

        /// <summary>
        /// Hash set that contains all
        /// </summary>
        private HashSet<int> _arucoMarkerIds = new HashSet<int>();

        // For enabling/disabling ArUco tracking... TODO Do we need this?
        private bool _triggerReleased = true;
        private MLInput.Controller controller = null;
        int lockCounter = 0;

        // TODO Implement a dictionary that holds marker

        void Start()
        {
#if PLATFORM_LUMIN
            MLArucoTracker.UpdateSettings(trackerSettings);
            MLArucoTracker.OnMarkerStatusChange += OnMarkerStatusChange;
            SetStatusText();
            EnableAruco();
#endif
            if (controller == null)
            {
                controller = MLInput.GetController(0);
                return;
            }
        }

        void Update()
        {
#if PLATFORM_LUMIN            
            //if (controller.TriggerValue >= 0.25f && !MLInputModuleBehavior.IsOverUI)
            //{
            //    if(_triggerReleased)
            //    {
            //        ToggleAruco();
            //        _triggerReleased = false;
            //    }
            //}
            //else
            //{
            //    _triggerReleased = true;
            //}
#endif
        }

        void OnApplicationPause(bool pause)
        {
#if PLATFORM_LUMIN
            if (pause)
            {
                DisableAruco();
            }
            else
            {
                if(MLPrivileges.RequestPrivilege(MLPrivileges.Id.CameraCapture).Result == MLResult.Code.PrivilegeGranted)
                {
                    EnableAruco();
                }
            }
#endif
        }

        void OnDestroy()
        {
#if PLATFORM_LUMIN
            if (MLArucoTracker.IsStarted)
            {
                MLArucoTracker.OnMarkerStatusChange -= OnMarkerStatusChange;
            }
#endif
        }

        private void ToggleAruco()
        {
            if(trackerSettings.Enabled)
            {
                DisableAruco();
            }
            else
            {
                EnableAruco();
            }
        }

        private void DisableAruco()
        {

            trackerSettings.Enabled = false;
#if PLATFORM_LUMIN
            MLArucoTracker.UpdateSettings(trackerSettings);
#endif
        }

        private void EnableAruco()
        {
            trackerSettings.Enabled = true;
#if PLATFORM_LUMIN
            MLArucoTracker.UpdateSettings(trackerSettings);
#endif
        }

        /// <summary>
        /// Prints current markers that are in view
        /// </summary>
        private void SetStatusText() {
            string output = "Detected markers: ";
            foreach (int markerId in _arucoMarkerIds)
            {
                output += string.Format("{0}; ", markerId);
            }
            print(output);
        }

        /// <summary>
        /// Called on a marker-by-marker basis when they move in or outside the view
        /// </summary>
        /// <param name="marker"></param>
        /// <param name="status"></param>
        private void OnMarkerStatusChange(MLArucoTracker.Marker marker, MLArucoTracker.Marker.TrackingStatus status)
        {
#if PLATFORM_LUMIN
            // When marker enters the view..
            if (status == MLArucoTracker.Marker.TrackingStatus.Tracked)
            {
                if (_arucoMarkerIds.Contains(marker.Id))
                {
                    return;
                }

                // Set the global origin to calibration marker's (#49) values
                if(marker.Id == 49 && lockCounter < 10) {

                    GameObject arucoMarker = Instantiate(MLArucoMarkerPrefab);
                    MLArucoTrackerBehavior arucoBehavior = arucoMarker.GetComponent<MLArucoTrackerBehavior>();
                    arucoBehavior.MarkerId = marker.Id;
                    arucoBehavior.MarkerDictionary = MLArucoTracker.TrackerSettings.Dictionary;
                    globalOrigin.transform.position = marker.Position;

                    GlobalOrigin.setTransform(globalOrigin.transform);
                    GlobalOrigin.setRot(marker.Rotation);
                    _arucoMarkerIds.Add(marker.Id);
                    print(lockCounter);
                    lockCounter += 1;

                }

                // Configure visualization of the markers and TrackingBehaviour for export
                // TODO Upon export make sure that positions are exported relative to the global position
            }
            // When marker exits the view remove from currently tracked dictionary
            else if(_arucoMarkerIds.Contains(marker.Id))
            {
                _arucoMarkerIds.Remove(marker.Id);
            }

            // Print currently tracked markers
            //SetStatusText();
#endif
        }
    }

}
