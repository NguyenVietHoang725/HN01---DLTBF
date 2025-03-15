using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private PlayerStats playerStats;
    private PlayerCollisionHandler collisionHandler;

    private float lastOnGroundTime = 0f; // Coyote Time
    private float jumpBufferTime = 0f;   // Jump Buffer
    private bool isJumping = false;      // Kiểm tra nhân vật có đang nhảy không
    private float jumpTimeCounter;       // Đếm thời gian giữ nút Jump

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerStats = GetComponent<PlayerStats>();
        collisionHandler = GetComponent<PlayerCollisionHandler>();
    }

    private void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        jumpBufferTime -= Time.deltaTime;

        // Nếu chạm đất, reset các trạng thái nhảy
        if (collisionHandler.IsGrounded)
        {
            lastOnGroundTime = 0.1f; // Coyote Time
            isJumping = false;
        }

        // Nếu người chơi nhấn Jump, lưu vào Jump Buffer
        if (inputHandler.IsHoldingJump)
        {
            jumpBufferTime = 0.1f;
        }

        // Thực hiện Jump nếu đủ điều kiện (Jump Buffer + Coyote Time)
        if (jumpBufferTime > 0 && lastOnGroundTime > 0)
        {
            Jump();
            jumpBufferTime = 0;
            lastOnGroundTime = 0;
        }

        // Nếu đang nhảy và giữ nút Jump, tiếp tục tăng lực
        if (isJumping && inputHandler.IsHoldingJump && jumpTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpForce * 1.2f);
            jumpTimeCounter -= Time.deltaTime;
        }

        // Nếu nhả nút Jump sớm, rơi nhanh hơn để giảm chiều cao
        if (!inputHandler.IsHoldingJump && isJumping && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveInput = inputHandler.MoveInput.x;
        float targetSpeed = moveInput * playerStats.maxSpeed;
        float acceleration = playerStats.acceleration;
        float friction = playerStats.friction;

        // Nếu ở trên không, giảm gia tốc và ma sát để di chuyển chậm hơn
        if (!collisionHandler.IsGrounded)
        {
            acceleration *= 0.5f;
            friction *= 0.8f;
        }

        // Điều chỉnh tốc độ
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime),
                rb.velocity.y
            );
        }
        else
        {
            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, 0, friction * Time.fixedDeltaTime),
                rb.velocity.y
            );
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpForce);
        isJumping = true;
        jumpTimeCounter = playerStats.jumpTime;
    }
}
