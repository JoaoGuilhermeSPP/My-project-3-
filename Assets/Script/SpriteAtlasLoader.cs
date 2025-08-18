using UnityEngine;
using UnityEngine.U2D; // Necessário para Sprite Atlas
using System.Collections.Generic;

public class SpriteAtlasLoader : MonoBehaviour
{
    // Lista de todos os Atlases no jogo
    public List<SpriteAtlas> atlases;

    // Busca a imagem dentro de todos os Atlases pelo nome
    public Sprite GetSprite(string spriteName)
    {
        foreach (var atlas in atlases)
        {
            Sprite sprite = atlas.GetSprite(spriteName);
            if (sprite != null)
                return sprite;
        }
        Debug.LogWarning("Sprite não encontrado: " + spriteName);
        return null;
    }
}
