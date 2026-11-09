using UnityEngine;

public class PlayerController : BaseController
{
    private Camera cam;

    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
    }

    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);
        lookDirection = worldPos - (Vector2)transform.position;

        if (lookDirection.magnitude < 0.9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }
}
