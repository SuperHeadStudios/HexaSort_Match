using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TermsAndConditionsPopup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject termsPopup; 
    [SerializeField] private Button agreeButton; 
    [SerializeField] private Button termsBtn;

    [Header("Game Settings")]
    [SerializeField] private Sprite gameIconSprite;
    private const string TermsAcceptedKey = "TermsAccepted";

    private void Start()
    {
        if (PlayerPrefs.GetInt(TermsAcceptedKey, 0) == 1)
        {
            StartGame();
        }
        else
        {
            agreeButton.onClick.AddListener(OnAgreeClicked);
            termsBtn.onClick.AddListener(OpenTermsCodition);
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
        termsPopup.SetActive(false);

        gameObject.SetActive(false);
    }
}
