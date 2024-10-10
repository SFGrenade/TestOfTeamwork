using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher;

[RequireComponent(typeof(BoxCollider2D))]
class PatchInspectRegion : MonoBehaviour
{
    public string GameTextConvo = "";
    public bool HeroAlwaysLeft = false;
    public bool HeroAlwaysRight = false;

    public void Start()
    {
    }
}