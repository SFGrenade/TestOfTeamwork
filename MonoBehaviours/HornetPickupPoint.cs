using GlobalEnums;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;

namespace TestOfTeamwork.MonoBehaviours
{
    public class HornetPickupPoint : MonoBehaviour
    {
        private static bool _moving = false;
        public Vector2[] points;
        public float speed = 0;
        public float secondsDelayBeforeInputAccepting = 0;
        private float currentSecondsDelayBeforeInputAccepting;
        private bool running;
        private HeroController heroController;
        private Rigidbody2D heroRb2d;
        private Vector2 heroGoPos;
        private int checkPointIndex;
        private MethodInfo heroResetAttacksFunction = typeof(HeroController).GetMethod("ResetAttacks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private void Start()
        {
            if (!PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkHornetCompanion"))
            {
                gameObject.SetActive(false);
            }
            else
            {
                running = false;
            }
        }

        public void OnEnable()
        {
            if (!PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkHornetCompanion"))
            {
                gameObject.SetActive(false);
            }
            else
            {
                running = false;
            }
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (points.Length <= 0 || speed == 0 || _moving || running)
                return;
            if (otherCollider.gameObject.name == "Knight")
            {
                //HeroController.instance.StartCoroutine(coroutine = MoveKnight());

                #region IEnumerator Start

                _moving = true;
                running = true;
                //Log("Moving the Knight");

                checkPointIndex = 0;
                heroController = HeroController.instance;
                heroRb2d = heroController.GetComponent<Rigidbody2D>();

                // Cancel stuff
                // Spells
                if (heroController.spellControl.ActiveStateName.Contains("Quake"))
                {
                    PlayMakerFSM.BroadcastEvent("QUAKE FALL END");
                    heroController.spellControl.SetState("Quake Finish");
                }
                Transform spellsTransform = heroController.transform.Find("Spells");
                for (int child = 0; child < spellsTransform.childCount; child++)
                {
                    spellsTransform.GetChild(child).gameObject.SetActive(false);
                }
                // Nail Arts
                heroController.EndCyclone();
                var nailArtFsm = heroController.gameObject.LocateMyFSM("Nail Arts");
                nailArtFsm.SendEvent("END");
                nailArtFsm.SendEvent("FINISHED");
                try { heroController.transform.Find("Great Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                try { heroController.transform.Find("Cyclone Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                try { heroController.transform.Find("Dash Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                // Other stuff
                heroController.CancelSuperDash();
                typeof(HeroController).GetMethod("CancelJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelDoubleJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelWallsliding", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelBackDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelDownAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelBounce", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelRecoilHorizontal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("CancelDamageRecoil", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                heroResetAttacksFunction?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("ResetAttacksDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                typeof(HeroController).GetMethod("ResetLook", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(heroController, null);
                // Animation
                var hac = heroController.GetAttr<HeroController, HeroAnimationController>("animCtrl");
                typeof(HeroAnimationController).GetMethod("Play", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hac, new object[] { "Airborne" });

                // Now we can do stuff
                heroController.IgnoreInputWithoutReset();
                heroController.ResetHardLandingTimer();
                heroController.hero_state = ActorStates.airborne;
                heroController.hero_state = ActorStates.no_input;
                heroController.AffectedByGravity(false);

                heroGoPos = gameObject.transform.position;

                currentSecondsDelayBeforeInputAccepting = 0;

                #endregion
            }
        }

        private void FixedUpdate()
        {
            if (!running) return;

            if (checkPointIndex < points.Length)
            {
                Vector2 heroPos = heroController.transform.position;
                Vector2 distance = points[checkPointIndex] + heroGoPos - heroPos;

                if (distance.magnitude <= 0.5)
                {
                    //Log("Passed Waypoint " + checkPointIndex);
                    checkPointIndex++;
                }

                distance.Normalize();
                distance.Scale(new Vector2(speed, speed));

                heroController.cState.falling = false;
                heroResetAttacksFunction?.Invoke(heroController, null);

                heroRb2d.velocity = distance;
                heroController.current_velocity = distance;
            }
            else
            {
                heroController.cState.falling = true;
                heroController.cState.doubleJumping = false;
                heroController.cState.dashing = false;
                heroController.cState.backDashing = false;
                heroController.cState.preventDash = false;
                heroController.SetAttr("doubleJump_steps", 0);
                heroController.SetAttr("dash_timer", 0.0f);
                heroController.SetAttr("airDashed", false);
                heroController.SetAttr("doubleJumped", false);
                heroController.hero_state = ActorStates.airborne;
                heroController.AffectedByGravity(true);

                if (currentSecondsDelayBeforeInputAccepting < secondsDelayBeforeInputAccepting)
                {
                    currentSecondsDelayBeforeInputAccepting += Time.fixedDeltaTime;
                }
                else
                {
                    running = false;
                    _moving = false;
                    heroController.AcceptInput();
                }
            }
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