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
        public bool[] gotCustomCharms = new bool[] { true, true, true, true };
        public bool[] newCustomCharms = new bool[] { false, false, false, false };
        public bool[] equippedCustomCharms = new bool[] { false, false, false, false };
        public int[] customCharmCosts = new int[] { 1, 1, 1, 1 };
#endif
    }

    public class TotGlobalSettings : ModSettings
    {
    }
}