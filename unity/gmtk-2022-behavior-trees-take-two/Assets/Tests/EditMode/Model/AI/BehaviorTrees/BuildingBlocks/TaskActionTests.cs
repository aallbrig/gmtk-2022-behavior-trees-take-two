using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace Tests.EditMode.Model.AI.BehaviorTrees.BuildingBlocks
{
    public class TaskActionTests
    {
        private static Status[] _checkStatuses = { Status.Success, Status.Running, Status.Failure };

        [Test]
        public void TaskAction_AllowsConsumer_ToDefineReturnStatus([ValueSource(nameof(_checkStatuses))] Status desiredStatus)
        {
            var sut = new TaskAction(() => desiredStatus);

            sut.Tick(Substitute.For<IBehaviorTree>());

            Assert.AreEqual(desiredStatus, sut.CurrentStatus);
        }
    }
}