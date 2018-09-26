// author: Stevie Giovanni

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUtilities
{

    public class DeviceTracker : MonoBehaviour
    {
        /// <summary>
        /// the device being tracked
        /// </summary>
        [SerializeField]
        private GameObject _device;

        /// <summary>
        /// name of the device to be tracked
        /// </summary>
        [SerializeField]
        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set { _deviceName = value; }
        }

        /// <summary>
        /// call track device coroutine to start tracking device
        /// </summary>
        public void TrackDevice()
        {
            StartCoroutine(TrackDeviceCoroutine());
        }

        /// <summary>
        /// track the device position and orientation
        /// </summary>
        public IEnumerator TrackDeviceCoroutine()
        {
            while (true)
            {
                // try to find the device if null
                if (_device == null)
                {
                    _device = GameObject.Find(DeviceName);
                    //if (_device == null)
                    //    Debug.LogWarning("No device found!");
                }

                // track the device if it exists
                if (_device != null)
                {
                    this.transform.SetPositionAndRotation(_device.transform.position, _device.transform.rotation);
                }

                yield return null;
            }
        }

        private void Start()
        {
            //TrackDevice();
        }
    }
}
