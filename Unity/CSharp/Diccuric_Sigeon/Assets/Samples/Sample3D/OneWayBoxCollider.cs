using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class OneWayBoxCollider : MonoBehaviour
{
    [Tooltip("The direction that the other object should be coming from for entry.")]
    [SerializeField] 
    private Vector3 entryDirection = Vector3.up;
    [Tooltip("Should the entry direction be used as a local direction?")]
    [SerializeField] 
    private bool localDirection = false;
    [Tooltip("How large should the trigger be in comparison to the original collider? Make sure that the size of the trigger is enough for the body entered it before collide the collider")]
    [SerializeField] 
    private Vector3 triggerScale = Vector3.one * 1.25f;
    [Tooltip("The collision will activate only when the penetration depth of the intruder is smaller than this threshold.")]
    [SerializeField]
    private float penetrationDepthThreshold = 0.2f;
    private new BoxCollider collider = null;
    [Tooltip("The trigger that we add ourselves once the game starts up.")]
    private BoxCollider collisionCheckTrigger = null;

    public Vector3 PassthroughDirection => localDirection ? transform.TransformDirection(entryDirection.normalized) : entryDirection.normalized;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        CalculateTrigger();
        collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        TryIgnoreCollision(other);
    }
    
    private void CalculateTrigger()
    {
        var size = new Vector3(collider.bounds.size.x / transform.localScale.x,
                               collider.bounds.size.y / transform.localScale.y,
                               collider.bounds.size.z / transform.localScale.z);
        collisionCheckTrigger.size = Vector3.Scale(size, triggerScale);
    }

    public void TryIgnoreCollision(Collider other)
    {
        if(other.isTrigger) return;

        if (Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth))
        {
            float dot = Vector3.Dot(PassthroughDirection, collisionDirection);

            if (dot < 0)
            {
                if(penetrationDepth < penetrationDepthThreshold) 
                {
                    Physics.IgnoreCollision(collider, other, false);
                }
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var direction = PassthroughDirection;
        var arrowHeadLength = 0.3f;
        var arrowHeadAngle = 20f;
        var arrowSize = 1f;
        var angle = Quaternion.LookRotation(direction);
        var from = transform.position + direction * arrowSize;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction * arrowSize);
        Vector3 right = angle * Quaternion.Euler(0, 180+arrowHeadAngle,0) * new Vector3(0,0,1);
        Vector3 left = angle * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
        Vector3 forward = angle * Quaternion.Euler(180+arrowHeadAngle,0, 0) * new Vector3(0,0,1);
        Vector3 backward = angle * Quaternion.Euler(180-arrowHeadAngle,0, 0) * new Vector3(0,0,1);
        Gizmos.DrawRay(from , right * arrowHeadLength);
        Gizmos.DrawRay(from, left * arrowHeadLength);
        Gizmos.DrawRay(from, forward * arrowHeadLength);
        Gizmos.DrawRay(from, backward * arrowHeadLength);
    }
#endif
}