﻿using HutongGames.PlayMaker.Actions;
using TestOfTeamwork.Consts;
using UnityEngine;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours
{
    public class WeaverPrincessBoss : MonoBehaviour
    {
        public void Start()
        {
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