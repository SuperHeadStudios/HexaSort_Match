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

    void Update()
    {

    }

    List<Vector3> arcPoint = new List<Vector3>();

    Vector3 midlePoint;

    public void SpawnGrass(Vector3 spawnPos)
    {
        if (!grassObj.gameObject.activeInHierarchy)
        {
            grassObj.gameObject.SetActive(true);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
            grassObj.position = WorldToCanvasPosition(uiCanvas, grassObj, Camera.main, spawnPos);

            arcPoint.Clear();
            midlePoint = BetweenP(grassObj.localPosition, targetRoot.localPosition, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(grassObj.localPosition, midlePoint, -100.0f, (float)i / 9.0f));
            }

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(midlePoint, targetRoot.localPosition, 100.0f, (float)i / 9.0f));
            }

            grassObj.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
            {
                grassObj.gameObject.SetActive(false);
                GameManager.instance.IncreaseGrassCount();
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
