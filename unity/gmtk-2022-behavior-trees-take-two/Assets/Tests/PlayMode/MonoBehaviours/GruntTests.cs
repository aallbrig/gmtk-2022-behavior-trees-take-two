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
        [UnityTest]
        public IEnumerator Grunts_CanAcquireTargets()
        {
            var sut = new GameObject().AddComponent<Grunt>();
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
            var sut = new GameObject().AddComponent<Grunt>();
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

        // TODO: fix complaints about nav mesh agent not being on a nav mesh
        public IEnumerator Grunts_LoseTargetsOnceOutOfRange()
        {
            var sut = new GameObject().AddComponent<Grunt>();
            // TODO: make IAgentConfiguration on Grunt public, and use NSub instead of creating an SO
            IAgentConfiguration referenceAgentConfig = ScriptableObject.CreateInstance<AgentConfiguration>();
            var eventCalled = false;
            sut.TargetLost += () => eventCalled = true;
            var testTarget = new GameObject
            {
                transform = { position = sut.transform.position + new Vector3(0, 0, referenceAgentConfig.DetectRange + 1) },
                layer = referenceAgentConfig.EnemyLayer
            };
            testTarget.AddComponent<SphereCollider>();
            yield return null;

            Assert.IsTrue(eventCalled);
        }
    }
}