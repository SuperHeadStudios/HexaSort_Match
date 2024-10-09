using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaCell : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public void InitCell(int colorID)
    {
        Debug.Log(colorID + "ColorId" );
        meshRenderer.material = GameManager.instance.colorConfig.colorList[colorID].material;
        
    }
}
