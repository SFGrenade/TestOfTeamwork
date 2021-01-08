using GlobalEnums;
using ModCommon.Util;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours
{
    public class HornetPickupPoint : MonoBehaviour
    {
        private static bool _moving = false;
        public Vector2[] points;
        public float speed = 0;
        public float secondsDelayBeforeInputAccepting = 0;
        private bool running;
        private IEnumerator coroutine;

        private void Start()
        {
            if (!TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkHornetCompanion)
            {
                gameObject.SetActive(false);
            }
            else
            {
                running = false;
                coroutine = null;
            }
        }

        public void OnEnable()
        {
            if (!TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkHornetCompanion)
            {
                gameObject.SetActive(false);
            }
            else
            {
                running = false;
            }
        }

        public void OnDisable()
        {
            if (coroutine != null)
            {
                HeroController.instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (points.Length <= 0 || speed == 0 || _moving || running)
                return;
            if (otherCollider.gameObject.name == "Knight")
            {
                HeroController.instance.StartCoroutine(coroutine = MoveKnight());
            }
        }

        private IEnumerator MoveKnight()
        {
            _moving = true;
            running = true;
            //Log("Moving the Knight");

            int i = 0;
            var hero = HeroController.instance;
            Rigidbody2D rb2d = hero.GetComponent<Rigidbody2D>();

            var heroResetAttacksFunction = typeof(HeroController).GetMethod("ResetAttacks", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Cancel stuff
            // Spells
            if (hero.spellControl.ActiveStateName.Contains("Quake"))
            {
                PlayMakerFSM.BroadcastEvent("QUAKE FALL END");
                hero.spellControl.SetState("Quake Finish");
            }
            Transform spellsTransform = hero.transform.Find("Spells");
            for (int child = 0; child < spellsTransform.childCount; child++)
            {
                spellsTransform.GetChild(child).gameObject.SetActive(false);
            }
            // Nail Arts
            hero.EndCyclone();
            var nailArtFsm = hero.gameObject.LocateMyFSM("Nail Arts");
            nailArtFsm.SendEvent("END");
            nailArtFsm.SendEvent("FINISHED");
            try { hero.transform.Find("Great Slash").gameObject.SetActive(false); }
            catch (Exception)
            { }

            try { hero.transform.Find("Cyclone Slash").gameObject.SetActive(false); }
            catch (Exception)
            { }

            try { hero.transform.Find("Dash Slash").gameObject.SetActive(false); }
            catch (Exception)
            { }

            // Other stuff
            hero.CancelSuperDash();
            typeof(HeroController).GetMethod("CancelJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelDoubleJump", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelWallsliding", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelBackDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelDownAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelAttack", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelBounce", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelRecoilHorizontal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("CancelDamageRecoil", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            heroResetAttacksFunction?.Invoke(hero, null);
            typeof(HeroController).GetMethod("ResetAttacksDash", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            typeof(HeroController).GetMethod("ResetLook", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hero, null);
            // Animation
            var hac = hero.GetAttr<HeroController, HeroAnimationController>("animCtrl");
            typeof(HeroAnimationController).GetMethod("Play", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(hac, new object[] { "Airborne" });

            // Now we can do stuff
            hero.IgnoreInputWithoutReset();
            hero.ResetHardLandingTimer();
            hero.hero_state = ActorStates.airborne;
            hero.hero_state = ActorStates.no_input;
            hero.AffectedByGravity(false);

            Vector2 goPos = gameObject.transform.position;

            while (i < points.Length && _moving)
            {
                Vector2 heroPos = hero.transform.position;
                Vector2 distance = points[i] + goPos - heroPos;

                if (distance.magnitude <= 0.5)
                {
                    //Log("Passed Waypoint " + i);
                    i++;
                }

                distance.Normalize();
                distance.Scale(new Vector2(speed, speed));

                hero.cState.falling = false;
                heroResetAttacksFunction?.Invoke(hero, null);

                yield return null;
                rb2d.velocity = distance;
                hero.current_velocity = distance;
            }
            //Log("Moved the Knight");
            hero.cState.falling = true;
            hero.cState.doubleJumping = false;
            hero.cState.dashing = false;
            hero.cState.backDashing = false;
            hero.cState.preventDash = false;
            hero.SetAttr("doubleJump_steps", 0);
            hero.SetAttr("dash_timer", 0.0f);
            hero.SetAttr("airDashed", false);
            hero.SetAttr("doubleJumped", false);
            hero.hero_state = ActorStates.airborne;
            hero.AffectedByGravity(true);
            running = false;

            yield return new WaitForSeconds(secondsDelayBeforeInputAccepting);
            _moving = false;
            hero.AcceptInput();
            coroutine = null;
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