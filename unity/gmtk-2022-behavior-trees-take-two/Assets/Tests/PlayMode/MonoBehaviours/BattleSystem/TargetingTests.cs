using System.Collections;
using System.Collections.Generic;
using Model.Interfaces;
using MonoBehaviours;
using MonoBehaviours.BattleSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours.BattleSystem
{
    public class TargetingTests
    {
        private readonly List<GameObject> _destroyAtEndOfTest = new List<GameObject>();

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyAtEndOfTest)
                Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireEnemies()
        {
            TargetAcquired capture = default;
            var gameObject = new GameObject();
            _destroyAtEndOfTest.Add(gameObject);
            gameObject.AddComponent<SphereCollider>();
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1<<13;
            sut.friendlyLayerMask = 1<<1;
            sut.neutralLayerMask = 1<<2;

            var testTarget = new GameObject { layer = 13 };
            _destroyAtEndOfTest.Add(testTarget);
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.enemies))
                    capture = targetAcquired;
            };

            yield return null;
            yield return null;

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireFriendlies()
        {
            TargetAcquired capture = default;
            var gameObject = new GameObject();
            _destroyAtEndOfTest.Add(gameObject);
            gameObject.AddComponent<SphereCollider>().radius = 5f;
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1 << 1;
            sut.friendlyLayerMask = 1 << 14;
            sut.neutralLayerMask = 1 << 2;

            var testTarget = new GameObject { layer = 14 };
            _destroyAtEndOfTest.Add(testTarget);
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.friendlies))
                    capture = targetAcquired;
            };

            yield return null;
            yield return null;

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireNeutrals()
        {
            TargetAcquired capture = default;
            var gameObject = new GameObject();
            _destroyAtEndOfTest.Add(gameObject);
            gameObject.AddComponent<SphereCollider>();
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1<<1;
            sut.friendlyLayerMask = 1<<2;
            sut.neutralLayerMask = 1<<15;

            var testTarget = new GameObject { layer = 15 };
            _destroyAtEndOfTest.Add(testTarget);
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.neutrals))
                    capture = targetAcquired;
            };

            yield return null;
            yield return null;

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }
    }
}