using System.Collections;
using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;

namespace TestOfTeamwork.MonoBehaviours
{
    public class HornetRefreshPoint : MonoBehaviour
    {
        private const float Cooldown = 1.0f;

        private void Start()
        {
            if (!PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkHornetCompanion"))
            {
                gameObject.SetActive(false);
            }
        }

        public void OnEnable()
        {
            if (!PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkHornetCompanion"))
            {
                gameObject.SetActive(false);
            }
        }

        public void OnDisable()
        {
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.name != "Knight") return;

            HeroController.instance.StartCoroutine(RefreshJump());

            gameObject.SetActive(false);

            HeroController.instance.StartCoroutine(RefreshJumpCooldown());
        }

        private IEnumerator RefreshJump()
        {
            var hero = HeroController.instance;

            hero.transform.Find("Can Focus Particles").gameObject.LocateMyFSM("Play").SendEvent("CAN HEAL EFFECT");
            var tmpState = GameCameras.instance.soulOrbFSM.GetState("Can Heal 2");
            var tmpAction = (HutongGames.PlayMaker.Actions.AudioPlayerOneShotSingle)tmpState.Actions[4];
            tmpAction.OnEnter();

            yield return new WaitWhile(() => hero.GetAttr<HeroController, int>("doubleJump_steps") != 0);

            hero.ResetAirMoves();
        }

        private IEnumerator RefreshJumpCooldown()
        {
            yield return new WaitForSeconds(Cooldown);

            gameObject.SetActive(true);
        }

        private void Log(string message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }

        private void Log(object message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }
    }
}