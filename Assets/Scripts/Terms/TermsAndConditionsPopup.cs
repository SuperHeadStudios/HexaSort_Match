using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TermsAndConditionsPopup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject termsPopup; 
    [SerializeField] private Image gameIcon;
    [SerializeField] private TextMeshProUGUI gameNameText; 
    [SerializeField] private Button agreeButton; 
    [SerializeField] private Button termsBtn;

    [SerializeField] private LoadingScreenManager loadingScreenManager;
    [SerializeField] private HomeScreen3D homeScreen;
    [SerializeField] private GameObject tittle;

    [Header("Game Settings")]
    [SerializeField] private Sprite gameIconSprite;
    private const string TermsAcceptedKey = "TermsAccepted";

    private void Start()
    {
        gameNameText.text = Application.productName;
        gameIcon.sprite = gameIconSprite;

        if (PlayerPrefs.GetInt(TermsAcceptedKey, 0) == 1)
        {
            StartGame();
        }
        else
        {
            loadingScreenManager.gameObject.SetActive(false);
            agreeButton.onClick.AddListener(OnAgreeClicked);
            termsBtn.onClick.AddListener(OpenTermsCodition);
            homeScreen.enabled = false;
            tittle.SetActive(false);
            termsPopup.SetActive(true);
        }
    }


    private void OnAgreeClicked()
    {
        PlayerPrefs.SetInt(TermsAcceptedKey, 1);
        StartGame();
    }

    private void OpenTermsCodition()
    {
        Application.OpenURL("https://www.superheadstudio.com/privacy-policy.php");
    }

    private void StartGame()
    {
        loadingScreenManager.gameObject.SetActive(true);
        homeScreen.enabled = true;
        tittle.SetActive(true);
        termsPopup.SetActive(false);

        gameObject.SetActive(false);
    }
}
