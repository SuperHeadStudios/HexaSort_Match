using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopup : MonoBehaviour
{
    [SerializeField] private RectTransform soundOnIcon;
    
    [SerializeField] private RectTransform musicOnIcon;
    
    [SerializeField] private RectTransform vibrateOnIcon;
    

    public bool isTogle = false;


    public void SoundBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleSound(true);
            soundOnIcon.DOLocalMoveX(-80f, 0.1f);
        }
        else
        {
            AudioManager.instance.ToogleSound(false);
            soundOnIcon.DOLocalMoveX(80f, 0.1f);
        }
    }

    public void MusicBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleMusic(true);
            musicOnIcon.DOLocalMoveX(80f, 0.1f);
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicOnIcon.DOLocalMoveX(-80f, 0.1f);
        }
    }
    public void VibrateBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleHaptic(true);
            vibrateOnIcon.DOLocalMoveX(-80f, 0.1f);
        }
        else
        {
            AudioManager.instance.ToogleHaptic(false);
            vibrateOnIcon.DOLocalMoveX(80f, 0.1f);
        }
    }
}
