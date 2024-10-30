using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;

public class FlyingStarRoot : MonoBehaviour
{
    public GameObject starPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform starObj1;

    public ParticleSystem flowerEffect;

    public RectTransform trail;

    private void Update()
    {
        
    }


    void Start()
    {
        GameObject obj1 = Instantiate(starPrefab) as GameObject;
        obj1.SetActive(true);
        starObj1 = obj1.GetComponent<RectTransform>();
        starObj1.SetParent(transform);
        starObj1.localScale = Vector3.one;
        starObj1.gameObject.SetActive(false);
    }
    public void SpawnStar(Vector3 spawnPos, Color currentColor)
    {
        starObj1.GetComponent<Image>().color = currentColor;
        starObj1.gameObject.SetActive(true);
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
        starObj1.position = WorldToCanvasPosition(uiCanvas, starObj1, Camera.main, spawnPos);
        starObj1.DOMove(targetRoot.position, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            starObj1.gameObject.SetActive(false);
            flowerEffect.Play();
            trail.position = starObj1.position;
        });
    }














    /*public GameObject starPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform starObj1;

    private void Awake()
    {

    }

    void Start()
    {
        GameObject obj1 = Instantiate(starPrefab) as GameObject;
        obj1.SetActive(true);
        starObj1 = obj1.GetComponent<RectTransform>();
        starObj1.SetParent(transform);
        starObj1.localScale = Vector3.one;
        starObj1.gameObject.SetActive(false);
    }
    void Update()
    {

    }
    public void SpawnStar(Vector3 spawnPos)
    {
            starObj1.gameObject.SetActive(true);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
            starObj1.position = WorldToCanvasPosition(uiCanvas, starObj1, Camera.main, spawnPos);
            starObj1.DOMove(targetRoot.position, 1f).OnComplete(() =>
            {
                starObj1.gameObject.SetActive(false);
            });
    }*/

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }

    /*Vector3 BetweenP(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }*/
}
