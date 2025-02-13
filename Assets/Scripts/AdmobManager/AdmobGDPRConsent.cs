using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Api;

public class AdmobGDPRConsent : MonoBehaviour
{
    public static AdmobGDPRConsent instance;

    public bool canRequestAd;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        canRequestAd = ConsentInformation.CanRequestAds();

        AdmobGDPRConsent.instance.GatherConsent((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed Gathering Because of " + error);
            }
        });
    }

    public void GatherConsent(Action<string> onComplete)
    {
        var requestParameter = new ConsentRequestParameters
        {
            // TODO:  Add Debug Settings
            ConsentDebugSettings = new ConsentDebugSettings
            {
                DebugGeography = DebugGeography.EEA,
                TestDeviceHashedIds = new()
                {
                    "TEST-DEVICE-HASED-ID"
                }
            }
        };

        ConsentInformation.Update(requestParameter, (FormError updateError) =>
        {
            if (updateError != null)
            {
                onComplete(updateError.Message);
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
            {
                onComplete?.Invoke(showError?.Message);
            });
        });
    }

    

}


