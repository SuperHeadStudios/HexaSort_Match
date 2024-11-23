using Unity.VisualScripting;
using UnityEngine;

namespace GameSystem
{

    public static class PlayerPrefsManager
    {
        static string LEVELKEY = "LEVEL";
        static string THEMEKEY = "THEME";
        static string LEVELCOUNTERKEY = "LEVELCOUNTER";

        static string COINSKEY = "COINS";

        static string FILLKEY = "FILL";

        static string SOUNDKEY = "SOUNDS";
        static string MUSICKEY = "MUSIC";
        static string VIBEKEY = "VIBERTION";
        static string REVEIWKEY = "REVEIW";

        static string NEXTLEVEL = "NEXTLEVEL";

        static string FIXLEVEL = "FIXLEVELNUMBER";
        static string SEASON = "SEASONNUMBER";

        static string PASSPOWERUP = "PASSPOWERUP";
        static string ROTATEPOWERUP = "ROTATEPOWERUP";
        static string NOADS = "NOADS";

        //Dailyreward
        static string PREVIOUSDAY = "PREVIOUSDAY";
        static string ISGIFTCOLLECTED = "ISGIFTCOLLECTED";

        static string PLAYERNAME = "PLAYERNAME";
        static string PLAYERFRAME = "PLAYERFRAME";
        static string PLAYERICON = "PLAYERICON";

        static string PLAYERPLANE = "SELECTEDPLAYERPLANE";

        static string SPINCOUNT = "TOTALCOUNTFORSPIN";

        static string INTRSCOUNT = "INTERSCOUNTKEY";

        static string HAMMERUNLOCK = "HAMMERUNLOCKKEY";
        static string SWAPUNLOCK = "UNLOCKKEY";
        static string SHUFFLEUNLOCK = "SHUFFLEUNLOCKKEY";
        
        static string WOODUNLOCK = "WOODUNLOCKKEY";
        static string HONEYUNLOCK = "HONEYUNLOCKKEY";
        static string GRASSUNLOCK = "GRASSUNLOCKKEY";
        static string ICCEUNLOCK = "ICEUNLOCKKEY";
        static string VINESUNLOCK = "VINESUNLOCKKEY";

        static string WHEELSPINAVAILABLE = "WHEELSPINKEY";
        static string WHEELSPINGRPOGRESS = "WHEELPRGRESS";

        static string WHEELINDEXCOUNT = "WHEELINDEXKEY";

        #region Level Value

        // Game Levels Area
        public static int GetLevel()
        { return PlayerPrefs.GetInt(LEVELKEY, 0); }

        public static void SaveLevel(int level)
        { PlayerPrefs.SetInt(LEVELKEY, level); }

        // Game Theme Area
        public static int GetThemeIndex()
        { return PlayerPrefs.GetInt(THEMEKEY, 0); }

        public static void SaveThemeIndex(int index)
        { PlayerPrefs.SetInt(THEMEKEY, index); }

        // Game Level Counter Area
        public static int GetLevelCounterIndex()
        { return PlayerPrefs.GetInt(LEVELCOUNTERKEY, 0); }

        public static void SaveLevelCounterIndex(int levelIndex)
        { PlayerPrefs.SetInt(LEVELCOUNTERKEY, levelIndex); }

        #endregion


        #region FruitFillvalue

        // Wheel Spin Count

        public static float GetFillAmount()
        { return PlayerPrefs.GetFloat(FILLKEY, 1f); }

        public static void SaveFillAmount(float fillValue)
        { PlayerPrefs.SetFloat(FILLKEY, fillValue); }

        #endregion


        #region Main Coins Values

        // Game Coins
        public static int GetCoins()
        { return PlayerPrefs.GetInt(COINSKEY, 0); }

        public static void SaveCoins(int coinsAmount)
        { PlayerPrefs.SetInt(COINSKEY, coinsAmount); }

        public static int GetSpinCount()
        { return PlayerPrefs.GetInt(SPINCOUNT, 0); }

        public static void SaveSpinCount(int spincount)
        { PlayerPrefs.SetInt(SPINCOUNT, spincount); }

        #endregion


        #region All Bool States

        /* ============= *** GAME SOUNDS *** ============= */

        // Game Sounds Area
        public static bool GetSoundState()
        {
            return GetBool(SOUNDKEY, true);
        }

        public static void SetSoundState(bool isMuted)
        {
            SaveBool(SOUNDKEY, isMuted);
        }

        // Game Sounds Area
        public static bool GetMusicState()
        {
            return GetBool(MUSICKEY, true);
        }

        public static void SetMusicState(bool isMuted)
        {
            SaveBool(MUSICKEY, isMuted);
        }

        // Game Sounds Area
        public static bool GetVibrateState()
        {
            return GetBool(VIBEKEY, true);
        }

        public static void SetVibrateState(bool isMuted)
        {
            SaveBool(VIBEKEY, isMuted);
        }


        /* ============= *** GAME REVIEW *** ============= */

        // Game Sounds Area
        public static int GetReviewState()
        { return PlayerPrefs.GetInt(REVEIWKEY); }

        public static void SaveReviewState(int amount)
        { PlayerPrefs.SetInt(REVEIWKEY, amount); }

        public static bool GetNextLevelState(bool defaultValue = false)
        {
            int defaultIntValue = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(NEXTLEVEL, defaultIntValue) == 1;
        }

        public static int GetFixLevelnumber()
        {
            return PlayerPrefs.GetInt(FIXLEVEL, 1);
        }

        public static void SetFixLevelnumber()
        {
            PlayerPrefs.SetInt(FIXLEVEL, (PlayerPrefs.GetInt(FIXLEVEL, 1) + 1));
            PlayerPrefs.Save();
        }

