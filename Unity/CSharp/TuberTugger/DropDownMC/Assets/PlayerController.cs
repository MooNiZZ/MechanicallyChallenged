using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float overlapRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 0.3f;

    [SerializeField] private bool isGrounded;

    private BoxCollider2D boxCollider2D, 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (Input.GetAxis("Vertical") >= -0.5)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else
                TryDropThrough();
        }

        CheckAboveForPlatform();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, overlapRadius, groundLayer);
    }

    private void TryDropThrough()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, groundLayer);
        foreach (var collider in colliders)
            if (collider.CompareTag("Drop"))
                StartCoroutine(nameof(Drop), collider);
    }

    private IEnumerator Drop(Collider2D collider)
    {
        collider.TryGetComponent(out spriteRenderer renderer);
        if(renderer != null) renderer.Color = Color.green;
        
        Physics2D.IgnoreCollision(collider, boxCollider2D, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(collider, boxCollider2D, false);
        
        if(renderer != null) renderer.Color = Color.red;
    }

    private void CheckAboveForPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.3f), Vector2.up, raycastDistance);
        Debug.DrawRay(transform.position + new Vector3(0, 0.3f), Vector2.up * raycastDistance, hit.collider == null ? Color.red : Color.green);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Drop") && !hit.collider.isTrigger)
            StartCoroutine(nameof(Drop), hit.collider.gameObject.GetComponent<BoxCollider2D>());  
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
            Respawn();
    }

    private void Respawn()
    {
        transform.position = new(0, 1, 0);
        rb.velocity = Vector3.zero;
    }
}
