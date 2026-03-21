using UnityEngine;
using UnityEngine.Events;

public class DiamanteRutaIndependiente : MonoBehaviour
{
    public UnityEvent OnUsed;
    private Transform playerTransform;

    void Start()
    {
        // Buscamos al Player (1) que aparece en tu jerarquía
        GameObject player = GameObject.Find("Player");
        if (player != null) playerTransform = player.transform;
    }

    // Estas versiones aceptan CUALQUIER llamada del puntero, con o sin datos
    public void OnPointerClickXR() { Teletransportar(); }
    public void OnPointerClickXR(GameObject data) { Teletransportar(); }

    private void Teletransportar()
    {
        if (playerTransform != null)
        {
            playerTransform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
            OnUsed.Invoke();
        }
        else
        {
            Debug.LogWarning("No se encontró al objeto 'Player'. Revisa el nombre.");
        }
    }

    // Funciones vacías para absorber los mensajes de error de la consola
    public void OnPointerEnterXR() { }
    public void OnPointerEnterXR(GameObject data) { }
    public void OnPointerExitXR() { }
    public void OnPointerExitXR(GameObject data) { }
}