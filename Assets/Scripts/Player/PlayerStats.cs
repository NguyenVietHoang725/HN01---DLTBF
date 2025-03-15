using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxSpeed = 5f; // Tốc độ tối đa
    public float acceleration = 10f; // Gia tốc
    public float deceleration = 15f; // Giảm tốc
    public float friction = 3f; // Hệ số ma sát
    public float jumpForce = 5f; // Lực nhảy cơ bản
    public float jumpTime = 0.2f; // Thời gian giữ nút Jump để nhảy cao hơn
}