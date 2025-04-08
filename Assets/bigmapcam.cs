using UnityEngine;

public class BigMapHoverPanner : MonoBehaviour
{
    public Camera mapCamera;
    public RectTransform mapUIArea; // The RawImage's RectTransform
    public float panSpeed = 50f;

    public Vector2 panLimitMin = new Vector2(-100f, -100f);
    public Vector2 panLimitMax = new Vector2(100f, 100f);

    private Rect screenRect;

    void Start()
    {
        // Convert the UI RectTransform to screen space once (or you can update it every frame if it resizes)
        screenRect = RectTransformToScreenSpace(mapUIArea);
    }

    void Update()
    {
        if (!ClickToLookWalkerWithMinimap.IsInteractingWithMap) return;

        Vector2 mousePos = Input.mousePosition;

        if (screenRect.Contains(mousePos))
        {
            float moveX = (mousePos.x - (screenRect.center.x)) / (screenRect.width / 2f);
            float moveY = (mousePos.y - (screenRect.center.y)) / (screenRect.height / 2f);

            Vector3 move = new Vector3(moveX, 0, moveY) * panSpeed * Time.deltaTime;

            Vector3 newPos = mapCamera.transform.position + move;
            newPos.x = Mathf.Clamp(newPos.x, panLimitMin.x, panLimitMax.x);
            newPos.z = Mathf.Clamp(newPos.z, panLimitMin.y, panLimitMax.y);

            mapCamera.transform.position = newPos;
        }
    }

    Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector3[] corners = new Vector3[4];
        transform.GetWorldCorners(corners);

        float x = corners[0].x;
        float y = corners[0].y;
        float width = corners[2].x - corners[0].x;
        float height = corners[2].y - corners[0].y;

        return new Rect(x, y, width, height);
    }
}
