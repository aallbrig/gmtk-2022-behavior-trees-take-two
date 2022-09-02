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
        public void SetUp(List<GameObject> destroyList, out GameObject sut, out GameObject testMasterChief, out GameObject platform)
        {
            var testPlatform = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Test Combat Platform"));
            foreach (var debugger in testPlatform.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            // Get nav mesh surface component and render out a nav mesh
            testPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
            destroyList.Add(testPlatform);
            var sutPrefabInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Grunt (AI)"));
            foreach (var debugger in sutPrefabInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = true;
            destroyList.Add(sutPrefabInstance);
            var testMasterChiefInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Master Chief (Player)"));
            foreach (var debugger in testMasterChiefInstance.GetComponents<IMonobehaviourDebugLogger>())
                debugger.DebugEnabled = false;
            testMasterChiefInstance.GetComponent<BehaviorTreeRunner>().DebugEnabled = false;
            testMasterChiefInstance.GetComponent<MasterChief>().DebugEnabled = false;
            testMasterChiefInstance.GetComponent<ProximitySensor>().DebugEnabled = false;
            testMasterChiefInstance.transform.position = Vector3.forward * 30;
            destroyList.Add(testMasterChiefInstance);
            sut = sutPrefabInstance;
            testMasterChief = testMasterChiefInstance;
            platform = testPlatform;
        }

        public void Teardown(List<GameObject> destroyList)
        {
            foreach (var gameObject in destroyList)
                Object.Destroy(gameObject);
            destroyList.Clear();
        }

        [UnityTest]
        public IEnumerator GruntTargetsMasterChief_IfCloseEnough()
        {
            var destroyList = new List<GameObject>();
            SetUp(destroyList, out var sut, out var testMasterChief, out _);

            sut.GetComponent<BehaviorTreeRunner>().config.timeBetween = 0.01f;
            var targetingMasterChief = false;
            var grunt = sut.GetComponent<Grunt>();
            grunt.TargetAcquired += _ => targetingMasterChief = true;

            // A grunt can see master chief if he is 3m away
            sut.transform.position = Vector3.zero;
            testMasterChief.transform.position = new Vector3(0, 0, 3);
            Debug.Log($"distance between grunt and test master chief {Vector3.Distance(sut.transform.position, testMasterChief.transform.position)}");
            yield return new WaitForSeconds(1.0f);
            Debug.Log($"distance between grunt and test master chief {Vector3.Distance(sut.transform.position, testMasterChief.transform.position)}");

            Assert.IsTrue(targetingMasterChief);
            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator GruntMovesCloseToMasterChief_IfOutsideEffectiveWeaponRange()
        {
            var destroyList = new List<GameObject>();
            SetUp(destroyList, out var sut, out var testMasterChief, out _);
            var weaponUser = sut.GetComponent<IWeaponsUser>();
            weaponUser.Weapon = Substitute.For<IFirearm>();
            weaponUser.Weapon.EffectiveRange.Returns(1f);
            sut.GetComponent<BehaviorTreeRunner>().config.timeBetween = 0.01f;
            var movingToMasterChief = false;
            var grunt = sut.GetComponent<Grunt>();
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
            Teardown(destroyList);
        }
    }
}