using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using NUnit.Framework;

namespace Tests.EditMode.Model.AI.BehaviorTrees.BuildingBlocks
{
    public class SequenceTests
    {
        [Test]
        public void Sequences_IterateToNextBehavior_WhenPreviousBehaviorSucceeds()
        {
            var sut = new Sequence();
            var nextBehaviorCalled = false;
            var previousBehavior = new TaskAction(() => Status.Success);
            var nextBehavior = new TaskAction(() =>
            {
                nextBehaviorCalled = true;
                return Status.Success;
            });
            sut.AddChild(previousBehavior);
            sut.AddChild(nextBehavior);
            var testBehaviorTree = new BehaviorTree(sut);

            testBehaviorTree.Evaluate();

            Assert.IsTrue(nextBehaviorCalled);
        }
        [Test]
        public void Sequences_HandleLongRunningBehaviors()
        {
            var sut = new Sequence();
            var nextBehaviorCalled = false;
            var counter = 0;
            var previousBehavior = new TaskAction(() => ++counter == 3 ? Status.Running : Status.Success);
            var nextBehavior = new TaskAction(() =>
            {
                nextBehaviorCalled = true;
                return Status.Success;
            });
            sut.AddChild(previousBehavior);
            sut.AddChild(nextBehavior);
            var testBehaviorTree = new BehaviorTree(sut);

            testBehaviorTree.Evaluate();

            Assert.IsTrue(nextBehaviorCalled);
        } 
        [Test]
        public void Sequences_HandleBehaviorFailures()
        {
            var sut = new Sequence();
            var failureEventBroadcast = false;
            sut.Failed += () => failureEventBroadcast = true;
            var nextBehaviorCalled = false;
            var counter = 0;
            var previousBehavior = new TaskAction(() => Status.Failure);
            var nextBehavior = new TaskAction(() =>
            {
                nextBehaviorCalled = true;
                return Status.Success;
            });
            sut.AddChild(previousBehavior);
            sut.AddChild(nextBehavior);
            var testBehaviorTree = new BehaviorTree(sut);

            testBehaviorTree.Evaluate();

            Assert.AreEqual(Status.Failure, sut.CurrentStatus);
            Assert.IsFalse(nextBehaviorCalled);
            Assert.IsTrue(failureEventBroadcast);
        } 
    }
}