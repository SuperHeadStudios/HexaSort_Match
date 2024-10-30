using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public float cellSpacing;

    public float XtileDistance;

    public float ZtileDistance;

    public MapDataLevelConfigSO levelConfig;

    public GameObject prefab_winStreakPower;

    public Transform cellHolder;

    public const int maxCountOfMapFile = 174;

    private int maxRow;

    public int goalNumber;

    public int currentGoalNumber;

    //blockers

    public int woodGoalNumber;

    public int honeyGoalNumber;

    public int grassGoalNumber;

    public int currentWoodGoalNumber;

    public int currentHoneyGoalNumber;

    public int currentGrassGoalNumber;

    public int widthOfMap;

    public int heighOfMap;

    public int currentMapSlots;

    public List<BottomCell> bottomCellList;

    public bool isBlockers;


    [SerializeField] private bool isDebug = false;
    [SerializeField] private int levelNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        //InitBoardGenerator();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitBoardGenerator()
    {
        currentGoalNumber = 0;
        currentWoodGoalNumber = 0;
        currentHoneyGoalNumber = 0;
        currentGrassGoalNumber = 0;
        goalNumber = 0;
        woodGoalNumber = 0;
        honeyGoalNumber = 0;
        grassGoalNumber = 0;
        widthOfMap = 0;
        maxRow = 0;
        GenMap();
    }

    public void GenMap()
    {
        if (isDebug)
        {
            levelConfig = Resources.Load("levels/map_" + levelNum.ToString()) as MapDataLevelConfigSO;
        }
        else
        {
            levelConfig = Resources.Load("levels/map_" + GameManager.instance.levelIndex.ToString()) as MapDataLevelConfigSO;
        }   

        bottomCellList = new List<BottomCell>();

        for (int i = 0; i < levelConfig.LevelData.Cells.Count; i++)
        {
            BottomCell bottomCell = GameManager.instance.poolManager.GetBottomCell();
            bottomCell.transform.SetParent(transform);
            bottomCell.row = levelConfig.LevelData.Cells[i].Row;
            bottomCell.column = levelConfig.LevelData.Cells[i].Col;
            bottomCell.cost = levelConfig.LevelData.Cells[i].Cost;
           
            int oddColumn = levelConfig.LevelData.Cells[i].Col % 2;
            if (oddColumn == 0)
            {
                bottomCell.transform.localPosition = new Vector3(levelConfig.LevelData.Cells[i].Col * XtileDistance, 0.0f,
                    levelConfig.LevelData.Cells[i].Row * ZtileDistance);
            }
            else
            {
                bottomCell.transform.localPosition = new Vector3(levelConfig.LevelData.Cells[i].Col * XtileDistance, 0.0f,
                        levelConfig.LevelData.Cells[i].Row * ZtileDistance + ZtileDistance * 0.5f);
            }

            if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.RV)
            {
                bottomCell.InitAdCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Wood)
            {
                bottomCell.InitWoodCell(true);

            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Grass)
            {
                bottomCell.InitGrassCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Honey)
            {
                bottomCell.InitHoneyCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Ice)
            {
                bottomCell.InitIceCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Vines)
            {
                bottomCell.InitVinesCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.Lock)
            {
                bottomCell.InitLockCell(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.PreFilled)
            { 
                bottomCell.InitPrefilled(true);
            }
            else if (levelConfig.LevelData.Cells[i].State == EnumStateOfBottomCell.RandomPrefiled)
            {
                bottomCell.InitRandomrefilled(true);
            }
            else
            {
                bottomCell.InitAdCell(false);
                bottomCell.InitWoodCell(false);
                bottomCell.InitGrassCell(false);
                bottomCell.InitHoneyCell(false);
                bottomCell.InitIceCell(false);
                bottomCell.InitVinesCell(false);
                bottomCell.InitLockCell(false);
                bottomCell.InitRandomrefilled(false);
                bottomCell.InitPrefilled(false);
            }

            bottomCell.CreateColumn();
            bottomCellList.Add(bottomCell);

            if (Mathf.Abs(bottomCell.column) > widthOfMap)
            {
                widthOfMap = Mathf.Abs(bottomCell.column);
            }

            if (Mathf.Abs(bottomCell.row) > heighOfMap)
            {
                heighOfMap = Mathf.Abs(bottomCell.row);
            }
            
            bottomCell.CheckNearOnStart();
        }

        SetCam();
        goalNumber = levelConfig.Goals[0].Target;
        if (isBlockers == true)
        {
            woodGoalNumber = levelConfig.Goals[1].Target;
            honeyGoalNumber = levelConfig.Goals[2].Target;
            grassGoalNumber = levelConfig.Goals[3].Target;
        }
        currentMapSlots = levelConfig.LevelData.Cells.Count;
        GameManager.instance.uiManager.gameView.GoallbarShow();
    }

    public void ClearMap()
    {
        for (int i = 0; i < bottomCellList.Count; i++)
        {
            bottomCellList[i].hexaColumn.ClearAllElements();
            bottomCellList[i].hexaColumn.cellHoder = null;
            bottomCellList[i].hexaColumn.currentBottomCell = null;
            GameManager.instance.poolManager.RemoveHexaColumn(bottomCellList[i].hexaColumn);

            bottomCellList[i].hexaColumn = null;
            GameManager.instance.poolManager.RemoveBottomCell(bottomCellList[i]);
        }
        bottomCellList.Clear();
    }

    private void SetCam()
    {
        if (widthOfMap == 2)
        {
            Camera.main.orthographicSize = 11;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -7.5f);
        }
        else if (widthOfMap == 3)
        {
            Camera.main.orthographicSize = 14;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -8.5f);
        }
        else
        {
            Camera.main.orthographicSize = 11;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -7.5f);
        }


        if (heighOfMap == 2)
        {
            Camera.main.orthographicSize = 11;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -7.5f);
        }
        else if (heighOfMap == 3)
        {
            Camera.main.orthographicSize = 13;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -9.25f);
        }
        else
        {
            Camera.main.orthographicSize = 11;
            cellHolder.transform.localPosition = new Vector3(0.0f, 0.0f, -7.5f);
        }
    }
}
