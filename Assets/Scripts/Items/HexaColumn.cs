using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HexaColumn : MonoBehaviour
{
    //public HexaCell hexaCellPrefab;

    public List<HexaCell> hexaCellList;

    public List<int> cellColorList;

    public int topColorID;

    public HexaColumnData currentHexaColumnData;

    private const float localSpacingY = 0.25f;

    private const float colliderHeight = 0.26f;

    public BoxCollider boxCollider;

    public BottomCell currentBottomCell;

    public CellHolder cellHoder;

    public Vector3 positionInHoler;

    public bool isSelected;

    public float offsetRaycast;

    public enum COLUMN_STATE
    {
        IDLE,
        MOVING
    };

    public COLUMN_STATE currentColumnState;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
            GetBottomCell();
    }

    public void InitColumn()
    {
        //hexaCellList = new List<HexaCell>();
        //cellColorList = new List<int>();
        boxCollider = GetComponent<BoxCollider>();
        isSelected = false;
        //currentHexaColumnData = new HexaColumnData();
        topColorID = -1;
        currentColumnState = COLUMN_STATE.IDLE;
        currentBottomCell = null;
        //AddNewHexaCell(10);
    }

    public void CreateColumn(HexaColumnData hexaColumnData)
    {
        currentHexaColumnData = hexaColumnData;

        for (int i = 0; i < hexaColumnData.columnDataList.Count; i++)
        {
            for (int j = 0; j < hexaColumnData.columnDataList[i].columnValue; j++)
            {
                HexaCell cell = GameManager.instance.poolManager.GetHexaCell();
                cell.transform.SetParent(transform);
                cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
                cell.InitCell(hexaColumnData.columnDataList[i].colorID);
                cellColorList.Add(hexaColumnData.columnDataList[i].colorID);
                hexaCellList.Add(cell);
            }
        }

        if (currentHexaColumnData.columnDataList.Count > 0)
        {
            if (currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].columnValue > 0)
                topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
            else
                topColorID = -1;
        }
        else
            topColorID = -1;
        UpdateColliderHeight();
    }

    public void Push()
    {
        int dataCount = currentHexaColumnData.columnDataList.Count;
        int topSize = currentHexaColumnData.columnDataList[dataCount - 1].columnValue;
        for (int i = 0; i < topSize; i++)
        {
            cellColorList.RemoveAt(cellColorList.Count - 1);
            hexaCellList.RemoveAt(hexaCellList.Count - 1);
        }
        currentHexaColumnData.columnDataList.RemoveAt(currentHexaColumnData.columnDataList.Count - 1);

        if (hexaCellList.Count > 0)
        {
            topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
        }
        else
        {
            topColorID = -1;
            GameManager.instance.boardController.hexaColumnsInMap.Remove(this);
        }

    }

    public void Pop(HexaColumn addColumn)
    {
        int dataCount = currentHexaColumnData.columnDataList.Count;
        int topSize = currentHexaColumnData.columnDataList[dataCount - 1].columnValue;

        int addDataCount = addColumn.currentHexaColumnData.columnDataList.Count;
        int addTopSize = addColumn.currentHexaColumnData.columnDataList[addDataCount - 1].columnValue;
        int addTopColor = addColumn.currentHexaColumnData.columnDataList[addDataCount - 1].colorID;

        for (int i = 0; i < addTopSize; i++)
        {
            cellColorList.Add(addTopColor);
            hexaCellList.Add(addColumn.hexaCellList[addColumn.hexaCellList.Count - 1 - i]);
        }

        currentHexaColumnData.columnDataList[dataCount - 1].columnValue += addTopSize;
    }

    public void AddCellColumn(HexaColumn addCellColumn)
    {
        //currentHexaColumnData = addCellColumn.currentHexaColumnData;
        currentHexaColumnData.columnDataList = new List<ColumnData>();
        for (int i = 0; i < addCellColumn.currentHexaColumnData.columnDataList.Count; i++)
        {
            currentHexaColumnData.columnDataList.Add(addCellColumn.currentHexaColumnData.columnDataList[i]);
        }
        for (int i = 0; i < addCellColumn.hexaCellList.Count; i++)
        {
            HexaCell cell = addCellColumn.hexaCellList[i];
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

       // addCellColumn.EmptyColumnData();

        UpdateColliderHeight();
        UpdateColorList();
    }


    public void AddMovingCells(HexaColumn addCellColumn)
    {
        currentHexaColumnData.columnDataList = new List<ColumnData>();

        for (int i = 0; i < addCellColumn.currentHexaColumnData.columnDataList.Count; i++)
        {
            currentHexaColumnData.columnDataList.Add(addCellColumn.currentHexaColumnData.columnDataList[i]);
        }

        for (int i = 0; i < addCellColumn.hexaCellList.Count; i++)
        {
            HexaCell cell = addCellColumn.hexaCellList[i];
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

        UpdateColliderHeight();
        UpdateColorList();
    }
/*

    public void AddNewHexaCell(int numberCell)
    {
        for (int i = 0; i < numberCell; i++)
        {
            HexaCell cell = GameManager.instance.poolManager.GetHexaCell();
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

        UpdateColliderHeight();
    }*/

    public void UpdateColliderHeight()
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(2.25f, colliderHeight * hexaCellList.Count, 2.25f);
            boxCollider.center = new Vector3(0, colliderHeight * hexaCellList.Count * 0.5f, 0);
        }
    }


    public void ExtendColliderHeight()
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1, colliderHeight * hexaCellList.Count + 3.0f, 1);

        }
    }

    public void UpdateColorList()
    {
        for (int i = 0; i < currentHexaColumnData.columnDataList.Count; i++)
        {
            for (int j = 0; j < currentHexaColumnData.columnDataList[i].columnValue; j++)
            {
                cellColorList.Add(currentHexaColumnData.columnDataList[i].colorID);
            }
        }
        topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
    }

    public void MoveBack()
    {
        currentColumnState = COLUMN_STATE.MOVING;
       /* if (cellHoder == null)
        {
            return;
        }*/
        transform.SetParent(cellHoder.transform);
        
        transform.DOLocalMove(positionInHoler, 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;
            currentColumnState = COLUMN_STATE.IDLE;
        });
    }

    public void MoveToLastBottom()
    {
        currentColumnState = COLUMN_STATE.MOVING;
        transform.SetParent(currentBottomCell.transform);
        transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;
            currentColumnState = COLUMN_STATE.IDLE;
        });

    }

    public void MoveToTarget(Vector3 targetPos)
    {
        currentColumnState = COLUMN_STATE.MOVING;
        Debug.Log("START MOVE");
        transform.DOLocalMove(targetPos, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;
            Debug.Log("FINISH MOVE");
            currentColumnState = COLUMN_STATE.IDLE;
        });
    }

    public void ClearAllElements()
    {
        if (hexaCellList.Count <= 0)
            return;

        for (int i = 0; i < hexaCellList.Count; i++)
        {
            GameManager.instance.poolManager.RemoveHexaCell(hexaCellList[i]);
        }
        //GameManager.instance.poolManager.RemoveHexaColumn(this);
        EmptyColumnData();
        //currentBottomCell = null;
       // cellHoder = null;
    }

    public void EmptyColumnData()
    {
        hexaCellList.Clear();
        currentHexaColumnData.columnDataList.Clear();
        cellColorList.Clear();
        topColorID = -1;
        UpdateColliderHeight();
    }
    public LayerMask bottomCellMaksk;
    private RaycastHit hit;
    private BottomCell hitBottomCell;

    public void GetBottomCell()
    {
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, offsetRaycast), -transform.up, out hit, 10.0f, bottomCellMaksk))
        {
            if (hit.transform.tag == "BottomCell")
            {
                if (GameManager.instance.boardController.currentHitBottomCell == null)
                {
                    //Debug.Log("FIND BOTTOM");

                    hitBottomCell = hit.transform.GetComponent<BottomCell>();
                    if (hitBottomCell.isAd)
                        return;
                    else if(hitBottomCell.isWood)
                        return;
                    else if (hitBottomCell.isHoney)
                        return;
                    else if(hitBottomCell.isIce) 
                        return;
                    else if(hitBottomCell.isVines) 
                        return;
                    else if (hitBottomCell.isLock)
                        return;
                    if (hitBottomCell.hexaColumn.hexaCellList.Count > 0)
                        return;
                    GameManager.instance.boardController.currentHitBottomCell = hitBottomCell;
                    hitBottomCell.SelectCell();
                }
                else
                {
                    if (hit.transform.gameObject != GameManager.instance.boardController.currentHitBottomCell.gameObject)
                    {
                        // Debug.Log("FIND BOTTOM");
                        hitBottomCell = hit.transform.GetComponent<BottomCell>();
                        if (hitBottomCell.isAd)
                            return;
                        else if(hitBottomCell.isWood)
                            return;
                        else if( hitBottomCell.isHoney)
                            return;
                        else if (hitBottomCell.isIce) 
                            return;
                        else if (hitBottomCell.isVines)
                            return;
                        else if (hitBottomCell.isLock)
                            return;
                        if (hitBottomCell.hexaColumn.hexaCellList.Count > 0)
                            return;

                        GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                        GameManager.instance.boardController.currentHitBottomCell = hitBottomCell;
                        hitBottomCell.SelectCell();
                        
                    }
                }
            }

            else
            {
                if (GameManager.instance.boardController.currentHitBottomCell != null)
                {
                    GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                    GameManager.instance.boardController.currentHitBottomCell = null;
                }
            }
        }

        else
        {
            if (GameManager.instance.boardController.currentHitBottomCell != null)
            {
                GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                GameManager.instance.boardController.currentHitBottomCell = null;
            }
        }

    }

}
