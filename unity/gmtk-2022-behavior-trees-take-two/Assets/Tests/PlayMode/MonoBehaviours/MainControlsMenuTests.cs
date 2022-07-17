using System.Collections;
using Model.Player;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours
{
    public class MainControlsMenuTests
    {
        [UnityTest]
        public IEnumerator MainControlsMenu_StartsWith_NothingSelected()
        {
            var sut = new GameObject().AddComponent<MainControlsMenu>();
            yield return null;

            Assert.AreEqual(
                new NothingSelectedState().GetType().Name,
                sut.CurrentState.GetType().Name
            );
        }
    }
}