using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trocanumero : MonoBehaviour
{

    public Sprite imagemEscrita;

    public void TrocaImagem()
    {
        GetComponent<Image>().sprite = imagemEscrita;
    }
}
