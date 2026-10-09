
using UnityEngine;

public class ValvulaCloacas : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject cascada;

    [Header("Interacción")]
    public KeyCode teclaInteraccion = KeyCode.E;

    private bool aldenCerca;
    private bool cascadaCerrada;

    private void Update()
    {
        if (!aldenCerca || !Input.GetKeyDown(teclaInteraccion))
            return;

        cascadaCerrada = !cascadaCerrada;

        if (cascada != null)
        {
            cascada.SetActive(!cascadaCerrada);
        }

        if (cascadaCerrada)
            Debug.Log("Alden cerró la válvula. La cascada se detuvo.");
        else
            Debug.Log("Alden abrió la válvula. La cascada volvió a fluir.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.name == "Alden_SpriteSheet_1")
        {
            aldenCerca = true;
            Debug.Log("Alden está cerca de la válvula. Pulsa E.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.name == "Alden_SpriteSheet_1")
        {
            aldenCerca = false;
        }
    }
}
