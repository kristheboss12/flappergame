using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapToggleAndControl : MonoBehaviour
{
    [Header("Minimap UI")]
    public GameObject smallMinimap;
    public GameObject bigMinimap;

    [Header("Zoom Settings")]
    public RectTransform bigMapContent;
    public float zoomSpeed = 2f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;

    [Header("Pan Settings")]
    public float panSpeed = 1f;
    public Vector2 panLimitMin = new Vector2(-500, -500);
    public Vector2 panLimitMax = new Vector2(500, 500);

    private bool isBigMap = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isBigMap = !isBigMap;
            bigMinimap.SetActive(isBigMap);
            smallMinimap.SetActive(!isBigMap);

            Cursor.lockState = isBigMap ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isBigMap;

            ClickToLookWalkerWithMinimap.IsInteractingWithMap = isBigMap;
        }

        if (isBigMap && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                ClickToLookWalkerWithMinimap.IsInteractingWithMap = true;
            }
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 scale = bigMapContent.localScale;
            scale += Vector3.one * scroll * zoomSpeed;
            scale = ClampScale(scale);
            bigMapContent.localScale = scale;
        }
    }

    Vector3 ClampScale(Vector3 scale)
    {
        scale.x = Mathf.Clamp(scale.x, minZoom, maxZoom);
        scale.y = Mathf.Clamp(scale.y, minZoom, maxZoom);
        scale.z = 1f; // Don't scale z
        return scale;
    }

    void HandlePanning()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 newPos = bigMapContent.localPosition + new Vector3(delta.x, delta.y, 0) * panSpeed * Time.deltaTime;

            // Clamp movement within limits
            newPos.x = Mathf.Clamp(newPos.x, panLimitMin.x, panLimitMax.x);
            newPos.y = Mathf.Clamp(newPos.y, panLimitMin.y, panLimitMax.y);
            bigMapContent.localPosition = newPos;

            lastMousePosition = Input.mousePosition;
        }
    }
}
