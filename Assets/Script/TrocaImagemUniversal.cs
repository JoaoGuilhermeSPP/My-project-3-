using UnityEngine;
using UnityEngine.UI;     // Para UI Image
using UnityEngine.U2D;    // Para Sprite Atlas

public class TrocaImagemUniversal : MonoBehaviour
{
    public string nomeDaImagemNoAtlas;

    void Start()
    {
        SpriteAtlasLoader loader = FindObjectOfType<SpriteAtlasLoader>();
        Sprite sprite = loader.GetSprite(nomeDaImagemNoAtlas);

        // Detecta se é UI Image
        Image imageUI = GetComponent<Image>();
        if (imageUI != null)
        {
            imageUI.sprite = sprite;
            return;
        }

        // Detecta se é SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
