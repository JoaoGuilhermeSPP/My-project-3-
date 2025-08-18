using UnityEngine;

public class ResponsiveLayoutManager : MonoBehaviour
{
    [Header("Menus Duplicados")]
    public GameObject leftMenu_Vertical;
    public GameObject leftMenu_Horizontal;
    public GameObject rightMenu_Vertical;
    public GameObject rightMenu_Horizontal;

    [Header("DEBUG (Editor)")]
    public bool simulateMobileInEditor = true;
    public bool forceHorizontalInEditor = false;

    private bool isLandscape;
    private bool isMobile;

    private bool leftMenuVisible = true;
    private bool rightMenuVisible = true;

    void Start()
    {
        ApplyLayout();
    }

    void Update()
    {
        bool landscapeNow = Screen.width > Screen.height;
        bool mobileNow = DetectMobile();

        if (landscapeNow != isLandscape || mobileNow != isMobile)
        {
            isLandscape = landscapeNow;
            isMobile = mobileNow;
            ApplyLayout();
        }

#if UNITY_EDITOR
        if (simulateMobileInEditor)
        {
            ApplyLayout();
        }
#endif
    }

    bool DetectMobile()
    {
#if UNITY_EDITOR
        return simulateMobileInEditor;
#else
        return Application.isMobilePlatform ||
               (Application.platform == RuntimePlatform.WebGLPlayer && Screen.width < 1024);
#endif
    }

    bool IsHorizontal()
    {
        bool horizontal = Screen.width > Screen.height;

#if UNITY_EDITOR
        if (simulateMobileInEditor)
            horizontal = forceHorizontalInEditor;
#endif

        return DetectMobile() && horizontal;
    }

    // 🟩 BOTÃO: Alternar menu esquerdo
    public void AlternarMenuEsquerdo()
    {
        bool horizontal = IsHorizontal();
        leftMenuVisible = !leftMenuVisible;

        if (horizontal)
        {
            if (leftMenu_Horizontal != null) leftMenu_Horizontal.SetActive(leftMenuVisible);
            if (leftMenu_Vertical != null) leftMenu_Vertical.SetActive(false);
        }
        else
        {
            if (leftMenu_Vertical != null) leftMenu_Vertical.SetActive(leftMenuVisible);
            if (leftMenu_Horizontal != null) leftMenu_Horizontal.SetActive(false);
        }
    }

    //  BOTÃO: Alternar menu direito
    public void AlternarMenuDireito()
    {
        bool horizontal = IsHorizontal();
        rightMenuVisible = !rightMenuVisible;

        if (horizontal)
        {
            if (rightMenu_Horizontal != null) rightMenu_Horizontal.SetActive(rightMenuVisible);
            if (rightMenu_Vertical != null) rightMenu_Vertical.SetActive(false);
        }
        else
        {
            if (rightMenu_Vertical != null) rightMenu_Vertical.SetActive(rightMenuVisible);
            if (rightMenu_Horizontal != null) rightMenu_Horizontal.SetActive(false);
        }
    }

    //  Atualiza todos os menus conforme orientação atual e visibilidade salva
    void ApplyLayout()
    {
        bool horizontal = IsHorizontal();

        // LEFT
        leftMenu_Vertical.SetActive(!horizontal && leftMenuVisible);
        leftMenu_Horizontal.SetActive(horizontal && leftMenuVisible);

        // RIGHT
        rightMenu_Vertical.SetActive(!horizontal && rightMenuVisible);
        rightMenu_Horizontal.SetActive(horizontal && rightMenuVisible);

        // Garante que os menus da orientação oposta estão desligados
        if (horizontal)
        {
            if (leftMenu_Vertical != null) leftMenu_Vertical.SetActive(false);
            if (rightMenu_Vertical != null) rightMenu_Vertical.SetActive(false);
        }
        else
        {
            if (leftMenu_Horizontal != null) leftMenu_Horizontal.SetActive(false);
            if (rightMenu_Horizontal != null) rightMenu_Horizontal.SetActive(false);
        }
    }
}
