using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace Tests.EditMode.Model.AI.BehaviorTrees.BuildingBlocks
{
    public class ConditionalTests
    {
        [Test]
        public void Conditional_SuccessStatus_On_FunctionTrue()
        {
            var sut = new Conditional(() => true);

            sut.Tick(Substitute.For<IBehaviorTree>());

            Assert.AreEqual(Status.Success, sut.CurrentStatus);
        }

        [Test]
        public void Conditional_FailureStatus_On_FunctionFalse()
        {
            var sut = new Conditional(() => false);

            sut.Tick(Substitute.For<IBehaviorTree>());

            Assert.AreEqual(Status.Failure, sut.CurrentStatus);
        }
    }
}