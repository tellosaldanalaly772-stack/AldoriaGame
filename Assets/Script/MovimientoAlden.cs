
using UnityEngine;

public class MovimientoAlden : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Animaciones")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool estaEnSuelo;
    private bool solicitarSalto;
    private float movimiento;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movimiento = 0f;

        if (Input.GetKey(KeyCode.A))
            movimiento = -1f;

        if (Input.GetKey(KeyCode.D))
            movimiento = 1f;

        if (Input.GetKeyDown(KeyCode.W) && estaEnSuelo)
            solicitarSalto = true;

        // Cambia entre reposo y carrera.
        if (animator != null)
            animator.SetBool("EnMovimiento", movimiento != 0f);

        // Voltea el personaje según la dirección.
        if (spriteRenderer != null && movimiento != 0f)
            spriteRenderer.flipX = movimiento < 0f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if (solicitarSalto && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            estaEnSuelo = false;
        }

        solicitarSalto = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            estaEnSuelo = true;
    }
}
