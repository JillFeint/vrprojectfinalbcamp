using UnityEngine;
using TMPro;

public class SkyTeleport : MonoBehaviour
{
    [Header("Configuración de Teleport")]
    public Transform puntoDestino;
    public Transform jugador;
    public float tiempoRequerido = 10f;

    [Header("UI para Cardboard")]
    public TextMeshProUGUI textoContador;
    public GameObject objetoCanvas;

    [Header("Ajuste de Límite")]
    // Según tu captura, el límite es 315. 
    // Lo pondremos en 320 para que se active justo antes de llegar al tope.
    public float limiteMiradaArriba = 320f;

    [Header("Estado Actual")]
    public float cronometro = 0f;
    private Transform camaraPrincipal;

    void Start()
    {
        if (Camera.main != null)
            camaraPrincipal = Camera.main.transform;

        if (objetoCanvas != null)
            objetoCanvas.SetActive(false);
    }

    void Update()
    {
        if (camaraPrincipal == null) return;

        float anguloX = camaraPrincipal.localEulerAngles.x;

        // Si el ángulo está entre 270 (mirar recto arriba) y 320 (tu límite)
        if (anguloX > 270f && anguloX <= limiteMiradaArriba)
        {
            if (objetoCanvas != null && !objetoCanvas.activeSelf)
                objetoCanvas.SetActive(true);

            cronometro += Time.deltaTime;
            float restante = tiempoRequerido - cronometro;

            if (textoContador != null)
                textoContador.text = "Regresando en: " + restante.ToString("F1") + "s";

            if (cronometro >= tiempoRequerido)
            {
                EjecutarTeletransporte();
            }
        }
        else
        {
            // REINICIO: Si bajas la mirada de ese punto, el tiempo vuelve a 0 y la UI se borra.
            cronometro = 0f;
            if (objetoCanvas != null && objetoCanvas.activeSelf)
                objetoCanvas.SetActive(false);
        }
    }

    void EjecutarTeletransporte()
    {
        if (puntoDestino != null && jugador != null)
        {
            // 1. Buscamos el controlador para "limpiarlo"
            CharacterController cc = jugador.GetComponent<CharacterController>();

            // 2. Apagamos para permitir el salto de posición
            if (cc != null) cc.enabled = false;

            // 3. Movemos al jugador
            jugador.position = puntoDestino.position;
            // Importante: Alinear también la rotación para que el Gaze mire al frente
            jugador.rotation = puntoDestino.rotation;

            // 4. Encendemos de nuevo
            if (cc != null) cc.enabled = true;

            // 5. Reiniciamos el sistema de Gaze por si se quedó trabado
            // Esto refresca la detección de colisiones del puntero
            Physics.SyncTransforms();

            cronometro = 0f;
            if (objetoCanvas != null) objetoCanvas.SetActive(false);

            Debug.Log("Teletransporte de regreso completado. Nav diamantina habilitada.");
        }
    }
}