using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovimentoCamera : MonoBehaviour
{
    private float pinchDistanciaInicial;
    private bool pinchIniciado;
    public enum BotaoMouse { Esquerdo, Direito, Meio }

    [Header("Zoom No Clique Duplo")]
    public BotaoMouse botaoZoom = BotaoMouse.Esquerdo;
    public float tempoCliqueDuplo = 0.10f;
    public float quantidadeZoomNoClique = 0.95f;
    public float duracaoDoMovimento = 0.5f;

    [Header("Scroll na Roda do Mouse")]
    public float velocidadeScroll = 0.005f;
    public float duracaoPosScroll = 2.5f;
    public float proporcaoScrollParaTras = 0.95f;
    public float maxZoomScrollPorFrame = 0.15f;

    [Header("Limites de Zoom (Z da Câmera)")]
    public float zoomMin = -70000f;
    public float zoomMax = -30000f;

    [Header("Movimento de Panning")]
    public BotaoMouse botaoMovimento = BotaoMouse.Direito;
    public float proporcaoMovimento = 0.0005f;
    public float duracaoPosPanning = 0.9f;

    [Header("Pinch Zoom (Touch)")]
    public float sensibilidadePinch = 0.00015f;
    public float maxZoomPorFrameTouch = 0.15f;

    public bool movendoCamera;
    public Vector2 velocidadeInstantanea;

    private float tempoUltimoClique;
    private Vector2 posicaoInicialMouse;
    private Vector2 posicaoInicialCamera;
    private Vector2 posicaoAnteriorTransform;
    private float velocidadeScrollAtual;
    private bool pinchAtivo;
    private bool usandoToque;
    private bool bloquearToqueSimplesTemporariamente;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Application.isMobilePlatform || Input.touchCount > 0)
        {
            usandoToque = true;

            if (Input.touchCount == 1 && ToqueEmUIInterativa(Input.GetTouch(0)))
                return;

            AtualizaPinchZoom();
            AtualizaMovimentoComUmDedo();

            if (Input.touchCount == 2)
                bloquearToqueSimplesTemporariamente = true;
            else if (Input.touchCount == 0)
                bloquearToqueSimplesTemporariamente = false;
        }
        else
        {
            usandoToque = false;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            AtualizaScroll();
            AtualizaMovimentoPanning();
        }
    }

    bool ToqueEmUIInterativa(Touch touch)
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = touch.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Selectable>() != null)
                return true;
        }
        return false;
    }

    void AtualizaScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float distancia = Mathf.Abs(transform.position.z);
            float escalaDinamica = Mathf.Clamp(distancia / 50000f, 0.3f, 2f);

            float fator = Input.mouseScrollDelta.y > 0
                ? 1f - (velocidadeScroll * escalaDinamica)
                : 1f + (velocidadeScroll * escalaDinamica);

            velocidadeScrollAtual = fator;
            StartCoroutine(ContinuaMovimentoScroll());
        }

        if (CliqueDuploScrollIdentificado())
            StartCoroutine(RotinaDeZoomNoClique());
    }

    IEnumerator ContinuaMovimentoScroll()
    {
        float tempoDecorrido = 0;
        float fatorInicial = Mathf.Clamp(velocidadeScrollAtual, 1f - maxZoomScrollPorFrame, 1f + maxZoomScrollPorFrame);

        while (tempoDecorrido < duracaoPosScroll)
        {
            yield return null;
            tempoDecorrido += Time.deltaTime;
            float t = tempoDecorrido / duracaoPosScroll;
            float fator = Mathf.Lerp(fatorInicial, 1f, t);

            float novoZ = transform.position.z * fator;
            novoZ = Mathf.Clamp(novoZ, zoomMin, zoomMax);

            transform.position = new Vector3(transform.position.x, transform.position.y, novoZ);
        }
    }

    bool CliqueDuploScrollIdentificado()
    {
        if (Input.GetMouseButtonUp((int)botaoZoom))
        {
            float t = Time.time;
            if (t - tempoUltimoClique < tempoCliqueDuplo)
            {
                tempoUltimoClique = t;
                return true;
            }
            tempoUltimoClique = t;
        }
        return false;
    }

    IEnumerator RotinaDeZoomNoClique()
    {
        float alvoZ = transform.position.z * quantidadeZoomNoClique;
        alvoZ = Mathf.Clamp(alvoZ, zoomMin, zoomMax);

        Vector3 inicio = transform.position;
        Vector3 destino = new Vector3(inicio.x, inicio.y, alvoZ);

        float tempoDecorrido = 0;
        while (tempoDecorrido < duracaoDoMovimento)
        {
            yield return null;
            tempoDecorrido += Time.deltaTime;
            float t = tempoDecorrido / duracaoDoMovimento;
            transform.position = Vector3.Lerp(inicio, destino, t);
        }
    }

    void AtualizaMovimentoPanning()
    {
        if (Input.GetMouseButtonDown((int)botaoMovimento))
            IniciaMovimentoDaCamera();

        if (movendoCamera)
            AtualizaPosicaoDaCamera();

        if (Input.GetMouseButtonUp((int)botaoMovimento))
            TerminaMovimentoDaCamera();
    }

    void IniciaMovimentoDaCamera()
    {
        posicaoInicialMouse = Input.mousePosition;
        posicaoInicialCamera = transform.position;
        posicaoAnteriorTransform = posicaoInicialCamera;
        movendoCamera = true;
        StopAllCoroutines();
    }

    void AtualizaPosicaoDaCamera()
    {
        if (Mathf.Approximately(zoomMax, zoomMin)) return;

        float zPos = Mathf.Clamp(transform.position.z, zoomMin, zoomMax);
        float t = Mathf.InverseLerp(zoomMax, zoomMin, zPos);
        float escalaZoom = Mathf.Lerp(1f, 3f, t);

        Vector2 deslocamento = (Vector2)Input.mousePosition - posicaoInicialMouse;
        Vector2 novaPos = posicaoInicialCamera - deslocamento * proporcaoMovimento * escalaZoom;

        Vector3 novaPosicao = new Vector3(novaPos.x, novaPos.y, transform.position.z);

        if (float.IsInfinity(novaPosicao.x) || float.IsNaN(novaPosicao.x) || float.IsInfinity(novaPosicao.y) || float.IsNaN(novaPosicao.y))
            return;

        transform.position = novaPosicao;

        velocidadeInstantanea = ((Vector2)transform.position - posicaoAnteriorTransform) / Time.deltaTime;
        posicaoAnteriorTransform = transform.position;
    }

    void TerminaMovimentoDaCamera()
    {
        movendoCamera = false;
        StartCoroutine(ContinuaMovimentoElastico());
    }

    IEnumerator ContinuaMovimentoElastico()
    {
        float tempoDecorrido = 0;
        while (tempoDecorrido < duracaoPosPanning)
        {
            yield return null;
            tempoDecorrido += Time.deltaTime;

            float t = tempoDecorrido / duracaoPosPanning;
            Vector2 velocidade = Vector2.Lerp(velocidadeInstantanea, Vector2.zero, t);
            transform.position += (Vector3)(velocidade * Time.deltaTime);
        }
    }

    void AtualizaPinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (!pinchIniciado) // Primeiro frame do pinch
            {
                pinchDistanciaInicial = Vector2.Distance(t0.position, t1.position);
                pinchIniciado = true;
                return; // Não aplica zoom neste frame
            }

            float distAtual = Vector2.Distance(t0.position, t1.position);
            float deltaDistancia = distAtual - pinchDistanciaInicial;

            // Só começa o zoom se mover mais que 5px (ajustável)
            if (Mathf.Abs(deltaDistancia) > 5f)
            {
                float distancia = Mathf.Abs(transform.position.z);
                float escalaDinamica = Mathf.Clamp(distancia / 60000f, 0.75f, 2f);

                float fator = 1f + (deltaDistancia * sensibilidadePinch * escalaDinamica);
                fator = Mathf.Clamp(fator, 1f - maxZoomPorFrameTouch, 1f + maxZoomPorFrameTouch);

                float novoZ = transform.position.z * fator;
                novoZ = Mathf.Clamp(novoZ, zoomMin, zoomMax);

                transform.position = new Vector3(transform.position.x, transform.position.y, novoZ);

                // Atualiza a distância de referência para próximo frame
                pinchDistanciaInicial = distAtual;
            }

            pinchAtivo = true;
        }
        else
        {
            pinchAtivo = false;
            pinchIniciado = false;
        }
    }

    void AtualizaMovimentoComUmDedo()
    {
        if (bloquearToqueSimplesTemporariamente)
            return;

        if (Input.touchCount == 1 && !pinchAtivo)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began && ToqueEmUIInterativa(toque))
                return;

            switch (toque.phase)
            {
                case TouchPhase.Began:
                    posicaoInicialMouse = toque.position;
                    posicaoInicialCamera = transform.position;
                    posicaoAnteriorTransform = posicaoInicialCamera;
                    movendoCamera = true;
                    StopAllCoroutines();
                    break;

                case TouchPhase.Moved:
                    if (movendoCamera)
                    {
                        float escalaZoom = Mathf.Lerp(1f, 3f, Mathf.InverseLerp(zoomMax, zoomMin, transform.position.z));
                        Vector2 deslocamento = toque.position - posicaoInicialMouse;
                        Vector2 novaPos = posicaoInicialCamera + deslocamento * proporcaoMovimento * 0.6f * escalaZoom;

                        transform.position = new Vector3(novaPos.x, novaPos.y, transform.position.z);
                        velocidadeInstantanea = ((Vector2)transform.position - posicaoAnteriorTransform) / Time.deltaTime;
                        posicaoAnteriorTransform = transform.position;
                    }
                    break;

                case TouchPhase.Ended:
                    movendoCamera = false;
                    StartCoroutine(ContinuaMovimentoElastico());
                    break;
            }
        }
    }
}
