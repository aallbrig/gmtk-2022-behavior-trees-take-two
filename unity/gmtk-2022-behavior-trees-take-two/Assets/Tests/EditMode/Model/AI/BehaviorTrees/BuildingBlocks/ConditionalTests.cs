using System.Collections;
using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests.EditMode.Model.AI.BehaviorTrees.BuildingBlocks
{
    public class ConditionalTests
    {
        [Test]
        public void Conditional_SucceedsOnTrue()
        {
            var sut = new Conditional(new ConditionalContext(() => true));

            sut.Tick();
            
            Assert.AreEqual(Status.Success, sut.CurrentStatus);
        }
    }
}