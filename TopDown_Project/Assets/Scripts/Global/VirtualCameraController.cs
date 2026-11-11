using Unity.Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    CinemachineCamera vcam;

    public int currentPriority = 5;
    public int activePriority = 20;

    private void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            vcam.Priority = activePriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            vcam.Priority = currentPriority;
        }
    }
}
