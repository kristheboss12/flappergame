using UnityEngine;

public class BigMapCameraController : MonoBehaviour
{
    public Camera mapCamera;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    public Vector2 panLimitsMin = new Vector2(-100, -100);
    public Vector2 panLimitsMax = new Vector2(100, 100);

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (!ClickToLookWalkerWithMinimap.IsInteractingWithMap) return;

        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newSize = mapCamera.orthographicSize - scroll * zoomSpeed;
            mapCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            delta *= mapCamera.orthographicSize / 200f; // Adjust speed by zoom level

            Vector3 newPos = mapCamera.transform.position - new Vector3(delta.x, 0, delta.y) * panSpeed * Time.deltaTime;

            newPos.x = Mathf.Clamp(newPos.x, panLimitsMin.x, panLimitsMax.x);
            newPos.z = Mathf.Clamp(newPos.z, panLimitsMin.y, panLimitsMax.y);

            mapCamera.transform.position = newPos;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
