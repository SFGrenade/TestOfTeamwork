using System.Collections.Generic;
using UnityEngine;

namespace TestOfTeamwork.Consts
{
    public class AudioStrings
    {
        #region BGM
        public const string ToT01Key = "ToT_01";
        private const string ToT01File = "ToT_01";
        public const string ToT02Key = "ToT_02";
        private const string ToT02File = "ToT_02_2";
        public const string ToT04Key = "ToT_04";
        private const string ToT04File = "ToT_04";
        public const string ToT05Key = "ToT_05";
        private const string ToT05File = "ToT_05_2";
        public const string ToT06Key = "ToT_06";
        private const string ToT06File = "ToT_06";
        public const string ToT01KeyZ = "ToT_01_Z";
        private const string ToT01FileZ = "ToTPiano";
        public const string ToT02KeyZ = "ToT_02_Z";
        private const string ToT02FileZ = "ToTFlute";
        public const string ToT04KeyZ = "ToT_04_Z";
        private const string ToT04FileZ = "ToTViola";
        public const string ToT05KeyZ = "ToT_05_Z";
        private const string ToT05FileZ = "ToTChoir";
        public const string ToT06KeyZ = "ToT_06_Z";
        private const string ToT06FileZ = "ToTOrgan";
        #endregion BGM

        #region Singleshots
        public const string HealSfxKey = "hk_focus_health_heal";
        private const string HealSfxFile = "hk_focus_health_heal";
        #endregion Singleshots

        private readonly Dictionary<string, AudioClip> dict;

        public AudioStrings(SceneChanger sc)
        {
            dict = new Dictionary<string, AudioClip>();
            string[] tmpAudioFiles = {
                ToT01File,
                ToT02File,
                ToT04File,
                ToT05File,
                ToT06File,
                ToT01FileZ,
                ToT02FileZ,
                ToT04FileZ,
                ToT05FileZ,
                ToT06FileZ,
                HealSfxFile
            };
            string[] tmpAudioKeys = {
                ToT01Key,
                ToT02Key,
                ToT04Key,
                ToT05Key,
                ToT06Key,
                ToT01KeyZ,
                ToT02KeyZ,
                ToT04KeyZ,
                ToT05KeyZ,
                ToT06KeyZ,
                HealSfxKey
            };
            for (int i = 0; i < tmpAudioFiles.Length; i++)
            {
                dict.Add(tmpAudioKeys[i], sc.AbOverallMat.LoadAsset<AudioClip>(tmpAudioFiles[i]));
                Object.DontDestroyOnLoad(dict[tmpAudioKeys[i]]);
            }
        }

        public AudioClip Get(string key)
        {
            return dict[key];
        }
    }
}