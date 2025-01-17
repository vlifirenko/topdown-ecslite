﻿using CyberNinja.Models.Ability;
using CyberNinja.Views.Projectile;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Global Unit", fileName = "GlobalUnit")]
    public class GlobalUnitConfig : ScriptableObject
    {
        public LayerMask mouseStickLookLayer;
        [Space]
        public float maxDamage = 100;
        public float abilityInputBlockTime = 0.1f;
        [Space]
        public float minLayerHit = 0.2f;
        [Space]
        public float layerWeightLerp = 0.3f;
        public float layerWeightTreshold = 0.02f;
        [Space]
        public float defaultMoveSpeed = 5f;
        public float lootRange = 7f;

        [Space]
        public float droidYSpawnPosition = 2f;
        public float droidMoveDistance = 2f;
        public float droidSpeed = 2f;
        public float lookingTargetDistance = 50f;
        public DroidProjectile droidProjectile;
        public float droidShootReloadTime = 0.25f;
        public float droidShootSpeed = 2f;
        public float droidHitDistance = 1f;
        public float droidDamage = 10f;

        [Header("Push")]
        public float pushLength = 0.5f;
        public float pushSpeed = 10f;
    }
}