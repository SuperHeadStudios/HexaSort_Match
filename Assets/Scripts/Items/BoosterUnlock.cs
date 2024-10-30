using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUnlock : MonoBehaviour
{
    [SerializeField] private int currentLevelNum;

    [SerializeField] private RectTransform rays;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Button boosterBtn;
    [SerializeField] private GameObject btnIcon;

    [SerializeField] private Sprite lockedSprit;
    [SerializeField] private Sprite unlockSprit;
    

    private void Start()
    {
        boosterBtn.interactable = false;
        btnIcon.GetComponent<Image>().sprite = lockedSprit;
        transform.DOScale(Vector3.zero, 0.001f);
    }

    private void Update()
    {
        rays.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void UnlockBooster()
    {
        currentLevelNum = GameManager.instance.levelIndex;
        if (currentLevelNum == 2)
        {
            StartCoroutine(PopupUn());
        }
    }


    private IEnumerator PopupUn()
    {
        yield return new WaitForSeconds(.2f);
        {
            transform.DOScale(Vector3.one, 0.1f);
            boosterBtn.interactable = true;
            btnIcon.GetComponent <Image>().sprite = unlockSprit;
        }
    }


    public void HidePopup()
    {
        transform.DOScale(Vector3.zero, 0.01f);
    }


}
