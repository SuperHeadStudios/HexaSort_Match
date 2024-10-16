using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using static AdsControl;
using UnityEngine.Advertisements;
using Unity.VisualScripting;

public class BottomCell : MonoBehaviour
{
    public static BottomCell instance;

    [HideInInspector]
    public int row;

    [HideInInspector]
    public int column;

    //[HideInInspector]
    public HexaColumn hexaColumn;

    public HexaColumn hexaColumn_ice;    

    public MeshRenderer meshRenderer;

    public Material cellMaterial;

    public Material cellSelectedMaterial;

    public Material lockMaterial;

    public GameObject AdObj;

    public bool isAd;

    public GameObject lockObj;

    public bool isLock;

    public GameObject woodObj;

    public GameObject grassObj;

    public GameObject honeyObj;

    public GameObject iceObj;

    public GameObject iceHexa;

    public bool isWood;

    public bool isGrass;

    public bool isHoney;

    public bool isIce;

    public HoneyBlocker honeyBlocker;

    public GrassBlocker grassBlocker;

    public WoodBlocker woodBlocker;

    public IceBlocker iceBlocker;

    public LockBlocker lockBlocker;

    public bool isBreakNow = false;

    public bool isUseNow;

    public bool isIceBroken;

    public BoardController boardController;

    public int index;

    [SerializeField] private float leafPos = 0.3f;

