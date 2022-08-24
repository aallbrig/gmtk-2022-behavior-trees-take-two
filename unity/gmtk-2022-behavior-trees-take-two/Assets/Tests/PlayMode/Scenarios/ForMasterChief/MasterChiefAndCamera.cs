using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForMasterChief
{
    public class MasterChiefAndCamera
    {
        private GameObject _sutPrefabInstance;
        private GameObject _testCameraInstance;
        private GameObject _testPlatform;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            // Get nav mesh surface component and render out a nav mesh
            _destroyMeAtEnd.Add(_testPlatform);
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            _testCameraInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player Camera"));
            _testCameraInstance.transform.position = Vector3.forward * 10;
            _destroyMeAtEnd.Add(_testCameraInstance);
            yield return null;
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyMeAtEnd)
                Object.Destroy(gameObject);
            _destroyMeAtEnd.Clear();
        }
        [Test]
        public void MasterChiefAndCameraSimplePasses()
        {
            // Use the Assert class to test conditions.
            
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator MasterChiefAndCameraWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}