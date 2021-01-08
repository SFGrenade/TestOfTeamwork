using System;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    class PatchSobParticleSystem : MonoBehaviour
    {
        public void Start()
        {
            try
            {
                PrefabHolder.popSobPartPrefab.SetActive(true);
                ParticleSystemRenderer tmpPSR = PrefabHolder.popSobPartPrefab.GetComponentInChildren<ParticleSystemRenderer>();
                foreach (ParticleSystemRenderer psr in GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    if ((psr.gameObject != null) && psr.gameObject.activeInHierarchy)
                    {
                        //// ToDo need to make particles round
                        //psr.material = loreMaterial;
                        // Renderer stuff
                        psr.sharedMaterial = tmpPSR.sharedMaterial;
                        psr.material = tmpPSR.material;
                        psr.realtimeLightmapScaleOffset = tmpPSR.realtimeLightmapScaleOffset;
                        psr.lightmapScaleOffset = tmpPSR.lightmapScaleOffset;
                        psr.realtimeLightmapIndex = tmpPSR.realtimeLightmapIndex;
                        psr.lightmapIndex = tmpPSR.lightmapIndex;
                        psr.probeAnchor = tmpPSR.probeAnchor;
                        psr.lightProbeProxyVolumeOverride = tmpPSR.lightProbeProxyVolumeOverride;
                        psr.allowOcclusionWhenDynamic = tmpPSR.allowOcclusionWhenDynamic;
                        psr.sortingOrder = tmpPSR.sortingOrder;
                        psr.sortingLayerID = tmpPSR.sortingLayerID;
                        psr.sortingLayerName = tmpPSR.sortingLayerName;
                        psr.reflectionProbeUsage = tmpPSR.reflectionProbeUsage;
                        psr.lightProbeUsage = tmpPSR.lightProbeUsage;
                        psr.motionVectorGenerationMode = tmpPSR.motionVectorGenerationMode;
                        psr.receiveShadows = tmpPSR.receiveShadows;
                        psr.shadowCastingMode = tmpPSR.shadowCastingMode;
                        psr.enabled = tmpPSR.enabled;
                        psr.materials = tmpPSR.materials;
                        psr.sharedMaterials = tmpPSR.sharedMaterials;
                        // ParticleSystemRenderer stuff
                        psr.trailMaterial = tmpPSR.trailMaterial;
                        psr.mesh = tmpPSR.mesh;
                        psr.maxParticleSize = tmpPSR.maxParticleSize;
                        psr.minParticleSize = tmpPSR.minParticleSize;
                        psr.sortingFudge = tmpPSR.sortingFudge;
                        psr.sortMode = tmpPSR.sortMode;
                        psr.pivot = tmpPSR.pivot;
                        psr.alignment = tmpPSR.alignment;
                        psr.normalDirection = tmpPSR.normalDirection;
                        psr.cameraVelocityScale = tmpPSR.cameraVelocityScale;
                        psr.velocityScale = tmpPSR.velocityScale;
                        psr.lengthScale = tmpPSR.lengthScale;
                        psr.renderMode = tmpPSR.renderMode;
                        psr.maskInteraction = tmpPSR.maskInteraction;
                    }
                }
                GlowResponse tmpGR = PrefabHolder.popSobPartPrefab.GetComponentInChildren<GlowResponse>();
                var gro = transform.GetChild(0).gameObject;
                if ((gro != null) && gro.activeInHierarchy)
                {
                    var gr = gro.AddComponent<GlowResponse>();
                    gr = gro.GetComponent<GlowResponse>();
                    gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                    gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                    gr.fadeTime = tmpGR.fadeTime;
                    gr.light = tmpGR.light;
                    gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                    gr.soundEffect = tmpGR.soundEffect;
                }
                gro = transform.GetChild(1).gameObject;
                if ((gro != null) && gro.activeInHierarchy)
                {
                    var gr = gro.AddComponent<GlowResponse>();
                    gr = gro.GetComponent<GlowResponse>();
                    gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                    gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                    gr.fadeTime = tmpGR.fadeTime;
                    gr.light = tmpGR.light;
                    gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                    gr.soundEffect = tmpGR.soundEffect;
                }
                PrefabHolder.popSobPartPrefab.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log("PatchSobParticleSystem - " + e.ToString());
            }
        }
    }
}
