
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CamaraCooperativa : MonoBehaviour
{
    [Header("Personajes")]
    public Transform alden;
    public Transform cedric;

    [Header("Encuadre")]
    public Vector2 desplazamiento = new Vector2(0f, 1f);
    [Min(0.01f)] public float suavidad = 0.25f;

    [Header("Zoom")]
    [Min(1f)] public float zoomNormal = 5f;
    [Min(1f)] public float zoomMaximo = 7f;
    [Min(0f)] public float margenHorizontal = 2f;
    [Min(0f)] public float margenVertical = 1.5f;
    [Min(0.01f)] public float velocidadZoom = 2f;

    private Camera cam;
    private Vector3 velocidadSuavizado;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = zoomNormal;
    }

    void LateUpdate()
    {
        if (alden == null || cedric == null)
            return;

        // Punto medio entre los personajes.
        Vector3 medio = (alden.position + cedric.position) * 0.5f;

        Vector3 destino = new Vector3(
            medio.x + desplazamiento.x,
            medio.y + desplazamiento.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            destino,
            ref velocidadSuavizado,
            suavidad
        );

        // Calcular el zoom necesario para encuadrar a ambos.
        float distanciaX = Mathf.Abs(alden.position.x - cedric.position.x);
        float distanciaY = Mathf.Abs(alden.position.y - cedric.position.y);

        float zoomHorizontal =
            (distanciaX * 0.5f + margenHorizontal) / cam.aspect;

        float zoomVertical =
            distanciaY * 0.5f + margenVertical;

        float zoomObjetivo = Mathf.Clamp(
            Mathf.Max(zoomNormal, zoomHorizontal, zoomVertical),
            zoomNormal,
            zoomMaximo
        );

        cam.orthographicSize = Mathf.MoveTowards(
            cam.orthographicSize,
            zoomObjetivo,
            velocidadZoom * Time.deltaTime
        );
    }
}
