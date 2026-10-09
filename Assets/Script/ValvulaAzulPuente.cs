
using UnityEngine;
using System.Collections;

public class ValvulaAzulPuente : MonoBehaviour
{
    [Header("Interacción")]
    public KeyCode teclaActivacion = KeyCode.Return;
    public string nombreCedric = "Cedric_SpriteSheet_7";

    [Header("Objetos")]
    public GameObject puenteMadera;

    [Header("Giro de la válvula")]
    public float gradosGiro = 360f;
    public float duracionGiro = 1f;

    private bool cedricCerca;
    private bool activada;
    private bool girando;

    private void Update()
    {
        if (!cedricCerca || activada || girando)
            return;

        if (Input.GetKeyDown(teclaActivacion))
            StartCoroutine(ActivarValvula());
    }

    private IEnumerator ActivarValvula()
    {
        girando = true;

        // El puente aparece inmediatamente.
        if (puenteMadera != null)
        {
            puenteMadera.SetActive(true);
            Debug.Log("Puente de madera activado.");
        }
        else
        {
            Debug.LogError(
                "Asigna Puente_madera en el Inspector."
            );
        }

        // La válvula gira mientras el puente ya está activo.
        Quaternion inicio = transform.rotation;
        Quaternion destino = inicio *
            Quaternion.Euler(0f, 0f, gradosGiro);

        float tiempo = 0f;

        while (tiempo < duracionGiro)
        {
            tiempo += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(
                inicio,
                destino,
                Mathf.Clamp01(tiempo / duracionGiro)
            );

            yield return null;
        }

        transform.rotation = destino;

        activada = true;
        girando = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == nombreCedric ||
            other.transform.root.name == nombreCedric)
        {
            cedricCerca = true;
            Debug.Log("Cedric está cerca de la válvula azul.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == nombreCedric ||
            other.transform.root.name == nombreCedric)
        {
            cedricCerca = false;
        }
    }
}
