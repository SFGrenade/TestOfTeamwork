using System.Collections.Generic;
using UnityEngine;

namespace TestOfTeamwork.Consts;

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

    private Dictionary<string, AudioClip> _dict;

    public AudioStrings(SceneChanger sc)
    {
        _dict = new Dictionary<string, AudioClip>();
        var tmpAudio = new Dictionary<string, string>();
        tmpAudio.Add(ToT01Key, ToT01File);
        tmpAudio.Add(ToT02Key, ToT02File);
        tmpAudio.Add(ToT04Key, ToT04File);
        tmpAudio.Add(ToT05Key, ToT05File);
        tmpAudio.Add(ToT06Key, ToT06File);
        tmpAudio.Add(ToT01KeyZ, ToT01FileZ);
        tmpAudio.Add(ToT02KeyZ, ToT02FileZ);
        tmpAudio.Add(ToT04KeyZ, ToT04FileZ);
        tmpAudio.Add(ToT05KeyZ, ToT05FileZ);
        tmpAudio.Add(ToT06KeyZ, ToT06FileZ);

        foreach (var pair in tmpAudio)
        {
            _dict.Add(pair.Key, sc.AbOverallMat.LoadAsset<AudioClip>(pair.Value));
            Object.DontDestroyOnLoad(_dict[pair.Key]);
        }
    }

    public AudioClip Get(string key)
    {
        return _dict[key];
    }
}