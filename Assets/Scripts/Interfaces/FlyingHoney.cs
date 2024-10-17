using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHoney : MonoBehaviour
{
    public static FlyingHoney instance;

    public GameObject honeyPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform honeyObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GameObject obj1 = Instantiate(honeyPrefab) as GameObject;
        obj1.SetActive(true);
        honeyObj = obj1.GetComponent<RectTransform>();
        honeyObj.SetParent(transform);
        honeyObj.localScale = Vector3.one;
        honeyObj.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public void SpawnHoney(Vector3 spawnPos)
    {
        honeyObj.gameObject.SetActive(true);
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
        honeyObj.position = WorldToCanvasPosition(uiCanvas, honeyObj, Camera.main, spawnPos);


        honeyObj.DOMove(targetRoot.position, 0.35f).OnComplete(() =>
        {
            honeyObj.gameObject.SetActive(false);
            GameManager.instance.IncreaseHoneyCount();
        });
    }

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }
}
