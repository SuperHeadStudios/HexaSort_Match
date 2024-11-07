using System.Collections;
using UnityEngine;

public class SetTileTrailColor : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    private void Start()
    {
        StartCoroutine(TrailSoundPlay());
    }

    private IEnumerator TrailSoundPlay()
    {
        yield return new WaitForSeconds(.35f);
        AudioManager.instance.trailAudio.Play();

    }
    public void SetColor(Color color, Material material)
    {
        meshRenderer.material = material;
        trailRenderer.material.color = color; 
    }
}
