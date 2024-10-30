using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaCell : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Color currentColor;

    public void InitCell(int colorID)
    {
        //Debug.Log(colorID + "ColorId" );
        meshRenderer.material = GameManager.instance.colorConfig.colorList[colorID].material;
        currentColor = GameManager.instance.colorConfig.colorList[colorID].colorValue;
    }
}
