using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    public Image loadingBar;

    public float timeLoading;

    private float timer;

    [SerializeField] private TextMeshProUGUI loadingText;

    // Start is called before the first frame update
    void Start()
    {
        loadingBar.fillAmount = 0;
        loadingBar.DOFillAmount(1, 3).OnComplete(() =>
        {
            OpenGame();
        });
    }

    // Update is called once per frame
    void Update()
    {
        loadingText.text = "LADING... " + Mathf.RoundToInt(loadingBar.fillAmount * 100) + "%";
    }

    void OpenGame()
    {
        GameManager.instance.LoadGame();
        gameObject.SetActive(false);
    }


}
