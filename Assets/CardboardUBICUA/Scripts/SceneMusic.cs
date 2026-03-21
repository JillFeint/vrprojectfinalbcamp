using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // Estas variables DEBEN aparecer en el Inspector si el código no tiene errores
    public AudioClip musicaDeEscena;
    [Range(0f, 1f)]
    public float volumen = 0.5f;
    public bool bucle = true;

    private AudioSource source;

    void Start()
    {
        // Añadimos el componente de sonido por código para que no tengas que hacerlo tú
        source = gameObject.AddComponent<AudioSource>();

        if (musicaDeEscena != null)
        {
            source.clip = musicaDeEscena;
            source.volume = volumen;
            source.loop = bucle;
            source.playOnAwake = true;
            source.Play();
        }
    }
}