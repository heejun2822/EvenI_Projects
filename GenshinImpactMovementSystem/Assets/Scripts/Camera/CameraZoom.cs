using Unity.Cinemachine;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] private float defaultDistance = 6f;
        [SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;
        [SerializeField] [Range(0f, 10f)] private float maximumDistance = 6f;

        [SerializeField] [Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField] [Range(0f, 10f)] private float zoomSensitivity = 1f;

        private CinemachinePositionComposer positionComposer;
        private Player player;

        private float currentTargetDistance;

        private void Awake()
        {
            positionComposer = GetComponent<CinemachinePositionComposer>();
            player = FindFirstObjectByType<Player>();

            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            float zoomValue = player.Input.PlayerActions.Zoom.ReadValue<float>() * zoomSensitivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);

            float currentDistance = positionComposer.CameraDistance;

            if (currentDistance == currentTargetDistance)
            {
                return;
            }

            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

            positionComposer.CameraDistance = lerpedZoomValue;
        }
    }
}
