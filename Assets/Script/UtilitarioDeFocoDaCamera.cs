using System.Collections;
using UnityEngine;

public class UtilitarioDeFocoDaCamera : MonoBehaviour
{
    public static UtilitarioDeFocoDaCamera Instance { get; private set; }
    
    public float tempoDuploClique = 0.2f;
    public Vector3 offsetPadrao = new Vector3(0, 0, -10);
    public float duracaoDoMovimento = 1f;
    public bool habilitarMensagensDebug;
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Debug.LogWarning("A cena contém múltiplas instâncias de UtilitarioDeFocoDaCamera." +
                             "Esta instância será destruída.", gameObject);
            Destroy(this);
        }
    }

    public void FocarEmObjeto(ObjetoDeFoco objetoAlvo)
    {
        StopAllCoroutines();
        StartCoroutine(FocarEmObjetoRoutine(objetoAlvo));
    }

    IEnumerator FocarEmObjetoRoutine(ObjetoDeFoco objetoAlvo)
    {
        if (habilitarMensagensDebug)
            Debug.Log("Focando em " + objetoAlvo.name, objetoAlvo.gameObject);

        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoFinal = objetoAlvo.transform.position + objetoAlvo.offsetCamera;
        if (!objetoAlvo.substituirOffsetPadrao)
            posicaoFinal += offsetPadrao;
        
        float tempoDecorrido = 0;
        while (tempoDecorrido < duracaoDoMovimento)
        {
            yield return null;
            tempoDecorrido += Time.deltaTime;
            float t = Easing.SmoothStop(tempoDecorrido / duracaoDoMovimento);
            transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, t);
        }

        transform.position = posicaoFinal;
    }
}
