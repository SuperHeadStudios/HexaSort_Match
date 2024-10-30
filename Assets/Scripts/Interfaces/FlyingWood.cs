using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingWood : MonoBehaviour
{
    public static FlyingWood instance;

    public GameObject starPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform starObj1;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnWood(Vector3 spawnPos)
    {
        starObj1.gameObject.SetActive(true);
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
        starObj1.position = WorldToCanvasPosition(uiCanvas, starObj1, Camera.main, spawnPos);

        starObj1.DOMove(targetRoot.position,0.35f).OnComplete(() =>
        {
            starObj1.gameObject.SetActive(false);
            GameManager.instance.IncreaseWoodCount();
        });
    }
    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }
}
