using System.Collections;
using System.Collections.Generic;
using Model.Interfaces.BattleSystem;
using MonoBehaviours.BattleSystem;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours
{
    public class WeaponsUserTests
    {
        private GameObject _sutGameObject;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _sutGameObject = new GameObject();
            _sutGameObject.AddComponent<WeaponsUser>();
            _destroyMeAtEnd.Add(_sutGameObject);
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
        public IEnumerator WeaponsUserIsAValidMonoBehaviourComponent()
        {
            yield return null;
            Assert.NotNull(_sutGameObject);
        }

        [UnityTest]
        public IEnumerator WeaponsUserBroadcastsNoWeaponEvent_WhenToldToFire_ButNoWeaponEquipped()
        {
            var sut = _sutGameObject.GetComponent<IWeaponsUser>();
            var eventCalled = false;
            sut.NoValidWeapon += () => eventCalled = true;

            sut.CommandFireWeapon();
            yield return null;
            Assert.IsTrue(eventCalled);
        }
    }
}