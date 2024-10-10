using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours;

[UsedImplicitly]
public class HornetRefreshPoint : MonoBehaviour
{
    [UsedImplicitly]
    private const float Cooldown = 1.0f;

    private void Awake()
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

    private IEnumerator RefreshJump()
    {
        yield break;
    }

    private IEnumerator RefreshJumpCooldown()
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