        public static int GetSeasonnumber()
        {
            return PlayerPrefs.GetInt(SEASON, 0);
        }

        public static void SetSeasonnumber()
        {
            PlayerPrefs.SetInt(SEASON, (PlayerPrefs.GetInt(SEASON, 0) + 1));
            PlayerPrefs.Save();
        }

        public static int GetPassPowerup()
        { return PlayerPrefs.GetInt(PASSPOWERUP, 3); }

        public static void SavePassPwerup(int count)
        { PlayerPrefs.SetInt(PASSPOWERUP, count); }

        public static int GetRotatwPowerup()
        { return PlayerPrefs.GetInt(ROTATEPOWERUP, 3); }

        public static void SaveRotatePwerup(int count)
        { PlayerPrefs.SetInt(ROTATEPOWERUP, count); }



        public static int GetNoAds()
        { return PlayerPrefs.GetInt(NOADS, 0); }

        public static void SetNoAds()
        { PlayerPrefs.SetInt(NOADS, 1); }

        #endregion

        #region DAILY REWARDS

        public static bool GetUpcomingReward()
        {
            return PlayerPrefs.GetInt(ISGIFTCOLLECTED, 0) == 0;
        }

        public static void SetUpcomingReward(bool collected)
        {
            PlayerPrefs.SetInt(ISGIFTCOLLECTED, collected ? 0 : 1);
            Debug.Log(PlayerPrefs.GetInt(ISGIFTCOLLECTED, 0));
            PlayerPrefs.Save();
        }

        public static string Getpreviousday()
        {
            return PlayerPrefs.GetString(PREVIOUSDAY, " ");
        }

        public static void SetPreviousday(string pass)
        {
            PlayerPrefs.SetString(PREVIOUSDAY, pass);
        }

        #endregion

        #region PlayerData

        public static string GetPlayername()
        {
            return PlayerPrefs.GetString(PLAYERNAME, "Player_420");
        }
        public static void SavePlayername(string name)
        {
            PlayerPrefs.SetString(PLAYERNAME, name);
        }

        public static int GetPlayerframe()
        { return PlayerPrefs.GetInt(PLAYERFRAME, 0); }

        public static void Setplayerframe(int Framevalue)
        { PlayerPrefs.SetInt(PLAYERFRAME, Framevalue); }


        public static int GetPlayericon()
        { return PlayerPrefs.GetInt(PLAYERICON, 0); }

        public static void SetPlayericon(int iconvalue)
        { PlayerPrefs.SetInt(PLAYERICON, iconvalue); }

        public static int GetPlayerplane()
        { return PlayerPrefs.GetInt(PLAYERPLANE, 0); }

        public static void SavePlayerPlane(int value)
        { PlayerPrefs.SetInt(PLAYERPLANE, value); }


        #endregion

        #region Boost Unlock

        public static bool GetHammerUnlocked()
        {
            return GetBool(HAMMERUNLOCK, false);
        }

        public static void SetHammerUnlocked(bool state)
        {
            SaveBool(HAMMERUNLOCK, state);
        }

        public static bool GetSwapUnlocked()
        {
            return GetBool(SWAPUNLOCK, false);
        }

        public static void SetSwapUnlocked(bool state)
        {
            SaveBool(SWAPUNLOCK, state);
        }

        public static bool GetShuffleUnlocked()
        {
            return GetBool(SHUFFLEUNLOCK, false);
        }

        public static void SetShuffleUnlocked(bool state)
        {
            SaveBool(SHUFFLEUNLOCK, state);
        }

        #endregion

        
        #region Blocker Unlock

        public static bool GetWoodUnlocked()
        {
            return GetBool(WOODUNLOCK, false);
        }

        public static void SetWoodUnlocked(bool state)
        {
            SaveBool(WOODUNLOCK, state);
        }

        public static bool GetHoneyUnlocked()
        {
            return GetBool(HONEYUNLOCK, false);
        }

        public static void SetHoneyUnlocked(bool state)
        {
            SaveBool(HONEYUNLOCK, state);
        }

        public static bool GetGrassUnlocked()
        {
            return GetBool(GRASSUNLOCK, false);
        }

        public static void SetGrassUnlocked(bool state)
        {
            SaveBool(GRASSUNLOCK, state);
        }

        public static bool GetIceUnlocked()
        {
            return GetBool(ICCEUNLOCK, false);
        }

        public static void SetIceUnlocked(bool state)
        {
            SaveBool(ICCEUNLOCK, state);
        }

        public static bool GetVinesUnlocked()
        {
            return GetBool(VINESUNLOCK, false);
        }

        public static void SetVinesUnlocked(bool state)
        {
            SaveBool(VINESUNLOCK, state);
        }

        #endregion

        public static float GetSpineProgCount()
        { return PlayerPrefs.GetFloat(WHEELSPINGRPOGRESS, 0); }

        public static void SaveSpineProgCount(float count)
        { PlayerPrefs.SetFloat(WHEELSPINGRPOGRESS, count); }

        public static int GetSpinIndexCount()
        { return PlayerPrefs.GetInt(WHEELINDEXCOUNT, 0); }

        public static void SaveSpinIndexCount(int index)
        { PlayerPrefs.SetInt(WHEELINDEXCOUNT, index); }


        public static bool GetIsSpin()
        {
            return GetBool(WHEELSPINAVAILABLE, false); 
        }

        public static void SaveIsSpin(bool spineAavailable)
        {
            SaveBool(WHEELSPINAVAILABLE, spineAavailable);
        }

        // Game Level Counter Area
        public static int GetIntrsAdCount()
        { return PlayerPrefs.GetInt(INTRSCOUNT, 0); }

        public static void SaveLevelIntrsCount(int count)
        { PlayerPrefs.SetInt(INTRSCOUNT, count); }


        public static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }


        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

    }
}