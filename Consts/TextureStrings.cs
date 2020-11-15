using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TestOfTeamwork.Consts
{
    public class TextureStrings
    {
        #region Inventory Items
        public const string InvHornetKey = "Inv_Hornet";
        private const string InvHornetFile = "TestOfTeamwork.Resources.Inv_Hornet.png";
        #endregion Inventory Items
        #region Bosses
        public const string WeavernPrincessKey = "WeavernPrincess";
        private const string WeavernPrincessFile = "TestOfTeamwork.Resources.WeavernPrincess.png";
        #endregion Bosses
        #region Achievements
        public const string AchievementItemKey = "Achievement_Item";
        private const string AchievementItemFile = "TestOfTeamwork.Resources.Achievement_Item.png";
        public const string AchievementBossKey = "Achievement_Boss";
        private const string AchievementBossFile = "TestOfTeamwork.Resources.Achievement_Boss.png";
        public const string AchievementWeaverPrincessKey = "Achievement_WeaverPrincess";
        private const string AchievementWeaverPrincessFile = "TestOfTeamwork.Resources.Achievement_WeaverPrincess.png";
        #endregion Achievements
        #region Misc
        public const string YKey = "Y";
        private const string YFile = "TestOfTeamwork.Resources.Y.png";
        public const string EKey = "E";
        private const string EFile = "TestOfTeamwork.Resources.E.png";
        public const string TKey = "T";
        private const string TFile = "TestOfTeamwork.Resources.T.png";
        public const string PetalKey = "Petal";
        private const string PetalFile = "TestOfTeamwork.Resources.Petal.png";
        #endregion Misc

        private readonly Dictionary<string, Sprite> dict;

        public TextureStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            dict = new Dictionary<string, Sprite>();
            string[] tmpTextureFiles = {
                InvHornetFile,
                AchievementItemFile,
                AchievementBossFile,
                AchievementWeaverPrincessFile,
                YFile,
                EFile,
                TFile,
                PetalFile
            };
            string[] tmpTextureKeys = {
                InvHornetKey,
                AchievementItemKey,
                AchievementBossKey,
                AchievementWeaverPrincessKey,
                YKey,
                EKey,
                TKey,
                PetalKey
            };
            for (int i = 0; i < tmpTextureFiles.Length; i++)
            {
                using (Stream s = asm.GetManifestResourceStream(tmpTextureFiles[i]))
                {
                    if (s == null) continue;

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    s.Dispose();

                    //Create texture from bytes
                    var tex = new Texture2D(2, 2);

                    tex.LoadImage(buffer, true);

                    // Create sprite from texture
                    // Split is to cut off the TestOfTeamwork.Resources. and the .png
                    dict.Add(tmpTextureKeys[i], Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }

        public Sprite Get(string key)
        {
            return dict[key];
        }
    }
}