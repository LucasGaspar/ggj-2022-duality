using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Character : MonoBehaviour
{
    private const float RunSpeed = 2f;
    private const float JumpForce = 7.5f;
    private const float Gravity = -15f;
    private const float GroundedGravity = -1f;
    private const float MaxFallSpeed = -10f;
    private const string GroundLayerName = "Ground";
    private const float RaycastGroundedLength = 0.10f;
    private const float RaycastCeilingLength = 0.10f;
    private const float RaycastOffsetFromBorder = 0.1f;

    private new Rigidbody2D rigidbody2D = null;
    private new Collider2D collider2D = null;
    private float horizontalSpeed = 0f;
    private float verticalSpeed = 0f;
    private bool grounded = true;
    private int groundLayer = 0;

    private bool Rising { get => verticalSpeed > 0.25f; }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        groundLayer = 1 << LayerMask.NameToLayer(GroundLayerName);
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
        if (!grounded)
            return;

        verticalSpeed = JumpForce;
        grounded = false;
    }

    private void FixedUpdate()
    {
        CalculateGrounded();
        CalculateCeilingHit();
        if (grounded)
        {
            verticalSpeed = GroundedGravity;
        }
        else
        {
            verticalSpeed += Gravity * Time.fixedDeltaTime;
            verticalSpeed = Mathf.Max(verticalSpeed, MaxFallSpeed);
        }

        Vector2 newVelocity = new Vector2(horizontalSpeed, verticalSpeed);
        rigidbody2D.velocity = newVelocity;
    }

    private void CalculateGrounded()
    {
        if (Rising)
        {
            grounded = false;
        }
        else
        {
            //Test hit on lowest right
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x - RaycastOffsetFromBorder, collider2D.bounds.min.y), Vector2.down, RaycastGroundedLength, groundLayer);
            grounded = (hit.collider != null);
            if (!grounded)
            {
                //Test hit on lowest left
                hit = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x + RaycastOffsetFromBorder, collider2D.bounds.min.y), Vector2.down, RaycastGroundedLength, groundLayer);
                grounded = (hit.collider != null);
            }
        }
    }

    private void CalculateCeilingHit()
    {
        if (grounded)
            return;

        //Test hit on upper right
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x - RaycastOffsetFromBorder, collider2D.bounds.max.y), Vector2.up, RaycastCeilingLength, groundLayer);
        if (hit.collider == null)
        {
            //Test hit on upper left
            hit = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x + RaycastOffsetFromBorder, collider2D.bounds.max.y), Vector2.up, RaycastCeilingLength, groundLayer);
            if (hit.collider == null)
                return;
        }

        verticalSpeed = 0f;
    }
}
