using UnityEngine;
using AppLovinMax;

public class GDPRConsentManager : MonoBehaviour
{
    void Start()
    {

        MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
        MaxSdk.InitializeSdk();
    }

    private void OnSdkInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
    {
        // Check if GDPR applies to the user
        if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies)
        {
            // Show your custom GDPR consent dialog here
            ShowCustomConsentDialog();
        }
        else if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply)
        {
            Debug.Log("User is not in a region that requires GDPR consent.");
        }
        else
        {
            Debug.Log("Consent dialog state unknown.");
        }
    }

    private void ShowCustomConsentDialog()
    {
        // Create your custom consent dialog UI and handle the user's response
        // When the user gives consent:
        MaxSdk.SetHasUserConsent(true);

        // If the user does not give consent:
        // MaxSdk.SetHasUserConsent(false);
    }
}
