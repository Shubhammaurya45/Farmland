using UnityEngine;

public class Mobile_Joystick : MonoBehaviour
{
    [Header("Joystick Details")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header("Joystick Settings")]
    [SerializeField] private float moveFactor = 1f;
    private Vector3 clickedPosition = new Vector3();
    private bool canControl;
    private Vector3 move;


    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {

        if (canControl)
        {
            ControlJoystick();
        }
    }

    public void ClickedOnJoystickZone()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;
        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
    }

    private void ControlJoystick()
    {
        var currentPosition = Input.mousePosition;
        var direction = currentPosition - clickedPosition;

        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);

         move = direction.normalized * moveMagnitude;

        Vector3 targetPosition = clickedPosition + move;

        joystickKnob.position = targetPosition;

        if (Input.GetMouseButtonUp(0)) HideJoystick();
    }

    public Vector3 GetMoveVector()
    {
        return move;
    }


}
