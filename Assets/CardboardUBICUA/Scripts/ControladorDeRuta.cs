using UnityEngine;
using System.Collections.Generic;

public class ControladorDeRuta : MonoBehaviour
{
    public GameObject prefabDiamanteLimpio;
    public Transform[] puntosGuia;
    public float distanciaEntrePuntos = 5.0f;
    public Color colorRuta = Color.red;
    public float separacionLateral = 0.0f;

    private List<GameObject> diamantesCreados = new List<GameObject>();

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

                // Aplicar offset para que no choque con otras rutas
                Vector3 derecha = Vector3.Cross(dir, Vector3.up).normalized;
                pos += derecha * separacionLateral;

                GameObject d = Instantiate(prefabDiamanteLimpio, pos, Quaternion.LookRotation(dir));

                // Color y escala
                MeshRenderer r = d.GetComponentInChildren<MeshRenderer>();
                if (r != null) r.material.color = colorRuta;
                d.transform.localScale = Vector3.one * 1.2f;

                // Conectar la desaparición automática
                var scriptD = d.GetComponent<DiamanteRutaIndependiente>();
                scriptD.OnUsed.AddListener(() => DesactivarPrevios(d));

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