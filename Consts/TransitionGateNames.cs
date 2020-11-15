namespace TestOfTeamwork.Consts
{
    internal struct TransitionGateNames
    {
        #region Scene Names
        public const string Rt = "Room_temple";
        public const string Wp06 = "White_Palace_06";
        public const string Tot = "SF_ToT_ToT_";
        public const string Tot01 = Tot + "01";
        public const string Tot02 = Tot + "02";
        public const string Tot03 = Tot + "03";
        public const string TotEndless = Tot + "Endless";
        #endregion Scene Names
        #region Gateway Names
        public const string Wp06Tot01 = "sf_tot_right1";
        public const string Tot01Wp06 = "sf_tot_left1";
        //public const string wp06_tot01_shortcut = "sf_tot_right2"; // Impossible due to GroundFinding, Custom SceneLoadVisualizations when?
        //public const string tot01_wp06_shortcut = "sf_tot_left2"; // Impossible due to GroundFinding, Custom SceneLoadVisualizations when?
        public const string Tot01Tot02Shortcut = "sf_tot_top2";
        public const string Tot02Tot01Shortcut = "sf_tot_bot2";
        public const string Tot01Tot03 = "sf_tot_top1";
        public const string Tot03Tot01 = "sf_tot_bot1";
        public const string Tot02Tot03 = "sf_tot_right1";
        public const string Tot03Tot02 = "sf_tot_left1";
        #endregion Gateway Names
    }
}