
using UnityEngine;

public class MovimientoCedric : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Animaciones")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool estaEnSuelo;
    private bool solicitarSalto;
    private float movimiento;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }


void Update()
{
    movimiento = 0f;

    if (Input.GetKey(KeyCode.LeftArrow))
        movimiento = -1f;

    if (Input.GetKey(KeyCode.RightArrow))
        movimiento = 1f;

    // Girar el sprite según la dirección.
    if (movimiento < 0f)
        GetComponent<SpriteRenderer>().flipX = true;
    else if (movimiento > 0f)
        GetComponent<SpriteRenderer>().flipX = false;

    // Actualizar animaciones.
    if (animator != null)
    {
        animator.SetBool("EnMovimiento", movimiento != 0f);
        animator.SetBool("EnSuelo", estaEnSuelo);
    }

    if (Input.GetKeyDown(KeyCode.UpArrow) && estaEnSuelo)
    {
        solicitarSalto = true;
    }
}


    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if (solicitarSalto)
        {
            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    fuerzaSalto
                );

                estaEnSuelo = false;

                if (animator != null)
                    animator.SetBool("EnSuelo", false);
            }

            solicitarSalto = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ComprobarSuelo(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ComprobarSuelo(collision);
    }

    private void ComprobarSuelo(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Suelo"))
            return;

        foreach (ContactPoint2D contacto in collision.contacts)
        {
            // Solo considerar suelo cuando Cedric toca desde arriba.
            if (contacto.normal.y > 0.5f)
            {
                estaEnSuelo = true;

                if (animator != null)
                    animator.SetBool("EnSuelo", true);

                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;

            if (animator != null)
                animator.SetBool("EnSuelo", false);
        }
    }
}
