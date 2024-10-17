using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGrass : MonoBehaviour
{
    public static FlyingGrass instance;

    public GameObject grassPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform grassObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GameObject obj1 = Instantiate(grassPrefab) as GameObject;
        obj1.SetActive(true);
        grassObj = obj1.GetComponent<RectTransform>();
        grassObj.SetParent(transform);
        grassObj.localScale = Vector3.one;
        grassObj.gameObject.SetActive(false);
    }
    public void SpawnGrass(Vector3 spawnPos)
    {
        grassObj.gameObject.SetActive(true);
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
        grassObj.position = WorldToCanvasPosition(uiCanvas, grassObj, Camera.main, spawnPos);
        grassObj.DOMove(targetRoot.position, 0.3f).OnComplete(() =>
        {
            grassObj.gameObject.SetActive(false);
            GameManager.instance.IncreaseGrassCount();
        });
    }

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }
}
