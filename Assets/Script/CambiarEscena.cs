using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public string nombreEscena;

    public void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}