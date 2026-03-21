using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SkyTeleport : MonoBehaviour
{
    [Header("Configuración de Teleport")]
    public string nombreEscenaPrincipal = "1-Escena Principal";
    public Transform puntoDestino;      // Arrastra aquí el "PuntoInicio" de la Escena 1
    public Transform jugador;            // Arrastra aquí tu "Player"
    public float tiempoRequerido = 10f;

    [Header("UI para Cardboard")]
    public TextMeshProUGUI textoContador;
    public GameObject objetoCanvas;

    [Header("Ajuste de Límite")]
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
        if (camaraPrincipal == null)
        {
            if (Camera.main != null) camaraPrincipal = Camera.main.transform;
            return;
        }

        float anguloX = camaraPrincipal.localEulerAngles.x;

        if (anguloX > 270f && anguloX <= limiteMiradaArriba)
        {
            if (objetoCanvas != null && !objetoCanvas.activeSelf)
                objetoCanvas.SetActive(true);

            cronometro += Time.deltaTime;
            float restante = tiempoRequerido - cronometro;

            if (textoContador != null)
                textoContador.text = "Regresando al Inicio en: " + restante.ToString("F1") + "s";

            if (cronometro >= tiempoRequerido)
            {
                DecidirDestino();
            }
        }
        else
        {
            cronometro = 0f;
            if (objetoCanvas != null && objetoCanvas.activeSelf)
                objetoCanvas.SetActive(false);
        }
    }

    void DecidirDestino()
    {
        cronometro = 0f;
        if (objetoCanvas != null) objetoCanvas.SetActive(false);

        // REGLA DE ORO:
        // Si la escena actual es la Principal, solo nos movemos de posición.
        // Si NO es la principal, cargamos la escena desde cero.
        if (SceneManager.GetActiveScene().name == nombreEscenaPrincipal)
        {
            MoverJugadorAPuntoUno();
        }
        else
        {
            SceneManager.LoadScene(nombreEscenaPrincipal);
        }
    }

    void MoverJugadorAPuntoUno()
    {
        if (puntoDestino != null && jugador != null)
        {
            CharacterController cc = jugador.GetComponent<CharacterController>();

            // Apagamos el controlador para evitar bloqueos
            if (cc != null) cc.enabled = false;

            jugador.position = puntoDestino.position;
            jugador.rotation = puntoDestino.rotation;

            // Encendemos y sincronizamos
            if (cc != null) cc.enabled = true;
            Physics.SyncTransforms();

            Debug.Log("Teletransportado al Punto 1 dentro de la misma escena.");
        }
    }
}