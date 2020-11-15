using Modding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestOfTeamwork
{
    public class TotSaveSettings : ModSettings
    {
        // Start Mod Quest
        public bool SFGrenadeTestOfTeamworkStartQuest { get => GetBool(false); set => SetBool(value); }

        // Mechanics
        public bool SFGrenadeTestOfTeamworkHornetCompanion { get => GetBool(false); set => SetBool(value); }

        // Bosses
        public bool SFGrenadeTestOfTeamworkEncounterBeforeBoss { get => GetBool(false); set => SetBool(value); }
        public bool SFGrenadeTestOfTeamworkDefeatedWeaverPrincess { get => GetBool(false); set => SetBool(value); }

        // Areas
        public bool SFGrenadeTestOfTeamworkTotOpened { get => GetBool(false); set => SetBool(value); }
        public bool SFGrenadeTestOfTeamworkVisitedTestOfTeamwork { get => GetBool(false); set => SetBool(value); }
        public bool SFGrenadeTestOfTeamworkTotOpenedShortcut { get => GetBool(false); set => SetBool(value); }
        public bool SFGrenadeTestOfTeamworkTotOpenedTotem { get => GetBool(false); set => SetBool(value); }

        //// Better charms
        //public List<bool> gotCharms = new List<bool>() { true, true, true, true };
        //public List<bool> newCharms = new List<bool>() { false, false, false, false };
        //public List<bool> equippedCharms = new List<bool>() { false, false, false, false };
        //public List<int> charmCosts = new List<int>() { 1, 1, 1, 1 };
    }

    public class TotGlobalSettings : ModSettings
    {
        public TextureData SpriteData = new TextureData();
    }
    [Serializable]
    public class SpriteDefinition
    {
        public string SpriteName = "Test";
        [JsonConverter(typeof(SFCore.ReflectionConverter<Rect>))]
        public Rect Position = new Rect(1, 2, 3, 4);
    }
    [Serializable]
    public class FrameData
    {
        public int Id = 5;
        public bool HasEvent = false;
        public string EventInfo = "6";
        public int EventInt = 7;
        public float EventFloat = 8.1f;
    }
    [Serializable]
    public class ClipData
    {
        public string clipname = "9";
        public float fps = 10.3f;
        public List<FrameData> frames = new List<FrameData>(new FrameData[] { new FrameData() });
    }
    [Serializable]
    public class TextureData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public tk2dBaseSprite.Anchor Anchor = tk2dBaseSprite.Anchor.MiddleCenter;
        [JsonConverter(typeof(SFCore.ReflectionConverter<Vector2>))]
        public Vector2 HeroSize = Vector2.down;
        [JsonConverter(typeof(SFCore.ReflectionConverter<Vector2>))]
        public Vector2 offset = Vector2.left;
        public List<SpriteDefinition> Definitions = new List<SpriteDefinition>(new SpriteDefinition[] { new SpriteDefinition() });
        public List<ClipData> Clips = new List<ClipData>(new ClipData[] { new ClipData() });
    }
}