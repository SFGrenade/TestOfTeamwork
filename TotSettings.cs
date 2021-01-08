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
        public bool SFGrenadeTestOfTeamworkStartQuest = false;

        // Mechanics
        public bool SFGrenadeTestOfTeamworkHornetCompanion = false;

        // Bosses
        public bool SFGrenadeTestOfTeamworkEncounterBeforeBoss = false;
        public bool SFGrenadeTestOfTeamworkDefeatedWeaverPrincess = false;

        // Areas
        public bool SFGrenadeTestOfTeamworkTotOpened = false;
        public bool SFGrenadeTestOfTeamworkVisitedTestOfTeamwork = false;
        public bool SFGrenadeTestOfTeamworkTotOpenedShortcut = false;
        public bool SFGrenadeTestOfTeamworkTotOpenedTotem = false;

#if DEBUG_CHARMS
        // Better charms
        public List<bool> gotCharms = new List<bool>() { true, true, true, true, true, true };
        public List<bool> newCharms = new List<bool>() { false, false, false, false, false, false };
        public List<bool> equippedCharms = new List<bool>() { false, false, false, false, false, false };
        public List<int> charmCosts = new List<int>() { 1, 1, 1, 1, 1, 1 };
#endif
    }

    public class TotGlobalSettings : ModSettings
    {
    }
}