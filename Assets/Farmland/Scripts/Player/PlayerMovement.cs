using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MobileJoystick joystick;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = 0f;

    private CharacterController characterController;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        
        Vector2 input = joystick.GetMoveInput();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
        Vector3 targetVelocity = inputDirection * maxSpeed;

        float currentRate = inputDirection.sqrMagnitude > 0.001f
            ? acceleration
            : deceleration;

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            currentRate * Time.deltaTime
        );

     
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 finalMovement = horizontalVelocity + Vector3.up * verticalVelocity;

        characterController.Move(finalMovement * Time.deltaTime);
        playerAnimator.SetPlayerMoveAnimation(finalMovement);

        // Turn the player toward their movement direction.
        if (horizontalVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}