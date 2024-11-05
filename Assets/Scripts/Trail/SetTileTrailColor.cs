using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTileTrailColor : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TrailRenderer trailRenderer;

    public void SetColor(Color color, Material material)
    {
        meshRenderer.material = material;
        trailRenderer.material.color = color; 
    }
}
