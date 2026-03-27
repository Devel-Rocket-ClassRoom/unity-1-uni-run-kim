using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;

    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isSlide = false;
    private bool isDead = false;

    private Rigidbody2D rigid;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    private GameObject coin;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        capsuleCollider.offset = new Vector2(0, -0.44f);
        capsuleCollider.size = new Vector2(0.57f, 0.69f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            rigid.linearVelocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }

        if (Input.GetButton("Fire2") && isGrounded)
        {
            isSlide = true;
            capsuleCollider.offset = new Vector2(0, -0.44f);
            capsuleCollider.size = new Vector2(0.57f, 0.69f);

        }
        else
        {
            isSlide = false;
            capsuleCollider.offset = new Vector2(0, -0.12f);
            capsuleCollider.size = new Vector2(0.82f, 1.34f);
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSlide", isSlide);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead") && !isDead)
        {
            Die();
        }

        if (collision.CompareTag("Coin"))
        {

            Destroy(collision.gameObject);
            //AddScore();
        }



        void Die()
        {
            animator.SetTrigger("Die");
            rigid.linearVelocity = Vector2.zero;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            isDead = true;
        }
    }
}
