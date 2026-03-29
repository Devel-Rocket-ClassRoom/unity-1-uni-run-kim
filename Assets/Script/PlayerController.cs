using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    private bool isInvincible = false;
    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isSlide = false;
    private bool isDead = false;

    private Rigidbody2D rigid;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 죽었거나 게임오버 상태면 조작 불가
        if (isDead || (GameManager.Instance != null && GameManager.Instance.IsGameOver())) return;

        // 점프 로직
        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            rigid.linearVelocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            isGrounded = false;
        }

        // 슬라이드 로직
        if (Input.GetButton("Fire2") && isGrounded)
        {
            isSlide = true;
            capsuleCollider.offset = new Vector2(0, -0.44f);
            capsuleCollider.size = new Vector2(0.82f, 0.5f);
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

    // 물리적 충돌 (발판 착지 등)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = 0;
        }

        // 낭떠러지(Dead)에 물리적으로 부딪혔을 때 (Is Trigger가 꺼져있을 때 대비)
        if (collision.collider.CompareTag("Dead"))
        {
            Die();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    // 트리거 충돌 (코인, 장애물, 낭떠러지 데드존)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // 1. 즉사 구역 (낭떠러지) - 무조건 즉사
        if (collision.CompareTag("Dead"))
        {
            Die();
            return; // 즉사했으므로 아래 로직 실행 안 함
        }

        // 2. 일반 장애물 - 무적 상태가 아닐 때만 데미지
        if (collision.CompareTag("Obstacle"))
        {
            if (!isInvincible)
            {
                // 중요: 코루틴 안에서 바꾸지 말고, 여기서 즉시 true로 변경!
                isInvincible = true;
                StartCoroutine(OnDamaged());
            }
        }

        // 3. 코인 체크
        if (collision.CompareTag("Coin"))
        {
            // (오브젝트 풀링을 사용 중이라면) Active(false) 추천
            collision.gameObject.SetActive(false);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(10);
                GameManager.Instance.RestoreHp(5f);
            }
        }
    }

    IEnumerator OnDamaged()
    {
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(30f);
        }

        // 데미지 입은 후 바로 죽었는지 체크
        // (GameManager에서 HP가 0이 되면 자동으로 Die를 호출하게 설계하는 것이 좋습니다)

        // 3초 깜빡임
        float duration = 3f;
        float blinkSpeed = 0.15f;
        float timer = 0;

        while (timer < duration)
        {
            // 죽었다면 깜빡임 중단
            if (isDead) yield break;

            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(blinkSpeed);
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed * 2;
        }

        spriteRenderer.color = new Color(1, 1, 1, 1f);
        isInvincible = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");

        rigid.linearVelocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Static; // 그 자리에 멈춤

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}