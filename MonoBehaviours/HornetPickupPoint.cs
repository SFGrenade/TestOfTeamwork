using GlobalEnums;
using System;
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
        private float _currentSecondsDelayBeforeInputAccepting;
        private bool _running;
        private HeroController _heroController;
        private Rigidbody2D _heroRb2d;
        private Vector2 _heroGoPos;
        private int _checkPointIndex;
        private MethodInfo _heroResetAttacksFunction = typeof(HeroController).GetMethod("ResetAttacks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private void Start()
        {
            if (!PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkHornetCompanion"))
            {
                gameObject.SetActive(false);
            }
            else
            {
                _running = false;
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
                _running = false;
            }
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (points.Length <= 0 || speed == 0 || _moving || _running)
                return;
            if (otherCollider.gameObject.name == "Knight")
            {
                //HeroController.instance.StartCoroutine(coroutine = MoveKnight());

                #region IEnumerator Start

                _moving = true;
                _running = true;
                //Log("Moving the Knight");

                _checkPointIndex = 0;
                _heroController = HeroController.instance;
                _heroRb2d = _heroController.GetComponent<Rigidbody2D>();

                // Cancel stuff
                // Spells
                if (_heroController.spellControl.ActiveStateName.Contains("Quake"))
                {
                    PlayMakerFSM.BroadcastEvent("QUAKE FALL END");
                    _heroController.spellControl.SetState("Quake Finish");
                }
                Transform spellsTransform = _heroController.transform.Find("Spells");
                for (int child = 0; child < spellsTransform.childCount; child++)
                {
                    spellsTransform.GetChild(child).gameObject.SetActive(false);
                }
                // Nail Arts
                _heroController.EndCyclone();
                var nailArtFsm = _heroController.gameObject.LocateMyFSM("Nail Arts");
                nailArtFsm.SendEvent("END");
                nailArtFsm.SendEvent("FINISHED");
                try { _heroController.transform.Find("Great Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                try { _heroController.transform.Find("Cyclone Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                try { _heroController.transform.Find("Dash Slash").gameObject.SetActive(false); }
                catch (Exception)
                { }

                // Other stuff
                _heroController.CancelSuperDash();
                typeof(HeroController).GetMethod("CancelJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelDoubleJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelWallsliding", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelBackDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelDownAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelBounce", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelRecoilHorizontal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("CancelDamageRecoil", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                _heroResetAttacksFunction?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("ResetAttacksDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                typeof(HeroController).GetMethod("ResetLook", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(_heroController, null);
                // Animation
                var hac = _heroController.GetAttr<HeroController, HeroAnimationController>("animCtrl");
                typeof(HeroAnimationController).GetMethod("Play", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hac, new object[] { "Airborne" });

                // Now we can do stuff
                _heroController.IgnoreInputWithoutReset();
                _heroController.ResetHardLandingTimer();
                _heroController.hero_state = ActorStates.airborne;
                _heroController.hero_state = ActorStates.no_input;
                _heroController.AffectedByGravity(false);

                _heroGoPos = gameObject.transform.position;

                _currentSecondsDelayBeforeInputAccepting = 0;

                #endregion
            }
        }

        private void FixedUpdate()
        {
            if (!_running) return;

            if (_checkPointIndex < points.Length)
            {
                Vector2 heroPos = _heroController.transform.position;
                Vector2 distance = points[_checkPointIndex] + _heroGoPos - heroPos;

                if (distance.magnitude <= 0.5)
                {
                    //Log("Passed Waypoint " + checkPointIndex);
                    _checkPointIndex++;
                }

                distance.Normalize();
                distance.Scale(new Vector2(speed, speed));

                _heroController.cState.falling = false;
                _heroResetAttacksFunction?.Invoke(_heroController, null);

                _heroRb2d.velocity = distance;
                _heroController.current_velocity = distance;
            }
            else
            {
                _heroController.cState.falling = true;
                _heroController.cState.doubleJumping = false;
                _heroController.cState.dashing = false;
                _heroController.cState.backDashing = false;
                _heroController.cState.preventDash = false;
                _heroController.SetAttr("doubleJump_steps", 0);
                _heroController.SetAttr("dash_timer", 0.0f);
                _heroController.ResetAirMoves();
                _heroController.hero_state = ActorStates.airborne;
                _heroController.AffectedByGravity(true);

                if (_currentSecondsDelayBeforeInputAccepting < secondsDelayBeforeInputAccepting)
                {
                    _currentSecondsDelayBeforeInputAccepting += Time.fixedDeltaTime;
                }
                else
                {
                    _running = false;
                    _moving = false;
                    _heroController.AcceptInput();
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