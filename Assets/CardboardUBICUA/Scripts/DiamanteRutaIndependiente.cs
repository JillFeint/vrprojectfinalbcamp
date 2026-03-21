using UnityEngine;
using UnityEngine.Events;

public class DiamanteRutaIndependiente : MonoBehaviour
{
    public UnityEvent OnUsed;
    private Transform playerTransform;

    void Start()
    {
        // Busca al jugador automáticamente para no tener que arrastrarlo manualmente
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) player = GameObject.Find("Player");

        if (player != null) playerTransform = player.transform;
    }

    // Estas funciones las llama tu mirada (Gazer)
    public void OnPointerClickXR()
    {
        if (playerTransform != null)
        {
            // Te mueve a la posición del diamante (manteniendo tu altura de ojos)
            playerTransform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);

            // Avisa que ya fue usado para desaparecer el rastro
            OnUsed.Invoke();
        }
    }

    public void OnPointerEnterXR() { /* Aquí podrías poner un sonido */ }
    public void OnPointerExitXR() { /* O cambiar el tamaño al mirar */ }
}