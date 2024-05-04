using UnityEngine;

namespace MechanicallyChallenged.Taffaz
{
    public class GroundChecker : MonoBehaviour
    {
        CapsuleCollider2D col;
        [SerializeField] float checkDistance = 0.05f;
        [SerializeField] LayerMask playerLayer;
        public bool isGrounded;

        public Transform Ground { get; private set; }
        public bool IsGrounded { get => isGrounded; }

        void Start()
        {
            col = GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            var hit = Physics2D.CircleCast(col.bounds.min, checkDistance, Vector2.down, checkDistance, ~playerLayer);

            if (hit.collider != null)
            {
                isGrounded = true;
                Ground = hit.transform;
                return;
            }

            isGrounded = false;
            Ground = null;
        }

    }
}