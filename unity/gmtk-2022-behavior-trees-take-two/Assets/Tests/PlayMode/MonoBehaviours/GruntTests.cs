using System.Collections;
using Model;
using MonoBehaviours;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours
{
    public class GruntTests
    {
        [UnityTest]
        public IEnumerator Grunts_CanDesireToMoveTowardsTarget()
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
    }
}