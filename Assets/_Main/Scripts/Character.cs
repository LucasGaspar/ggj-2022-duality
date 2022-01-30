using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    private const float RunSpeed = 3f;
    private const float JumpForce = 7.5f;
    private const float Gravity = -15f;
    private const float GroundedGravity = 0f;
    private const float MaxFallSpeed = -10f;

    private new Rigidbody2D rigidbody2D = null;
    private float horizontalSpeed = 0f;
    private float verticalSpeed = 0f;
    private bool grounded = true;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveLeft()
    {
        horizontalSpeed = -1f * RunSpeed;
    }

    public void MoveRight()
    {
        horizontalSpeed = 1f * RunSpeed;
    }

    public void Stop()
    {
        horizontalSpeed = 0;
    }

    public void Jump()
    {
        verticalSpeed = JumpForce;
    }

    private void FixedUpdate()
    {
        grounded = transform.position.y <= 0 && verticalSpeed <= 0;
        Vector2 newVelocity = new Vector2();
        if (grounded)
        {
            verticalSpeed = GroundedGravity;
            newVelocity.y = verticalSpeed;
            newVelocity.x = horizontalSpeed;
        }
        else
        {
            verticalSpeed += Gravity * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Max(verticalSpeed, MaxFallSpeed);
            newVelocity.y = verticalSpeed;
            newVelocity.x = rigidbody2D.velocity.x;
        }

        rigidbody2D.velocity = newVelocity;
    }
}
