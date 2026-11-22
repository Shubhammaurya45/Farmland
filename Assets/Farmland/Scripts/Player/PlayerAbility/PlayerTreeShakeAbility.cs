using UnityEngine;

public class PlayerTreeShakeAbility : MonoBehaviour
{
    [Header("Reference")]
    private Tree currentTree;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    [Header("Setting")]
    [SerializeField]
    private float distanceToTree = 1f;

    [Range(0f, 1f)]
    [SerializeField]
    private float shakeThreshold;
    private bool isActive = false;
    private Vector2 previousMousePosition;

    private void OnEnable()
    {
        TreeManager.enteredInTreeZone += InTreeZoneCallBack;
        TreeManager.exitTreeMode += ExitTreeModeCallBack;
    }

    private void OnDisable()
    {
        TreeManager.enteredInTreeZone -= InTreeZoneCallBack;
        TreeManager.exitTreeMode += ExitTreeModeCallBack;
    }

    private void Start()
    {
        characterController = GetComponent<PlayerMovement>().CharacterController;
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (isActive)
            ManageTreeShake();
    }

    private void InTreeZoneCallBack(Tree tree)
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        currentTree = tree;
        MoveTowardsTree();
        FaceTree();
        isActive = true;
    }

    private void ExitTreeModeCallBack()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;

        currentTree = null;
        isActive = false;

        playerAnimator.StopShakeTreeAnimation();
    }

    private void MoveTowardsTree()
    {
        if (currentTree == null)
            return;

        Vector3 treePosition = currentTree.transform.position;

        // Direction from the tree toward the player, flattened to the ground.
        Vector3 fromTreeToPlayer = transform.position - treePosition;
        fromTreeToPlayer.y = 0f;

        if (fromTreeToPlayer.sqrMagnitude < 0.001f)
            fromTreeToPlayer = -transform.forward;

        // Stop this far away from the tree.
        Vector3 targetPosition = treePosition + fromTreeToPlayer.normalized * distanceToTree;

        Vector3 moveDelta = targetPosition - transform.position;
        moveDelta.y = 0f;

        characterController.Move(moveDelta);
    }

    private void FaceTree()
    {
        if (currentTree == null)
            return;

        Vector3 lookDirection = currentTree.transform.position - transform.position;
        lookDirection.y = 0f; // Keep the player upright.

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private void ManageTreeShake()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            currentTree.StopShake();
            return;
        }

        float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);

        if (ShouldShake(shakeMagnitude))
            Shake();
        else
            currentTree.StopShake();

        previousMousePosition = Input.mousePosition;
    }

    private bool ShouldShake(float shakeMagnitude)
    {
        float screenPercent = shakeMagnitude / Screen.width;
        return shakeMagnitude > shakeThreshold;
    }

    private void Shake()
    {
        currentTree.Shake();
        playerAnimator.PlayShakeTreeAnimation();
    }
}
