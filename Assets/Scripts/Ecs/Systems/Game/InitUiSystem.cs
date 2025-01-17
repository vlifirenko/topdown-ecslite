﻿using System;
using CyberNinja.Events;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Game
{
    public class InitUiSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<CanvasView> _canvasView;

        public void Init(IEcsSystems systems)
        {
            GameEventsHolder.OnTimeUpdate += value =>
            {
                var time = TimeSpan.FromSeconds(Mathf.RoundToInt(value));

                _canvasView.Value.TimerView.TimerText.text = time.ToString(@"hh\:mm\:ss");
            };
        }
    }
}