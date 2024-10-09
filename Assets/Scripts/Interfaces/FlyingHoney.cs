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

    List<Vector3> arcPoint = new List<Vector3>();

    Vector3 midlePoint;

    public void SpawnHoney(Vector3 spawnPos)
    {
        if (!honeyObj.gameObject.activeInHierarchy)
        {
            honeyObj.gameObject.SetActive(true);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
            honeyObj.position = WorldToCanvasPosition(uiCanvas, honeyObj, Camera.main, spawnPos);

            arcPoint.Clear();
            midlePoint = BetweenP(honeyObj.localPosition, targetRoot.localPosition, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(honeyObj.localPosition, midlePoint, -100.0f, (float)i / 9.0f));
            }

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(midlePoint, targetRoot.localPosition, 100.0f, (float)i / 9.0f));
            }

            honeyObj.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
            {
                honeyObj.gameObject.SetActive(false);
            });
        }
        else
        {
            // GameManager.Instance.uiManager.gameView.GetStarCombo(combo);
        }
    }

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }

    Vector3 BetweenP(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
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
    }
}
