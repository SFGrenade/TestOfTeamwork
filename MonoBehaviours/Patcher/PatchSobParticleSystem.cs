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
                PrefabHolder.PopSobPartPrefab.SetActive(true);
                ParticleSystemRenderer tmpPsr = PrefabHolder.PopSobPartPrefab.GetComponentInChildren<ParticleSystemRenderer>();
                foreach (ParticleSystemRenderer psr in GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    if ((psr.gameObject != null) && psr.gameObject.activeInHierarchy)
                    {
                        //// ToDo need to make particles round
                        //psr.material = loreMaterial;
                        // Renderer stuff
                        psr.sharedMaterial = tmpPsr.sharedMaterial;
                        psr.material = tmpPsr.material;
                        psr.realtimeLightmapScaleOffset = tmpPsr.realtimeLightmapScaleOffset;
                        psr.lightmapScaleOffset = tmpPsr.lightmapScaleOffset;
                        psr.realtimeLightmapIndex = tmpPsr.realtimeLightmapIndex;
                        psr.lightmapIndex = tmpPsr.lightmapIndex;
                        psr.probeAnchor = tmpPsr.probeAnchor;
                        psr.lightProbeProxyVolumeOverride = tmpPsr.lightProbeProxyVolumeOverride;
                        psr.allowOcclusionWhenDynamic = tmpPsr.allowOcclusionWhenDynamic;
                        psr.sortingOrder = tmpPsr.sortingOrder;
                        psr.sortingLayerID = tmpPsr.sortingLayerID;
                        psr.sortingLayerName = tmpPsr.sortingLayerName;
                        psr.reflectionProbeUsage = tmpPsr.reflectionProbeUsage;
                        psr.lightProbeUsage = tmpPsr.lightProbeUsage;
                        psr.motionVectorGenerationMode = tmpPsr.motionVectorGenerationMode;
                        psr.receiveShadows = tmpPsr.receiveShadows;
                        psr.shadowCastingMode = tmpPsr.shadowCastingMode;
                        psr.enabled = tmpPsr.enabled;
                        psr.materials = tmpPsr.materials;
                        psr.sharedMaterials = tmpPsr.sharedMaterials;
                        // ParticleSystemRenderer stuff
                        psr.trailMaterial = tmpPsr.trailMaterial;
                        psr.mesh = tmpPsr.mesh;
                        psr.maxParticleSize = tmpPsr.maxParticleSize;
                        psr.minParticleSize = tmpPsr.minParticleSize;
                        psr.sortingFudge = tmpPsr.sortingFudge;
                        psr.sortMode = tmpPsr.sortMode;
                        psr.pivot = tmpPsr.pivot;
                        psr.alignment = tmpPsr.alignment;
                        psr.normalDirection = tmpPsr.normalDirection;
                        psr.cameraVelocityScale = tmpPsr.cameraVelocityScale;
                        psr.velocityScale = tmpPsr.velocityScale;
                        psr.lengthScale = tmpPsr.lengthScale;
                        psr.renderMode = tmpPsr.renderMode;
                        psr.maskInteraction = tmpPsr.maskInteraction;
                    }
                }
                GlowResponse tmpGr = PrefabHolder.PopSobPartPrefab.GetComponentInChildren<GlowResponse>();
                var gro = transform.GetChild(0).gameObject;
                if ((gro != null) && gro.activeInHierarchy)
                {
                    var gr = gro.AddComponent<GlowResponse>();
                    gr = gro.GetComponent<GlowResponse>();
                    gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                    gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                    gr.fadeTime = tmpGr.fadeTime;
                    gr.light = tmpGr.light;
                    gr.audioPlayerPrefab = tmpGr.audioPlayerPrefab;
                    gr.soundEffect = tmpGr.soundEffect;
                }
                gro = transform.GetChild(1).gameObject;
                if ((gro != null) && gro.activeInHierarchy)
                {
                    var gr = gro.AddComponent<GlowResponse>();
                    gr = gro.GetComponent<GlowResponse>();
                    gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                    gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                    gr.fadeTime = tmpGr.fadeTime;
                    gr.light = tmpGr.light;
                    gr.audioPlayerPrefab = tmpGr.audioPlayerPrefab;
                    gr.soundEffect = tmpGr.soundEffect;
                }
                PrefabHolder.PopSobPartPrefab.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log("PatchSobParticleSystem - " + e);
            }
        }
    }
}
