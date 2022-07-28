using System.Collections;
using Model;
using Model.Interfaces;
using MonoBehaviours;
using NUnit.Framework;
using ScriptableObjects.Agent;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours
{
    public class GruntTests
    {
        // Intended to provide nav mesh surface
        private const string TestPlatform = "Prefabs/Environment/Test Combat Platform";
        private readonly Vector3 _testLocation = new Vector3();
        private Transform _testVolume;

        [SetUp]
        public void CreateTestVolume()
        {
            _testVolume = new GameObject { transform = { position = _testLocation } }.transform;
            Object.Instantiate(Resources.Load<GameObject>(TestPlatform), _testVolume);
        }

        [UnityTest]
        public IEnumerator Grunts_CanAcquireTargets()
        {
            var sut = new GameObject { transform = { parent = _testVolume } }.AddComponent<Grunt>();
            var eventCalled = false;
            sut.TargetAcquired += _ => eventCalled = true;
            var testTarget = new GameObject
            { transform = { position = sut.transform.position + new Vector3(0, 0, 10) } };
            yield return null;

            sut.SetTarget(new Target(testTarget.transform));

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator Grunts_TargetEnemiesWithinRange()
        {
            var sut = new GameObject { transform = { parent = _testVolume } }.AddComponent<Grunt>();
            // TODO: make IAgentConfiguration on Grunt public, and use NSub instead of creating an SO
            IAgentConfiguration referenceAgentConfig = ScriptableObject.CreateInstance<AgentConfiguration>();
            var eventCalled = false;
            sut.TargetAcquired += _ => eventCalled = true;
            var testTarget = new GameObject
            {
                transform = { position = sut.transform.position + new Vector3(0, 0, referenceAgentConfig.DetectRange - 1) },
                layer = referenceAgentConfig.EnemyLayer
            };
            testTarget.AddComponent<SphereCollider>();
            yield return null;

            Assert.IsTrue(eventCalled);
        }

        [UnityTest]
        public IEnumerator Grunts_LoseTargetsOnceOutOfRange()
        {
            var sut = new GameObject { transform = { parent = _testVolume } }.AddComponent<Grunt>();
            // TODO: make IAgentConfiguration on Grunt public, and use NSub instead of creating an SO
            IAgentConfiguration referenceAgentConfig = ScriptableObject.CreateInstance<AgentConfiguration>();
            var eventCalled = false;
            sut.TargetLost += () => eventCalled = true;
            var testTarget = new GameObject
            {
                transform = { position = sut.transform.position + new Vector3(0, 0, referenceAgentConfig.DetectRange - 1) },
                layer = referenceAgentConfig.EnemyLayer
            };
            testTarget.AddComponent<SphereCollider>();
            yield return null;
            testTarget.transform.position += new Vector3(0, 0, 2);
            yield return null;

            Assert.IsTrue(eventCalled);
        }
    }
}