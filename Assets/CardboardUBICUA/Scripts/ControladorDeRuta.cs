using UnityEngine;
using System.Collections.Generic;

public class ControladorDeRuta : MonoBehaviour
{
    public GameObject prefabDiamanteLimpio;
    public Transform[] puntosGuia;
    public float distanciaEntrePuntos = 5.0f;
    public Color colorRuta = Color.red;
    public float separacionLateral = 0.0f;

    // Nueva variable para controlar la velocidad del parpadeo
    public float velocidadPalpito = 1.0f;

    private List<GameObject> diamantesCreados = new List<GameObject>();

    // Añadimos Update para el efecto de parpadeo
    void Update()
    {
        if (diamantesCreados.Count > 0)
        {
            // 0.6f es el mínimo de visibilidad (más sólido)
            // 0.4f es el rango de oscilación (qué tanto sube y baja)
            float lerp = 0.6f + Mathf.PingPong(Time.time * velocidadPalpito, 0.4f);

            foreach (GameObject d in diamantesCreados)
            {
                if (d != null)
                {
                    MeshRenderer r = d.GetComponentInChildren<MeshRenderer>();
                    if (r != null)
                    {
                        Color c = colorRuta;
                        c.a = lerp;
                        r.material.color = c;
                    }
                }
            }
        }
    }

    public void ConmutarRuta(bool activado)
    {
        if (activado) GenerarCamino();
        else BorrarCamino();
    }

    void GenerarCamino()
    {
        BorrarCamino();
        if (puntosGuia == null || puntosGuia.Length < 2) return;

        for (int i = 0; i < puntosGuia.Length - 1; i++)
        {
            if (puntosGuia[i] == null || puntosGuia[i + 1] == null) continue;

            Vector3 inicio = puntosGuia[i].position;
            Vector3 fin = puntosGuia[i + 1].position;
            float tramo = Vector3.Distance(inicio, fin);
            Vector3 dir = (fin - inicio).normalized;

            int cantidad = Mathf.FloorToInt(tramo / distanciaEntrePuntos);

            for (int j = 0; j <= cantidad; j++)
            {
                Vector3 pos = inicio + (dir * (j * distanciaEntrePuntos));

                // Mantengo tu ajuste de altura de 1.6f
                pos.y -= 1.6f;

                Vector3 derecha = Vector3.Cross(dir, Vector3.up).normalized;
                pos += derecha * separacionLateral;

                GameObject d = Instantiate(prefabDiamanteLimpio, pos, Quaternion.LookRotation(dir));

                MeshRenderer r = d.GetComponentInChildren<MeshRenderer>();
                if (r != null)
                {
                    // Importante: El material debe permitir transparencia (ej. modo Fade o Transparent)
                    r.material.color = colorRuta;
                }
                d.transform.localScale = Vector3.one * 1.2f;

                var scriptD = d.GetComponent<DiamanteRutaIndependiente>();
                if (scriptD != null)
                {
                    scriptD.OnUsed.AddListener(() => DesactivarPrevios(d));
                }

                diamantesCreados.Add(d);
            }
        }
    }

    void DesactivarPrevios(GameObject actual)
    {
        int indice = diamantesCreados.IndexOf(actual);
        for (int i = 0; i <= indice; i++)
        {
            if (diamantesCreados[i] != null) diamantesCreados[i].SetActive(false);
        }
    }

    void BorrarCamino()
    {
        foreach (GameObject d in diamantesCreados) if (d != null) Destroy(d);
        diamantesCreados.Clear();
    }
}