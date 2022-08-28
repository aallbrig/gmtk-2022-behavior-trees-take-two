using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Model.Player;
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
            _testGameplayCameraInstance.name = "Test gameplay camera rig (vcam)";
            _destroyMeAtEnd.Add(_testGameplayCameraInstance);

            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
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
        [UnityTest]
        public IEnumerator MasterChiefAndCamera_Exist()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
            Assert.NotNull(_sutPrefabInstance);
            Assert.NotNull(_testGameplayCameraInstance);
            Assert.NotNull(_testPlatform);
        }

        public class LocomotionTouchInput
        {
            public Vector2 PressPosition;
            public Vector2 ReleasePosition;
            public Vector3 ExpectedDesiredDirection;
            public string TestMessage;
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
            Release(pointer.press);
            yield return null;

            // master chief should now know which direction to move
            var locomotion = _sutPrefabInstance.GetComponent<ILocomotion>();
            Assert.That(locomotion.DesiredDirectionInWorld, Is.EqualTo(input.ExpectedDesiredDirection).Using(Vector3EqualityComparer.Instance), input.TestMessage);
        }
    }
}