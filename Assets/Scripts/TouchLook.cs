using UnityEngine;
using UnityEngine.EventSystems; // This is required for UI touching!

public class TouchLook : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public float lookX; // Sends rotation info to the camera
    [HideInInspector]
    public float lookY;

    public void OnDrag(PointerEventData eventData)
    {
        // Calculate how fast the finger is moving
        lookX = eventData.delta.x;
        lookY = eventData.delta.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop spinning when finger lifts up
        lookX = 0;
        lookY = 0;
    }
}