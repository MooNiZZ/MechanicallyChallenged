using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DropThroughPlatform _dropThroughtPlatform;
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Collider _collider;
    public float MoveSpeed;
    public float JumpForce;
    private float _lastJumpTime = float.MinValue;
    public float JumpColdown;
    private static RaycastHit[] CheckGroundResult = new RaycastHit[4];

    public Transform GroundChecker;

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

        _body.velocity = new Vector3(x, _body.velocity.y, _body.velocity.z);
    }

    private void Jump()
    {

        if(Input.GetKey(JumpKey) && IsReadyForJump())
        {
            _body.velocity = new Vector3(_body.velocity.x, JumpForce, _body.velocity.z);
            _lastJumpTime = Time.time;
        }
    }

    public bool IsGrounded()
    {
        var hits = Physics.RaycastNonAlloc(GroundChecker.position, Vector2.down, CheckGroundResult, CheckGroundDistance, GroundLayers);

        for(var i = 0; i < hits; i++)
        {
            if(CheckGroundResult[i].transform != transform)
            {
                if(!Physics.GetIgnoreCollision(_collider, CheckGroundResult[i].collider))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsReadyForJump()
    {
        return _lastJumpTime + JumpColdown < Time.time && IsGrounded();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GroundChecker.position, GroundChecker.position + Vector3.down * CheckGroundDistance);
    }
}
