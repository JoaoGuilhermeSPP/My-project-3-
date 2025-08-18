using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class TrocaImagemAuto : MonoBehaviour
{
    void Start()
    {
        SpriteAtlasLoader loader = FindObjectOfType<SpriteAtlasLoader>();

        // Tenta pegar componente UI Image
        Image imageUI = GetComponent<Image>();
        if (imageUI != null && imageUI.sprite != null)
        {
            string nomeSprite = imageUI.sprite.name;
            imageUI.sprite = loader.GetSprite(nomeSprite);
            return;
        }

        // Tenta pegar componente SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            string nomeSprite = spriteRenderer.sprite.name;
            spriteRenderer.sprite = loader.GetSprite(nomeSprite);
        }
    }
}
