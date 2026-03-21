using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class TeleportToScene : MonoBehaviour
{
    [Header("Configuración de Destino")]
    public string nombreEscena; // Aquí escribirás el nombre, por ejemplo: 7-Morgue

    [Header("Efectos")]
    public ParticleSystem particulas; // Aquí arrastraremos el EfectoTP_Ragnarok

    // Esta es la función exacta que busca tu CameraPointerManager
    public void OnPointerClickXR()
    {
        // 1. Iniciamos las partículas para que el usuario las vea
        if (particulas != null)
        {
            particulas.Play();
        }

        // 2. Esperamos un momento (ej. 1 segundo) para que luzca el efecto
        // antes de cargar la siguiente escena
        Invoke("CargarMapa", 1.0f);
    }

    private void CargarMapa()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}