using HutongGames.PlayMaker;
using Logger = Modding.Logger;

namespace TestOfTeamwork.Utils
{
    internal class ActualLogAction : FsmStateAction
    {
        public FsmString text;

        public override void Reset()
        {
            text = string.Empty;

            base.Reset();
        }

        public override void OnEnter()
        {
            if (!string.IsNullOrEmpty(this.text.Value))
            {
                Log($"FSM Log: \"{this.text.Value}\"");
            }
            Finish();
        }

        private new void Log(string message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }

        private void Log(object message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }
    }
}