    // Start is called before the first frame update

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        boardController = transform.GetComponentInParent<BoardController>();
    }
    private void Update()
    {

    }

    public void UpdateLeafPosition()
    {
        Vector3 leafposition = grassBlocker.centerLeaf.position;
        leafposition.y = hexaColumn.hexaCellList.Count - 1 * leafPos;
    }


    public void CreateColumn()
    {
        if (isIce)
        {
            hexaColumn = GameManager.instance.poolManager.GetHexaColumn();
            hexaColumn.InitColumn();
            hexaColumn.transform.SetParent(transform);
            hexaColumn.transform.localPosition = Vector3.zero;
            hexaColumn.currentBottomCell = this;

        }
        else
        {
            hexaColumn = GameManager.instance.poolManager.GetHexaColumn();
            hexaColumn.InitColumn();
            hexaColumn.transform.SetParent(transform);
            hexaColumn.transform.localPosition = Vector3.zero;
            hexaColumn.currentBottomCell = this;
        }
        
    }

    public void InitAdCell(bool isAdCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isAd = isAdCell;
        if (isAd)
            AdCell();
        else
            OpenCell();
    }
    public void InitLockCell(bool isLockCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isLock = isLockCell;
        if (isLock)
            LockCell();
        else
            OpenLockCell();
    }


    public void InitWoodCell(bool isWoodCell)
    {   
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isWood = isWoodCell;
        if (isWood)
            WoodCell();
        else
            WoodCellOpen();
    }

    public void InitGrassCell(bool isGrassCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isGrass = isGrassCell;
        if (isGrass)
            GrassCell();
        else
            GrassCellOpen();
    }
    public void InitHoneyCell(bool isHoneyCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isHoney = isHoneyCell;
        if (isHoney)
            HoneyCell();
        else
            HoneyCellOpen();
    }
    public void InitIceCell(bool isIceCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isIce = isIceCell;
        if (isIce)
            IceCell();
        else
            IceCellOpen();
    }

    public void ClearBottomCell()
    {
        hexaColumn = null;
    }    

    public void SelectCell()
    {
        meshRenderer.material = cellSelectedMaterial;
        meshRenderer.transform.localPosition = new Vector3(0, 0.25f, 0);
    }

    public void UnSelectCell()
    {
        meshRenderer.material = cellMaterial;
        meshRenderer.transform.localPosition = new Vector3(0, 0.0f, 0);
    }

    private void AdCell()
    {
        meshRenderer.material = lockMaterial;
        AdObj.SetActive(true);
    }
    private void WoodCell()
    {
        //meshRenderer.material = lockMaterial;
        woodObj.SetActive(true);
    }

    private void GrassCell()
    {
        grassObj.SetActive(true);   
    }

    private void HoneyCell()
    {
        honeyObj.SetActive(true);
    }

    private void IceCell()
    {
        iceHexa.SetActive(true);
        iceObj.SetActive(true);
    }
    private void LockCell()
    {
        lockObj.SetActive(true);
    }
    private void OpenLockCell()
    {
        isLock = false;
        lockObj.SetActive(false);
    }

    private void OpenCell()
    {
        isAd = false;
        meshRenderer.material = cellMaterial;
        AdObj.SetActive(false);
    }

    private void WoodCellOpen()
    {
        isWood = false;
        //meshRenderer.material = cellMaterial;
        woodObj.SetActive(false);
    }
    private void GrassCellOpen()
    {
        isGrass = false;
        //meshRenderer.material = cellMaterial;
        grassObj.SetActive(false);
    }
    private void HoneyCellOpen()
    {
        isHoney = false;
        //meshRenderer.material = cellMaterial;
        honeyObj.SetActive(false);
    }
    public void IceCellOpen()
    {
        isIce = false;
        //meshRenderer.material = cellMaterial;
        iceObj.SetActive(false);
    }

    public void UnLockCell()
    {
        if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
            return;
        WatchAds();
    }

    public LayerMask bottomMask;

    public List<BottomCell> nearCellList;

    public void GetNearCells()
    {
        nearCellList.Clear();
        for(int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1.5f, bottomMask))
            {
                //Debug.Log("HIT " + hitData.transform.name);
                BottomCell nearCell = hitData.transform.GetComponent<BottomCell>();
                if (nearCell.hexaColumn.hexaCellList.Count > 0 && nearCell.hexaColumn.topColorID == hexaColumn.topColorID && nearCell.hexaColumn.topColorID != -1)
                {
                    nearCellList.Add(hitData.transform.GetComponent<BottomCell>());
                    //hitData.transform.GetComponent<BottomCell>().SelectCell();
                }
            }
        }
    }

    public void CheckNearByOnCompelteStake()
    {
        for (int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1.5f, bottomMask))
            {
                BottomCell nearCell = hitData.transform.GetComponentInChildren<BottomCell>();
                if (nearCell.isWood == true)
                {
                    StartCoroutine(nearCell.woodBlocker.MakeWoodBreak());
                }
                else if (nearCell.isIce == true)
                {
                    if (nearCell.iceBlocker.MakeIceBreak())
                    {
                        nearCell.IceCellOpen();
                        BoardController.instance.currentHexaColumn = nearCell.hexaColumn_ice;
                        BoardController.instance.currentHitBottomCell = nearCell;
                        BoardController.instance.PutColumnInHolder_2(nearCell.hexaColumn_ice, nearCell);
                        nearCell.isIce = false;
                    }
                }
                else if (nearCell.isHoney == true)
                {
                    StartCoroutine(nearCell.honeyBlocker.MakeHoneyBreak());
                }
                else if (nearCell.isLock == true)
                {
                    StartCoroutine(nearCell.lockBlocker.MakeLockOpen());
                }
            }
        }
    }

    public void CheckCurrentCellCompleteStake()
    {
        for (int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            if (Physics.Raycast(ray, 1.5f, bottomMask))
            {
                BottomCell currentCell = transform.GetComponent<BottomCell>();
                if(currentCell.isGrass == true)
                {
                    StartCoroutine(currentCell.grassBlocker.MakeGrassBreak());
                }
                
            }
        }
    }

    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnReward);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnReward);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnReward(Reward reward)
    {
        AudioManager.instance.rewardDone.Play();
        isAd = false;
        meshRenderer.material = cellMaterial;
        AdObj.SetActive(false);
    }
    /*public void RewardEarn()
    {
        isWood = false;
        meshRenderer.material = cellMaterial;
        woodObj.SetActive(false);
    }*/

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                isAd = false;
                meshRenderer.material = cellMaterial;
                AdObj.SetActive(false);
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }
        });
    }

}
