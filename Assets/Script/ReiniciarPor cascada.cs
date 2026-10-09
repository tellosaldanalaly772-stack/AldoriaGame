
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarPorCascada : MonoBehaviour
{
    private bool reiniciando = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform personaje = other.transform.root;

        if (personaje.name != "Alden_SpriteSheet_1" &&
            personaje.name != "Cedric_SpriteSheet_7")
        {
            return;
        }

        if (reiniciando)
            return;

        reiniciando = true;

        Debug.Log(personaje.name + " tocó la cascada. Reiniciando nivel.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
