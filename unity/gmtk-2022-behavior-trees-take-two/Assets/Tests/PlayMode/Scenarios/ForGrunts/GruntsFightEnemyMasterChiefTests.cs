using System.Collections;
using System.Collections.Generic;
using Model.Interfaces.BattleSystem;
using MonoBehaviours;
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
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            // Get nav mesh surface component and render out a nav mesh
            _destroyMeAtEnd.Add(_testPlatform);
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Grunt (AI)"));
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            _testMasterChiefInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
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
            var testMasterChief = _testMasterChiefInstance;
            var targetAcquired = false;
            Transform acquiredTargetCapture = null;
            var grunt = sut.GetComponent<Grunt>();
            grunt.TargetAcquired += acquiredTarget =>
            {
                acquiredTargetCapture = acquiredTarget.Transform;
                targetAcquired = true;
            };

            // A grunt can see master chief if he is 3m away
            sut.transform.position = Vector3.zero;
            testMasterChief.transform.position = sut.transform.position + Vector3.forward * 3;
            yield return null;

            Assert.IsTrue(targetAcquired);
            Assert.AreEqual(testMasterChief.transform, acquiredTargetCapture);
        }

        // TODO
        // [UnityTest]
        public IEnumerator GruntMovesCloseToMasterChief_IfOutsideEffectiveWeaponRange()
        {
            var sut = _sutPrefabInstance;
            var testMasterChief = _testMasterChiefInstance;
            var weaponUser = sut.GetComponent<IWeaponsUser>();
            weaponUser.Weapon = Substitute.For<IWeapon>();
            weaponUser.Weapon.EffectiveRange.Returns(4f);
            var movingToMasterChief = false;
            var grunt = _sutPrefabInstance.GetComponent<Grunt>();
            grunt.MovingCloserToTarget += () => movingToMasterChief = true;

            // A grunt move towards master chief
            sut.transform.position = Vector3.zero;
            testMasterChief.transform.position = sut.transform.position + Vector3.forward * (weaponUser.Weapon.EffectiveRange + 1);
            yield return null;

            Assert.IsTrue(movingToMasterChief);
        }
    }
}