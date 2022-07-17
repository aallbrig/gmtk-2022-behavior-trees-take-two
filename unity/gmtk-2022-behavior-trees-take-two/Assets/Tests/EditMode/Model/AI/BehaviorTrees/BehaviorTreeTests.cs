using System.Collections;
using Model.AI.BehaviorTrees;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests.EditMode.Model.AI.BehaviorTrees
{
    public class BehaviorTreeTests
    {

        [Test]
        public void BehaviorTrees_Runnable()
        {
            var mockBehavior = Substitute.For<IBehavior>();
            var sut = new BehaviorTree(mockBehavior);
            var eventCalled = false;
            sut.TreeTraversalStarted += () => eventCalled = true;

            sut.Run();

            Assert.IsTrue(eventCalled);
        }
    }
}