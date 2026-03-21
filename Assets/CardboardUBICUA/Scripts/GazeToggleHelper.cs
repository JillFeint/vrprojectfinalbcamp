using UnityEngine;
using UnityEngine.UI;

public class GazeToggleHelper : MonoBehaviour
{
    private Toggle _toggle;
    void Start() { _toggle = GetComponent<Toggle>(); }

    // Quitamos el (object data) y dejamos los paréntesis VACÍOS ()
    public void OnPointerClickXR()
    {
        if (_toggle != null)
        {
            _toggle.isOn = !_toggle.isOn;
            Debug.Log("¡Gazer activó el Toggle!");
        }
    }

    // También vaciamos estas para que no den error
    public void OnPointerEnterXR() { }
    public void OnPointerExitXR() { }
}