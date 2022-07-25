using Model.AI.BehaviorTrees;
using NSubstitute;
using NUnit.Framework;

namespace Tests.EditMode.Model.AI.BehaviorTrees
{
    public class BehaviorTreeTests
    {

        [Test]
        public void BehaviorTrees_CanFullyRun()
        {
            var mockBehavior = Substitute.For<IBehavior>();
            var sut = new BehaviorTree(mockBehavior);
            var eventCalled = false;
            sut.TreeTraversalCompleted += () => eventCalled = true;

            sut.Run();

            Assert.IsTrue(eventCalled);
        }

        [Test]
        public void BehaviorTrees_RootNodeTicksChildNode()
        {
            var mockBehavior = Substitute.For<IBehavior>();
            var sut = new BehaviorTree(mockBehavior);

            sut.Run();

            mockBehavior.Received().Tick();
        }
    }
}