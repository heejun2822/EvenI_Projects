using System;
using Unity.Cinemachine;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerCameraUtility
    {
        [field: SerializeField] public CinemachineCamera Camera { get; private set; }
        [field: SerializeField] public float DefaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float DefaultHorizontalRecenteringTime { get; private set; } = 4f;

        private CinemachinePanTilt cinemachinePanTilt;

        public void Initialize()
        {
            cinemachinePanTilt = Camera.GetComponent<CinemachinePanTilt>();
        }

        public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
        {
            cinemachinePanTilt.PanAxis.Recentering.Enabled = true;

            cinemachinePanTilt.PanAxis.CancelRecentering();

            if (waitTime == -1f)
            {
                waitTime = DefaultHorizontalWaitTime;
            }

            if (recenteringTime == -1f)
            {
                recenteringTime = DefaultHorizontalRecenteringTime;
            }

            recenteringTime = recenteringTime * baseMovementSpeed / movementSpeed;

            cinemachinePanTilt.PanAxis.Recentering.Wait = waitTime;
            cinemachinePanTilt.PanAxis.Recentering.Time = recenteringTime;
        }

        public void DisableRecentering()
        {
            cinemachinePanTilt.PanAxis.Recentering.Enabled = false;
        }
    }
}
