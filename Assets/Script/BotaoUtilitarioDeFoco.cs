using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoUtilitarioDeFoco : MonoBehaviour, IPointerDownHandler
{
    public ObjetoDeFoco objetoAlvo;

    private static float tempoUltimoClique;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        float tempoCliqueAtual = Time.time;
        if (tempoCliqueAtual - tempoUltimoClique < UtilitarioDeFocoDaCamera.Instance.tempoDuploClique)
        {
            tempoUltimoClique = 0;
            UtilitarioDeFocoDaCamera.Instance.FocarEmObjeto(objetoAlvo);
        }
        else
        {
            tempoUltimoClique = tempoCliqueAtual;
        }
    }
}
