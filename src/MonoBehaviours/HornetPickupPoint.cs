using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours;

[UsedImplicitly]
public class HornetPickupPoint : MonoBehaviour
{
    [UsedImplicitly]
    private static bool _moving = false;
    [UsedImplicitly]
    public Vector2[] points;
    [UsedImplicitly]
    public float speed = 0;
    [UsedImplicitly]
    public float secondsDelayBeforeInputAccepting = 0;
    [UsedImplicitly]
    private bool _running;
    [UsedImplicitly]
    private IEnumerator _coroutine;

    private void Start()
    {
    }

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
    }

    private IEnumerator MoveKnight()
    {
        yield break;
    }

    private void Log(string message)
    {
    }

    private void Log(object message)
    {
    }
}