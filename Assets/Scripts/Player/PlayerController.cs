using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private PlayerInputHandler inputHandler; // Quản lý dữ liệu đầu vào
    private PlayerStats playerStats; // Quản lý chỉ số nhân vật

    private float currentSpeed = 0; // Vận tốc ban đầu
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveInput = inputHandler.MoveInput.x;
        float moveSpeed = playerStats.maxSpeed;
        float acceleration = playerStats.acceleration;
        float deceleration = playerStats.deceleration;
        float friction = playerStats.friction;
        
        // Nếu có input, tăng tốc
        if (moveInput != 0)
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, moveSpeed); // Tránh việc vận tốc tăng quá giới hạn maxSpeed
        }
        else
        {
            // Nếu không có input, thay vì giảm ngay, áp dụng ma sát
            currentSpeed *= (1 - friction * Time.fixedDeltaTime);
            if (currentSpeed < 0.1f) currentSpeed = 0; // Dừng hoàn toàn khi tốc độ quá nhỏ
        }
        
        // Áp dụng di chuyển
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }
    
}
