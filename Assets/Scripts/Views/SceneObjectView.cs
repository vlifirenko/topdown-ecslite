using System;
using CyberNinja.Models.Config;
using CyberNinja.Views.Core;
using UniRx;
using UnityEngine;
using UnityEngine.VFX;

namespace CyberNinja.Views
{
    public class SceneObjectView : AEntityView
    {
        [SerializeField] private MeshRenderer model;
        [SerializeField] private Collider rootCollider;
        [SerializeField] private SceneObjectConfig config;
        [SerializeField] private VisualEffect materializeVfx;
        [SerializeField] private float spawnTime = 3f;

        public SceneObjectConfig Config => config;
        public VisualEffect MaterializeVfx => materializeVfx;
        public Collider Collider => rootCollider;
        public MeshRenderer Model => model;

        protected override void Awake()
        {
            base.Awake();

            if (materializeVfx)
                PlayMaterializeVfx();
        }

        private void PlayMaterializeVfx()
        {
            model.enabled = false;

            Observable.Timer(TimeSpan.FromSeconds(spawnTime))
                .Subscribe(_ =>
                {
                    materializeVfx.enabled = false;
                    model.enabled = true;
                }).AddTo(this);
        }
    }
}