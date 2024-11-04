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
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer + Time.deltaTime < timeLoading)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = (float)timer / (float)timeLoading;
            loadingText.text = "LADING... " + loadingBar.fillAmount * 100 + "%";
        }
        else
            OpenGame();
    }

    void OpenGame()
    {
        GameManager.instance.LoadGame();
        gameObject.SetActive(false);
    }


}
