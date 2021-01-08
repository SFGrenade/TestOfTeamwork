using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TestOfTeamwork.Consts
{
    public class LanguageStrings
    {
        #region Mechanics
        public const string HornetInvNameKey = "SFGrenadeTestOfTeamwork_HornetInvName";
        public const string HornetInvDescKey = "SFGrenadeTestOfTeamwork_HornetInvDesc";
        #endregion Mechanics
        #region Bosses
        public const string HornetMonologue1Key = "SFGrenadeTestOfTeamwork_HornetMonologue1";
        public const string HornetMonologue2Key = "SFGrenadeTestOfTeamwork_HornetMonologue2";

        public const string WeaverPrincessNameSuperKey = "SFGrenadeTestOfTeamwork_WeaverPrincessName_SUPER";
        public const string WeaverPrincessNameMainKey = "SFGrenadeTestOfTeamwork_WeaverPrincessName_MAIN";
        public const string WeaverPrincessNameSubKey = "SFGrenadeTestOfTeamwork_WeaverPrincessName_SUB";
        #endregion Bosses
        #region Places
        public const string TotAreaTitleEvent = "SFGrenadeTestOfTeamwork_TotAreaTitle";
        public const string TotAreaTitleSuperKey = "SFGrenadeTestOfTeamwork_TotAreaTitle_SUPER";
        public const string TotAreaTitleMainKey = "SFGrenadeTestOfTeamwork_TotAreaTitle_MAIN";
        public const string TotAreaTitleSubKey = "SFGrenadeTestOfTeamwork_TotAreaTitle_SUB";
        public const string LoreTabletTextKey = "SFGrenadeTestOfTeamwork_TotLoreTablet";
        #endregion Places
        #region Credits
        public const string CreditsTabletTextKey = "SFGrenadeTestOfTeamwork_TotCreditsTablet";
        #endregion Credits
        #region Achievement Strings Bosses
        public const string AchievementDefeatedWeaverPrincessTitleKey = "SFGrenadeTestOfTeamwork_Achievement_Title_DefeatedWeaverPrincess";
        public const string AchievementDefeatedWeaverPrincessTextKey = "SFGrenadeTestOfTeamwork_Achievement_Text_DefeatedWeaverPrincess";
        #endregion Achievement Strings Bosses

        private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> jsonDict;

        public LanguageStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream s = asm.GetManifestResourceStream("TestOfTeamwork.Resources.Language.json"))
            {
                if (s == null) return;

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Dispose();

                string json = System.Text.Encoding.Default.GetString(buffer);

                jsonDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            }
        }

        public string Get(string key, string sheet)
        {
            GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
            try
            {
                return jsonDict[lang.ToString()][sheet][key].Replace("<br>", "\n");
            }
            catch
            {
                return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet][key].Replace("<br>", "\n");
            }
        }

        public bool ContainsKey(string key, string sheet)
        {
            try
            {
                GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
                try
                {
                    return jsonDict[lang.ToString()][sheet].ContainsKey(key);
                }
                catch
                {
                    try
                    {
                        return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet].ContainsKey(key);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}