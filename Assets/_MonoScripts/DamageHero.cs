using UnityEngine;

public class DamageHero : MonoBehaviour
{
	public int damageDealt = 1;
	public RealHazardType hazardType = RealHazardType.NORMAL;
	public bool shadowDashHazard = false;
	public bool resetOnEnable = false;
}
