using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private RectTransform joystickArea;

    [Header("Settings")]
    [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.1f;

    private Vector2 moveInput;
    private float radius;

    private void Awake()
    {
        radius = joystickOutline.sizeDelta.x * 0.5f;
        HideJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPosition);

        joystickOutline.anchoredPosition = localPosition;
        joystickOutline.gameObject.SetActive(true);

        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HideJoystick();
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickOutline,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPosition);

        Vector2 clampedPosition = Vector2.ClampMagnitude(localPosition, radius);
        joystickKnob.anchoredPosition = clampedPosition;

        moveInput = clampedPosition / radius;

        if (moveInput.magnitude < deadZone)
        {
            moveInput = Vector2.zero;
        }
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        joystickKnob.anchoredPosition = Vector2.zero;
        moveInput = Vector2.zero;
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}