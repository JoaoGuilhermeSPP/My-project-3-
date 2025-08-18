using UnityEngine;

public class AjusteMenusPorOrientacao : MonoBehaviour
{
    [Header("Referências dos Menus")]
    public RectTransform menuLeft;
    public RectTransform menuRight;

    [Header("Posições no modo VERTICAL (retrato)")]
    public Vector2 posicaoLeftVertical = new Vector2(-300, 100);
    public Vector2 posicaoRightVertical = new Vector2(300, 100);

    [Header("Posições no modo HORIZONTAL (paisagem)")]
    public Vector2 posicaoLeftHorizontal = new Vector2(-600, 200);
    public Vector2 posicaoRightHorizontal = new Vector2(600, 200);

    private ScreenOrientation ultimaOrientacao;
    private Vector2 ultimaLeftPos;
    private Vector2 ultimaRightPos;

    void Start()
    {
        AplicarPosicoesPorOrientacao();
        ultimaOrientacao = Screen.orientation;
        ultimaLeftPos = menuLeft != null ? menuLeft.anchoredPosition : Vector2.zero;
        ultimaRightPos = menuRight != null ? menuRight.anchoredPosition : Vector2.zero;
    }

    void Update()
    {
        bool orientacaoMudou = Screen.orientation != ultimaOrientacao;
        bool posicoesMudaram = (
            (menuLeft != null && menuLeft.anchoredPosition != ultimaLeftPos) ||
            (menuRight != null && menuRight.anchoredPosition != ultimaRightPos)
        );

#if UNITY_EDITOR
        // Sempre atualiza no Editor para testes ao vivo
        AplicarPosicoesPorOrientacao();
#else
        if (orientacaoMudou || posicoesMudaram)
        {
            AplicarPosicoesPorOrientacao();
        }
#endif

        ultimaOrientacao = Screen.orientation;
        if (menuLeft != null) ultimaLeftPos = menuLeft.anchoredPosition;
        if (menuRight != null) ultimaRightPos = menuRight.anchoredPosition;
    }

    void AplicarPosicoesPorOrientacao()
    {
        bool emVertical = Screen.height >= Screen.width;

        if (emVertical)
        {
            if (menuLeft != null) menuLeft.anchoredPosition = posicaoLeftVertical;
            if (menuRight != null) menuRight.anchoredPosition = posicaoRightVertical;
           
        }
        else
        {
            if (menuLeft != null) menuLeft.anchoredPosition = posicaoLeftHorizontal;
            if (menuRight != null) menuRight.anchoredPosition = posicaoRightHorizontal;
          
        }
    }
}
