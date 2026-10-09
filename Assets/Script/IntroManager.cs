using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[System.Serializable]
public class Vineta
{
    public Sprite sprite;
    public float duracion = 6f;
    public float zoomInicial = 1f;
    public float zoomFinal = 1.15f;
    public Vector2 desplazamiento = Vector2.zero;
    public AudioClip sonido;   // voz del narrador
    public AudioClip musica;   // música de fondo (opcional)
}

public class IntroManager : MonoBehaviour
{
    public Image imagen;
    public AudioSource audioSource;    // voz
    public AudioSource musicaSource;   // música
    public Vineta[] vinetas;
    public float fade = 0.8f;
    public float pausaFinal = 1f;
    [Range(0f, 1f)] public float volumenMusica = 0.4f;
    public string escenaSiguiente = "";

    IEnumerator Start()
    {
        imagen.color = new Color(1, 1, 1, 0);
        var rt = imagen.rectTransform;

        foreach (var v in vinetas)
        {
            float dur = v.duracion > 0f ? v.duracion : 6f;
            float zi = v.zoomInicial > 0f ? v.zoomInicial : 1f;
            float zf = v.zoomFinal > 0f ? v.zoomFinal : 1.15f;

            // La viñeta dura al menos lo que dura la voz
            if (v.sonido != null)
                dur = Mathf.Max(dur, v.sonido.length + pausaFinal);

            imagen.sprite = v.sprite;

            if (v.sonido != null)
            {
                audioSource.clip = v.sonido;
                audioSource.Play();
            }

            bool hayMusica = v.musica != null && musicaSource != null;
            if (hayMusica)
            {
                musicaSource.clip = v.musica;
                musicaSource.volume = 0f;
                musicaSource.Play();
            }

            float t = 0f;
            while (t < dur)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / dur);
                float e = Mathf.SmoothStep(0f, 1f, k);

                float z = Mathf.Lerp(zi, zf, e);
                rt.localScale = new Vector3(z, z, 1f);
                rt.anchoredPosition = Vector2.Lerp(-v.desplazamiento / 2f, v.desplazamiento / 2f, e);

                float alfa = Mathf.Clamp01(Mathf.Min(t, dur - t) / fade);
                imagen.color = new Color(1, 1, 1, alfa);

                // La música sube y baja con el mismo fundido de la imagen
                if (hayMusica) musicaSource.volume = volumenMusica * alfa;

                if (PidioSaltar()) { Terminar(); yield break; }
                yield return null;
            }

            if (hayMusica) musicaSource.Stop();
        }
        Terminar();
    }

    bool PidioSaltar()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Space);
#endif
    }

    void Terminar()
    {
        audioSource.Stop();
        if (musicaSource != null) musicaSource.Stop();

        if (!string.IsNullOrEmpty(escenaSiguiente))
            SceneManager.LoadScene(escenaSiguiente);
        else
            Debug.Log("Intro terminada (aún no hay escena siguiente).");
    }
}