using System.Collections;
using Model.Player;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.MonoBehaviours
{
    public class MainMenuTests
    {
        [UnityTest]
        public IEnumerator MainMenu_StartsWith_NothingSelected()
        {
            var sut = new GameObject().AddComponent<MainMenu>();
            yield return null;

            Assert.AreEqual(
                new NothingSelectedState().GetType().Name,
                sut.CurrentState.GetType().Name
            );
        }
    }
}