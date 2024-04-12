using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private static RaycastHit2D[] CheckGroundResult = new RaycastHit2D[8];

    [SerializeField] private DropThroughPlatform2D _dropThroughtPlatform;
    [SerializeField] private Rigidbody2D _body;
    public float MoveSpeed;
    public float JumpForce;
    private float _lastJumpTime = float.MinValue;
    public float JumpColdown;

    public LayerMask GroundLayers;
    public float CheckGroundDistance;

    public KeyCode JumpKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode DropKey;


    private void Update()
    {
        Move();
        Drop();
        Jump();
    }

    private void Drop()
    {   
        if(Input.GetKeyDown(DropKey))
        {
            _dropThroughtPlatform.StartDrop();
        }

        if(Input.GetKeyUp(DropKey))
        {
            _dropThroughtPlatform.StopDrop();
        }
    }

    private void Move()
    {
        float x = 0;

        if(Input.GetKey(LeftKey))
        {
            x -= MoveSpeed;
        }

        if(Input.GetKey(RightKey))
        {
            x += MoveSpeed;
        }

        _body.velocityX = x;

    }

    private void Jump()
    {
        if(Input.GetKey(JumpKey) && IsReadyForJump())
        {
		    _body.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            _lastJumpTime = Time.time;
        }
    }

    public bool IsGrounded()
    {
        var hits = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, CheckGroundResult, CheckGroundDistance, GroundLayers);

        for(var i = 0; i < hits; i++)
        {
            if(CheckGroundResult[i].transform != transform)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsReadyForJump()
    {
        return _lastJumpTime + JumpColdown < Time.time && IsGrounded();
    }
}
