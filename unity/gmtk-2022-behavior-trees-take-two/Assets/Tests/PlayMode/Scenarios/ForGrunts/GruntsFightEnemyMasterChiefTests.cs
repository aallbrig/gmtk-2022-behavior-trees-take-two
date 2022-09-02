using System.Collections;
using System.Collections.Generic;
using Model.Interfaces;
using Model.Interfaces.BattleSystem;
using MonoBehaviours;
using MonoBehaviours.Brains;
using MonoBehaviours.Sensors;
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
            foreach (var debugger in _testPlatform.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            // Get nav mesh surface component and render out a nav mesh
            _testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            _destroyMeAtEnd.Add(_testPlatform);
            _sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Grunt (AI)"));
            foreach (var debugger in _sutPrefabInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = true;
            _destroyMeAtEnd.Add(_sutPrefabInstance);
            _testMasterChiefInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            foreach (var debugger in _sutPrefabInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            _testMasterChiefInstance.GetComponent<BehaviorTreeRunner>().DebugEnabled = false;
            _testMasterChiefInstance.GetComponent<MasterChief>().DebugEnabled = false;
            _testMasterChiefInstance.GetComponent<ProximitySensor>().DebugEnabled = false;
            _testMasterChiefInstance.transform.position = Vector3.forward * 30;
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
            var targetingMasterChief = false;
            var grunt = sut.GetComponent<Grunt>();
            grunt.TargetAcquired += _ => targetingMasterChief = true;

            // A grunt can see master chief if he is 3m away
            sut.transform.position = Vector3.zero;
            testMasterChief.transform.position = new Vector3(0, 0, 2);
            Debug.Log($"distance between grunt and test master chief {Vector3.Distance(sut.transform.position, testMasterChief.transform.position)}");
            yield return new WaitForSeconds(1.0f);

            Assert.IsTrue(targetingMasterChief);
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
            testMasterChief.transform.position = new Vector3(0, 0, 4);
            Debug.Log($"distance between grunt and test master chief {Vector3.Distance(sut.transform.position, testMasterChief.transform.position)}");
            yield return new WaitForSeconds(1.0f);

            Assert.IsTrue(movingToMasterChief);
        }
    }
}