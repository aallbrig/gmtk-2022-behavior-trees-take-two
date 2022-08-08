using System.Collections;
using Model.Interfaces;
using MonoBehaviours;
using MonoBehaviours.BattleSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours.BattleSystem
{
    public class TargetingTests
    {
        private readonly Vector3 _testLocation = new Vector3(30f, 30f, 30f);
        private readonly Vector3 _volumeOffset = new Vector3(0, 0, 30f);
        private int _testVolumeIndex = 0;

        private Transform NewTestVolume()
        {
            return new GameObject { transform = { position = _testLocation + (_volumeOffset * _testVolumeIndex++) } }.transform;
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireEnemies()
        {
            TargetAcquired capture = default;
            var testVolume = NewTestVolume();
            var gameObject = new GameObject { transform = { parent = testVolume } };
            gameObject.AddComponent<SphereCollider>();
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1<<13;
            sut.friendlyLayerMask = 1<<1;
            sut.neutralLayerMask = 1<<2;


            var testTarget = new GameObject { layer = 13, transform = { parent = testVolume } };
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.enemies))
                    capture = targetAcquired;
            };

            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireFriendlies()
        {
            TargetAcquired capture = default;
            var testVolume = NewTestVolume();
            var gameObject = new GameObject { transform = { parent = testVolume } };
            gameObject.AddComponent<SphereCollider>().radius = 5f;
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1 << 1;
            sut.friendlyLayerMask = 1 << 14;
            sut.neutralLayerMask = 1 << 2;

            var testTarget = new GameObject { layer = 14, transform = { parent = testVolume } };
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.friendlies))
                    capture = targetAcquired;
            };

            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }

        [UnityTest]
        public IEnumerator TargetingCanAcquireNeutrals()
        {
            TargetAcquired capture = default;
            var testVolume = NewTestVolume();
            var gameObject = new GameObject { transform = { parent = testVolume } };
            gameObject.AddComponent<SphereCollider>();
            var sut = gameObject.AddComponent<Targeting>();
            sut.enemiesLayerMask = 1<<1;
            sut.friendlyLayerMask = 1<<2;
            sut.neutralLayerMask = 1<<15;

            var testTarget = new GameObject { layer = 15, transform = { parent = testVolume } };
            testTarget.AddComponent<SphereCollider>();
            var testBattleAgent = testTarget.AddComponent<BattleAgent>();

            sut.TargetAcquired += targetAcquired =>
            {
                if (Equals(targetAcquired.CurrentTrackedTargets, sut.neutrals))
                    capture = targetAcquired;
            };

            yield return null;
            yield return new WaitForEndOfFrame();

            Assert.NotNull(capture);
            Assert.AreEqual(testBattleAgent.ID, capture.NewTrackedTarget.ID);
        }
    }
}