using CyberNinja.Ecs.Components.Room;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Ecs.Systems.Ability;
using CyberNinja.Ecs.Systems.Ai;
using CyberNinja.Ecs.Systems.Door;
using CyberNinja.Ecs.Systems.Game;
using CyberNinja.Ecs.Systems.Item;
using CyberNinja.Ecs.Systems.Player;
using CyberNinja.Ecs.Systems.Player.Droid;
using CyberNinja.Ecs.Systems.Room;
using CyberNinja.Ecs.Systems.SceneObjects;
using CyberNinja.Ecs.Systems.Ui;
using CyberNinja.Ecs.Systems.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Serialization;

namespace CyberNinja.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SceneView sceneView;
        [SerializeField] private CanvasView canvasView;
        [SerializeField] private EcsUguiEmitter uguiEmitter;
        [SerializeField] private LayersConfig layersConfig;
        [SerializeField] private GlobalUnitConfig globalUnitConfig;
        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private InputConfig inputConfig;
        [SerializeField] private GamePrefabsConfig gamePrefabsConfig;

        private EcsSystems _systems;

        private IAiService _aiService;
        private UnitService _unitService;
        private AbilityService _abilityService;
        private IDoorService _doorService;
        private VfxService _vfxService;
        private ISceneService _sceneService;
        private IGameService _gameService;
        private ITimeService _timeService;
        private ItemService _itemService;
        private PlayerService _playerService;

        private GameData _gameData;

        private void Start()
        {
            var world = new EcsWorld();

            _gameData = new GameData();

            EcsPhysicsEvents.ecsWorld = world;

            _vfxService = new VfxService(world);
            _itemService = new ItemService(world);
            _unitService = new UnitService(world, globalUnitConfig, canvasView, _vfxService, _itemService);
            _itemService.UnitService = _unitService;
            _sceneService = new SceneService(world, _unitService);
            _doorService = new DoorService(world, _unitService);
            _abilityService = new AbilityService(world, globalUnitConfig, layersConfig, _unitService, _doorService, _vfxService,
                _sceneService);
            _unitService.AbilityService = _abilityService;
            _aiService = new AiService(world, _abilityService);
            _gameService = new GameService(world, sceneView, canvasView, _gameData);
            _timeService = new TimeService();
            _playerService = new PlayerService(world);

            _systems = new EcsSystems(world);
            _systems

                // init
                .Add(new InitInputSystem())
                .Add(new InitUiSystem())

                // game
                .Add(new GameSystem())
                .Add(new CameraMovementSystem())
                .Add(new AudioSystem())
                .Add(new TimeSystem())
                
                // rooms
                .Add(new InitRoomsSystem())
                .Add(new UpdateRoomSystem())

                // trigger
                .Add(new PlayerTriggerSystem())

                // movement
                .Add(new StunSystem())
                .Add(new MovementSystem())
                .Add(new VectorLookSystem())
                .Add(new StationarySystem())
                .Add(new DashSystem())

                // player
                //.Add(new PlayerActionSystem())

                // enemy
                .Add(new OnKillEnemySystem())
                // ai
                .Add(new AiUpdateStateSystem())
                .Add(new AiIdleSystem())
                .Add(new AiChaseSystem())
                .Add(new AiAttackSystem())

                // animation
                .Add(new AnimationSystem())
                .Add(new AnimationLayersSystem())

                // other
                .Add(new AbilityInputBlockSystem())
                .Add(new HealthRegenerationSystem())
                .Add(new EnergyRegenerationSystem())
                .Add(new CalculateDamageFactorSystem())
                .Add(new ReviveSystem())
                .Add(new HealthEventSystem())
                .Add(new EnergyEventSystem())
                .Add(new DroidMovementSystem())
                //todo fix .Add(new FindTargetsSystem())
                .Add(new DroidShootSystem())

                // ability
                .Add(new InitAbilitiesSystem())
                .Add(new AbilityCooldownSystem())

                // doors
                //.Add(new InitDoorsSystem())

                // scene objects
                //.Add(new InitSceneObjectsSystem())
                .Add(new UseSceneObjectSystem())

                // items
                //.Add(new TryPickupItemService())
                .Add(new PickupSystem())
                
                // damage
                .Add(new DamageSystem())
                .Add(new PushSystem())
                .Add(new SpawnLootSystem())
                
                .Add(new CollectLootSystem())

                // ui
                .Add(new PlayerUiSystem())
                .Add(new ItemPopupSystem())
                .Add(new UiUpdateEnemyHealthSystem())
                
                //
                .DelHere<PickupComponent>()
                .DelHere<UpdateRoomComponent>()
                .DelHere<SpawnLootComponent>()
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_gameData, sceneView, canvasView)
                .Inject(layersConfig, globalUnitConfig, audioConfig, inputConfig, gamePrefabsConfig)
                .Inject(_unitService, _aiService, _abilityService, _doorService, _vfxService, _sceneService,
                    _gameService, _timeService, _itemService, _playerService)
                .Inject()
                .InjectUgui(uguiEmitter)
                .DelHerePhysics()
                .Init();
        }

        private void Update() => _systems?.Run();

        private void OnDestroy()
        {
            EcsPhysicsEvents.ecsWorld = null;
            _systems?.GetWorld()?.Destroy();
            _systems?.Destroy();
            _systems = null;

            _aiService.OnDestroy();
            _vfxService.OnDestroy();
        }
    }
}