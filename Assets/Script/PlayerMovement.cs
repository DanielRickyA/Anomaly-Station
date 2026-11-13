using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float slowSpeed = 2.5f;
    [SerializeField] private InputActionReference moveActionRefrence;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private float currentSpeed;

    private bool isInPuddle = false;
    private float puddleTimer = 0f;
    private bool isLocked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (isLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.MovePosition(rb.position + moveInput.normalized * currentSpeed * Time.fixedDeltaTime);

    }

    private void Update()
    {
        if (isLocked) return;
        UpdateAnimationDirection();
        HandlePuddleTimer();
    }

    // Buat nyimpen arah dari input player
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (isLocked) return;
        moveInput = context.ReadValue<Vector2>();
    }

    // Ketika tidak ada input di keyboard jadi ya berhenti gk gerak wkwk
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        moveActionRefrence.action.Enable();
        moveActionRefrence.action.performed += OnMovePerformed;
        moveActionRefrence.action.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        moveActionRefrence.action.performed -= OnMovePerformed;
        moveActionRefrence.action.canceled -= OnMoveCanceled;
        moveActionRefrence.action.Disable();
    }

    private void UpdateAnimationDirection()
    {
        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("LastMoveX", moveInput.x);
            animator.SetFloat("LastMoveY", moveInput.y);

            if (moveInput.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput.x > 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puddle"))
        {
            isInPuddle = true;
            currentSpeed = slowSpeed;
            puddleTimer = 0f;
            currentSpeed = slowSpeed;
            Debug.Log("Player entered water. Speed reduced to: " + currentSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Puddle"))
        {
            isInPuddle = false;
            currentSpeed = slowSpeed;
            puddleTimer = 0f;
        }
    }

    private void HandlePuddleTimer()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (isInPuddle && isMoving)
        {

            puddleTimer += Time.deltaTime;
            if (puddleTimer >= 2f)
            {
                StartCoroutine(TriggerSlipRoutine());
                puddleTimer = 0f;
            }
        }
        else
        {
            puddleTimer = 0f;
        }

    }

    private IEnumerator TriggerSlipRoutine()
    {
        isLocked = true;

        moveInput = Vector2.zero;
        currentSpeed = moveSpeed;

        animator.SetBool("isSlip", true);
        animator.SetBool("isMoving", false);

        yield return new WaitForSeconds(1f);

        animator.SetBool("isSlip", false);
        isLocked = false;

        Vector2 currentKeyboardInput = moveActionRefrence.action.ReadValue<Vector2>();
        moveInput = currentKeyboardInput;

        if (isInPuddle)
        {
            currentSpeed = slowSpeed;
        }
    }

}
