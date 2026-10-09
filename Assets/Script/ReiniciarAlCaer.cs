
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarAlCaer : MonoBehaviour
{
    [Header("Altura límite")]
    public float alturaLimite = -5f;

    private bool reiniciando = false;

    void Update()
    {
        if (reiniciando)
            return;

        GameObject alden = GameObject.Find("Alden_SpriteSheet_1");
        GameObject cedric = GameObject.Find("Cedric_SpriteSheet_7");

        if (alden != null && alden.transform.position.y < alturaLimite)
        {
            ReiniciarNivel();
        }
        else if (cedric != null && cedric.transform.position.y < alturaLimite)
        {
            ReiniciarNivel();
        }
    }

    private void ReiniciarNivel()
    {
        reiniciando = true;

        Debug.Log("Un personaje cayó al vacío. Reiniciando Nivel4.");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
