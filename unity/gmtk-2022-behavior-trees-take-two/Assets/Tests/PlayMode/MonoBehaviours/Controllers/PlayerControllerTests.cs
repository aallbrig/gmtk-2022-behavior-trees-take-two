using System.Collections;
using System.Collections.Generic;
using MonoBehaviours.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;


namespace Tests.PlayMode.MonoBehaviours.Controllers
{
    // reference https://github.com/aallbrig/global-game-jam-2022/blob/main/unity/global-game-jam-2022/Assets/Tests/PlayMode/Input/InputManagerTests.cs
    public class PlayerControllerTests
    {
        private GameObject _sutGameObject;
        private GameObject _testPlatform;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            _destroyMeAtEnd.Add(_testPlatform);
            _sutGameObject = new GameObject();
            _sutGameObject.AddComponent<NavMeshAgent>();
            _sutGameObject.AddComponent<PlayerController>();
            _destroyMeAtEnd.Add(_sutGameObject);
            yield return null;
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyMeAtEnd)
                Object.Destroy(gameObject);
        }

    }
}