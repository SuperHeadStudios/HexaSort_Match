using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject adsObj;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingDuration = 3f;
    [SerializeField] private GameObject AdPanel;


    public AsyncOperation operation;

    public bool isLoadHome = false;
    bool isSeconPlay;
    private const string TermsAcceptedKey = "TermsAccepted";

    private void Start()
    {
        if (isLoadHome)
        {
            StartLoading();
            Debug.Log("calling from home loading scene");
        }

    }

    private void OnEnable()
    {
        if (!isLoadHome)
        {
            StartLoading();
        }
    }

    public void StartLoading()
    {
        isSeconPlay = PlayerPrefsHelper.GetBool("isSeconPlay", false);

        AdPanel.SetActive(false);
        StartCoroutine(LoadSceneWithProgress());

        Debug.Log("isSecondPlay" + isSeconPlay);
    }

    IEnumerator LoadSceneWithProgress()
    {
        operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / loadingDuration);

            progressBar.value = progress;
            loadingText.text = "" + (progress * 100f).ToString("F0") + "%";
            yield return null;
        }
        progressBar.value = 1f;
        loadingText.text = "100%";

        StartCoroutine(ShowAppOpenWithDelay());
        AdPanel.SetActive(true);

        while (!AppOpenAdController.instance.isLoadHome)
        {
            yield return null;
        }
        operation.allowSceneActivation = AppOpenAdController.instance.isLoadHome;

        if (isSeconPlay)
        {/*
            Debug.Log(" Should be true isSecondPlay " + isSeconPlay);
            StartCoroutine(ShowAppOpenWithDelay());
            AdPanel.SetActive(true);

            while (!AppOpenAdController.instance.isLoadHome)
            {
                yield return null;
            }
            operation.allowSceneActivation = AppOpenAdController.instance.isLoadHome;*/
        }
        else
        {
            PlayerPrefsHelper.SetBool("isSeconPlay", true);
            Debug.Log("Should be false isSecondPlay" + isSeconPlay);
            operation.allowSceneActivation = true;
        }
    }

    private IEnumerator ShowAppOpenWithDelay()
    {
        AdPanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AppOpenAdController.instance.ShowAppOpenAd();
    }
}
