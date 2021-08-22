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
        public const string Key = "T";
        private const string File = "TestOfTeamwork.Resources.T.png";
        public const string GcKey = "fammchild";
        private const string GcFile = "TestOfTeamwork.Resources.fammchild.png";
        public const string Gc2Key = "fammchild2";
        private const string Gc2File = "TestOfTeamwork.Resources.fammchild2.png";
        public const string PetalKey = "Petal";
        private const string PetalFile = "TestOfTeamwork.Resources.Petal.png";
        #endregion Misc

        private readonly Dictionary<string, Sprite> _dict;

        public TextureStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            _dict = new Dictionary<string, Sprite>();
            var tmpTextures = new Dictionary<string, string>();
            tmpTextures.Add(InvHornetKey, InvHornetFile);
            tmpTextures.Add(AchievementItemKey, AchievementItemFile);
            tmpTextures.Add(AchievementBossKey, AchievementBossFile);
            tmpTextures.Add(AchievementWeaverPrincessKey, AchievementWeaverPrincessFile);
            tmpTextures.Add(YKey, YFile);
            tmpTextures.Add(EKey, EFile);
            tmpTextures.Add(Key, File);
            tmpTextures.Add(GcKey, GcFile);
            tmpTextures.Add(Gc2Key, Gc2File);
            tmpTextures.Add(PetalKey, PetalFile);

            foreach (var pair in tmpTextures)
            {
                using (Stream s = asm.GetManifestResourceStream(pair.Value))
                {
                    if (s != null)
                    {
                        byte[] buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                        s.Dispose();

                        //Create texture from bytes
                        var tex = new Texture2D(2, 2);

                        tex.LoadImage(buffer, true);

                        // Create sprite from texture
                        // Split is to cut off the DreamKing.Resources. and the .png
                        _dict.Add(pair.Key, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                    }
                }
            }
        }

        public Sprite Get(string key)
        {
            return _dict[key];
        }
    }
}