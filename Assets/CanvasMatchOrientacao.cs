using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasMatchOrientacao : MonoBehaviour
{
    [Header("Configura��o do Match")]
    [Tooltip("Match quando em modo horizontal (landscape)")]
    [Range(0f, 1f)] public float matchHorizontal = 0f;

    [Tooltip("Match quando em modo vertical (portrait)")]
    [Range(0f, 1f)] public float matchVertical = 1f;

    [Header("Somente no Editor")]
    [Tooltip("For�a detec��o de orienta��o baseada na propor��o da tela no Editor")]
    public bool simularNoEditor = true;

    private CanvasScaler canvasScaler;
    private bool ultimaFoiPaisagem;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        AtualizarMatch();
    }

    void Update()
    {
        bool emPaisagem = EstaEmModoPaisagem();

        if (emPaisagem != ultimaFoiPaisagem)
        {
            AtualizarMatch();
        }
    }

    void AtualizarMatch()
    {
        bool emPaisagem = EstaEmModoPaisagem();
        ultimaFoiPaisagem = emPaisagem;

        canvasScaler.matchWidthOrHeight = emPaisagem ? matchHorizontal : matchVertical;
#if UNITY_EDITOR
        Debug.Log($"[CanvasMatchOrientacao] Orienta��o detectada: {(emPaisagem ? "Horizontal" : "Vertical")} - Match: {canvasScaler.matchWidthOrHeight}");
#endif
    }

    bool EstaEmModoPaisagem()
    {
#if UNITY_EDITOR
        if (simularNoEditor)
        {
            return Screen.width > Screen.height;
        }
#endif

        return Screen.orientation == ScreenOrientation.LandscapeLeft
               || Screen.orientation == ScreenOrientation.LandscapeLeft
               || Screen.orientation == ScreenOrientation.LandscapeRight
               || Screen.width > Screen.height;
    }
}
