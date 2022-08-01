using System.Collections;
using MonoBehaviours.MobileSuit;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours.MobileSuit
{
    public class BattleSystemTests
    {
        [UnityTest]
        public IEnumerator BattleSystemTestsWithEnumeratorPasses()
        {
            var sut = new GameObject().AddComponent<BattleSystem>();
            yield return null;
        }
    }
}