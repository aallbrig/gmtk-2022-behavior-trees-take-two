using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForGrunts
{
    public class GruntsFightEnemyMasterChiefTests
    {
        private const string PrefabFileLocation = "Prefabs/Grunt (AI)";
        private GameObject _sutPrefabInstance;
        private GameObject _testPlatform;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            // Get nav mesh surface component and render out a nav mesh
            _destroyMeAtEnd.Add(_testPlatform);
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>(PrefabFileLocation));
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            // allow for https://docs.unity3d.com/Manual/ExecutionOrder.html
            yield return null;
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyMeAtEnd)
                Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator GruntAndMasterChiefAreLoadableGameplayElements()
        {
            var sut = _sutPrefabInstance;
            var testMasterChief = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            _destroyMeAtEnd.Add(testMasterChief);
            // position the two gameplay objects in relevant relation to each other
            sut.transform.position = Vector3.zero;
            // Set master chief 3 meters away
            testMasterChief.transform.position = sut.transform.position + Vector3.forward * 3;
            yield return null;

            // The grunt behavior tree produces an event to move towards master chief
            Assert.NotNull(sut);
            Assert.NotNull(testMasterChief);
        }
    }
}