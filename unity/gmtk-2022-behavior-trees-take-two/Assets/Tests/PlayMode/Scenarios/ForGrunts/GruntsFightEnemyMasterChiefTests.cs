using System.Collections;
using System.Collections.Generic;
using Model.Interfaces;
using Model.Interfaces.BattleSystem;
using MonoBehaviours;
using MonoBehaviours.Brains;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForGrunts
{
    public class GruntsFightEnemyMasterChiefTests
    {
        private GameObject _sutPrefabInstance;
        private GameObject _testMasterChiefInstance;
        private GameObject _testPlatform;
        private readonly List<GameObject> _destroyMeAtEnd = new List<GameObject>();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            // Get nav mesh surface component and render out a nav mesh
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            _destroyMeAtEnd.Add(_testPlatform);
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Grunt (AI)"));
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            _testMasterChiefInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            _testMasterChiefInstance.GetComponent<BehaviorTreeRunner>().DebugEnabled = false;
            _testMasterChiefInstance.GetComponent<MasterChief>().DebugEnabled = false;
            _testMasterChiefInstance.transform.position = Vector3.forward * 10;
            _destroyMeAtEnd.Add(_testMasterChiefInstance);
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
        public IEnumerator GruntTargetsMasterChief_IfCloseEnough()
        {
            var sut = _sutPrefabInstance;
            sut.GetComponent<BehaviorTreeRunner>().config.timeBetween = 0.01f;
            var testMasterChief = _testMasterChiefInstance;
            var movingCloserToTargetEventCalled = false;
            var grunt = sut.GetComponent<Grunt>();
            grunt.MovingCloserToTarget += target =>
            {
                movingCloserToTargetEventCalled = true;
            };

            // A grunt can see master chief if he is 3m away
            sut.transform.position = Vector3.zero;
            testMasterChief.transform.position = sut.transform.position + Vector3.forward * 3;
            yield return new WaitForSeconds(1.0f);

            Assert.IsTrue(movingCloserToTargetEventCalled);
        }

        [UnityTest]
        public IEnumerator GruntMovesCloseToMasterChief_IfOutsideEffectiveWeaponRange()
        {
            var sut = _sutPrefabInstance;
            var testMasterChief = _testMasterChiefInstance;
            var weaponUser = sut.GetComponent<IWeaponsUser>();
            weaponUser.Weapon = Substitute.For<IFirearm>();
            weaponUser.Weapon.EffectiveRange.Returns(1f);
            sut.GetComponent<BehaviorTreeRunner>().config.timeBetween = 0.01f;
            var movingToMasterChief = false;
            var grunt = _sutPrefabInstance.GetComponent<Grunt>();
            grunt.MovingCloserToTarget += _ =>
            {
                movingToMasterChief = true;
            };

            // A grunt move towards master chief
            sut.transform.position = Vector3.zero;
            var masterChiefWithinRange = sut.transform.position + Vector3.forward * (weaponUser.Weapon.EffectiveRange + 1);
            testMasterChief.transform.position = masterChiefWithinRange;
            yield return new WaitForSeconds(1.0f);

            Assert.IsTrue(movingToMasterChief);
        }
    }
}