using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Model.Interfaces;
using Model.Player;
using MonoBehaviours;
using MonoBehaviours.Brains;
using MonoBehaviours.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode.Scenarios.ForMasterChief
{
    public class MasterChiefAndAGameplayCamera: InputTestFixture
    {
        private GameObject _sutPrefabInstance;
        private GameObject _testMainCameraInstance;
        private GameObject _testGameplayCameraInstance;
        private GameObject _testPlatform;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testMainCameraInstance = new GameObject();
            _testMainCameraInstance.AddComponent<Camera>();
            var brain = _testMainCameraInstance.AddComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = 0;
            _testMainCameraInstance.name = "Test main camera";
            _testMainCameraInstance.tag = "MainCamera";
            _destroyMeAtEnd.Add(_testMainCameraInstance);

            _testGameplayCameraInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player Camera"));
            foreach (var debugger in _testGameplayCameraInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            _testGameplayCameraInstance.name = "Test gameplay camera rig (vcam)";
            _destroyMeAtEnd.Add(_testGameplayCameraInstance);

            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            foreach (var debugger in _testPlatform.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            // Get nav mesh surface component and render out a nav mesh
            _destroyMeAtEnd.Add(_testPlatform);
            yield return null;
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyMeAtEnd)
                Object.Destroy(gameObject);
            _destroyMeAtEnd.Clear();
        }

        public class LocomotionTouchInput
        {
            public Vector2 PressPosition;
            public Vector2 ReleasePosition;
            public Vector3 ExpectedDesiredDirection;
            public readonly string TestMessage;
            public LocomotionTouchInput(string testMessage, Vector2 pressPosition, Vector2 releasePosition, Vector3 expectedDesiredDirection)
            {
                PressPosition = pressPosition;
                ReleasePosition = releasePosition;
                ExpectedDesiredDirection = expectedDesiredDirection;
                TestMessage = testMessage;
            }
        }
        private static LocomotionTouchInput[] _locomotionTouchInput = {
         new LocomotionTouchInput("Up input", Vector2.zero, Vector2.up, new Vector3(0, 0, 1f)),
         new LocomotionTouchInput("Down input", Vector2.zero, Vector2.down, new Vector3(0, 0, -1.0f)),
         new LocomotionTouchInput("Left input", Vector2.zero, Vector2.left, new Vector3(-1.0f,0,0)),
         new LocomotionTouchInput("Right input", Vector2.zero, Vector2.right, new Vector3(1f,0,0)),
        };

        [UnityTest]
        public IEnumerator MasterChief_Locomotion_TouchscreenInput([ValueSource(nameof(_locomotionTouchInput))] LocomotionTouchInput input)
        {
            var pointer = InputSystem.AddDevice<Pointer>();

            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            foreach (var debugger in _sutPrefabInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = true;
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            var virtualCamera = _testGameplayCameraInstance.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.LookAt = _sutPrefabInstance.transform;
            virtualCamera.Follow = _sutPrefabInstance.transform;
            _sutPrefabInstance.GetComponent<PlayerController>().perspectiveCamera = _testMainCameraInstance.GetComponent<Camera>();
            yield return null;

            Set(pointer.position, input.PressPosition);
            Press(pointer.press);
            yield return null;
            Set(pointer.position, input.ReleasePosition);
            yield return null;

            // master chief should now know which direction to move
            var locomotion = _sutPrefabInstance.GetComponent<ILocomotion>();
            Assert.That(locomotion.DesiredDirectionInWorld, Is.EqualTo(input.ExpectedDesiredDirection).Using(Vector3EqualityComparer.Instance), input.TestMessage);
        }

        public class MasterChiefCanShootTestCase
        {
            public readonly string TestMessage;
            public MasterChiefCanShootTestCase(string testMessage, float waitTimeInSeconds, Vector3 gruntPosition)
            {
                TestMessage = testMessage;
                WaitTimeInSeconds = waitTimeInSeconds;
                GruntPosition = gruntPosition;
            }

            public float WaitTimeInSeconds { get; }

            public Vector3 GruntPosition { get; }
        }
        private static MasterChiefCanShootTestCase[] _masterChiefCanShootTestCases = {
            new MasterChiefCanShootTestCase("shoots grunt in front of master chief", 1.0f, new Vector3(0, 0, 10)),
            new MasterChiefCanShootTestCase("turns and shoots grunt, given enough time", 1.0f, new Vector3(0, 0, -10))
        };
        [UnityTest]
        public IEnumerator MasterChief_ShootsGrunt_NoMatterOrientation([ValueSource(nameof(_masterChiefCanShootTestCases))] MasterChiefCanShootTestCase input)
        {
            var shootingTargetEventFired = false;
            var targetAcquiredEventFired = false;
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            foreach (var debugger in _sutPrefabInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = true;
            _sutPrefabInstance.GetComponent<MasterChief>().TargetAcquired += _ => targetAcquiredEventFired = true;
            _sutPrefabInstance.GetComponent<MasterChief>().ShootingTarget += _ => shootingTargetEventFired = true;
            _sutPrefabInstance.GetComponent<BehaviorTreeRunner>().config.timeBetween = 0.01f;
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            var virtualCamera = _testGameplayCameraInstance.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.LookAt = _sutPrefabInstance.transform;
            virtualCamera.Follow = _sutPrefabInstance.transform;
            _sutPrefabInstance.GetComponent<PlayerController>().perspectiveCamera = _testMainCameraInstance.GetComponent<Camera>();

            var testGruntInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Grunt (AI)"));
            testGruntInstance.GetComponent<Transform>().position = input.GruntPosition;
            _destroyMeAtEnd.Add(testGruntInstance );
            yield return null;

            yield return new WaitForSeconds(input.WaitTimeInSeconds);

            Assert.IsTrue(targetAcquiredEventFired);
            Assert.IsTrue(shootingTargetEventFired);
        }
    }
}