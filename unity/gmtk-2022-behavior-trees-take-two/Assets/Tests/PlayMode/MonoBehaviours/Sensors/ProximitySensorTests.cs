using System.Collections;
using System.Collections.Generic;
using Model.Interfaces.Sensors;
using MonoBehaviours.Sensors;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours.Sensors
{
    public class ProximitySensorTests
    {
        private readonly List<GameObject> _destroyAtEndOfTest = new List<GameObject>();

        [TearDown]
        public void Teardown()
        {
            foreach (var gameObject in _destroyAtEndOfTest)
                Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator ProximitySensorCanBeConfiguredWithRules()
        {
            var sutGameObject = new GameObject();
            sutGameObject.AddComponent<SphereCollider>();
            var sut = sutGameObject.AddComponent<ProximitySensor>();
            sut.Configuration = Substitute.For<IProximitySensorConfiguration>();
            _destroyAtEndOfTest.Add(sut.gameObject);
            yield return null;

            Assert.NotNull(sut);
        }

        [UnityTest]
        public IEnumerator ProximitySensorCanSenseFriendlies()
        {
            var sutGameObject = new GameObject();
            sutGameObject.AddComponent<SphereCollider>();
            var sut = sutGameObject.AddComponent<ProximitySensor>();
            var testLayerInt = 13;
            sut.Configuration = Substitute.For<IProximitySensorConfiguration>();
            sut.Configuration.FriendlyLayerMask.Returns((LayerMask)(1 << testLayerInt));
            _destroyAtEndOfTest.Add(sut.gameObject);
            var dummySenseObject = new GameObject { layer = testLayerInt };
            dummySenseObject.AddComponent<SphereCollider>();
            _destroyAtEndOfTest.Add(dummySenseObject);
            yield return null;

            sut.Sense();

            Assert.AreEqual(1, sut.Friendlies.Count);
            Assert.AreSame(dummySenseObject, sut.Friendlies[0]);
        }

        [UnityTest]
        public IEnumerator ProximitySensorCanSenseEnemies()
        {
            var sutGameObject = new GameObject();
            sutGameObject.AddComponent<SphereCollider>();
            var sut = sutGameObject.AddComponent<ProximitySensor>();
            var testLayerInt = 13;
            sut.Configuration = Substitute.For<IProximitySensorConfiguration>();
            sut.Configuration.FriendlyLayerMask.Returns((LayerMask)(1 << 1));
            sut.Configuration.EnemyLayerMask.Returns((LayerMask)(1 << testLayerInt));
            _destroyAtEndOfTest.Add(sut.gameObject);

            var dummySenseObject = new GameObject { layer = testLayerInt };
            dummySenseObject.AddComponent<SphereCollider>();
            _destroyAtEndOfTest.Add(dummySenseObject);
            yield return null;

            sut.Sense();

            Assert.AreEqual(1, sut.Enemies.Count);
            Assert.AreSame(dummySenseObject, sut.Enemies[0]);
        }

        [UnityTest]
        public IEnumerator ProximitySensorCanSenseCover()
        {
            var sutGameObject = new GameObject();
            sutGameObject.AddComponent<SphereCollider>();
            var sut = sutGameObject.AddComponent<ProximitySensor>();
            var testLayerInt = 13;
            sut.Configuration = Substitute.For<IProximitySensorConfiguration>();
            sut.Configuration.CoverLayerMask.Returns((LayerMask)(1 << testLayerInt));
            _destroyAtEndOfTest.Add(sut.gameObject);

            var dummySenseObject = new GameObject { layer = testLayerInt };
            dummySenseObject.AddComponent<SphereCollider>();
            _destroyAtEndOfTest.Add(dummySenseObject);
            yield return null;

            sut.Sense();

            Assert.AreEqual(1, sut.CoverPoints.Count);
            Assert.AreSame(dummySenseObject, sut.CoverPoints[0]);
        }
    }
}