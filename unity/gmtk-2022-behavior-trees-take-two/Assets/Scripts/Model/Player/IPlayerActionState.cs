using System.Collections.Generic;

namespace Model.Player
{
    public interface IPlayerActionState
    {
        public IEnumerable<IPlayerStateTransition> GetTransitions();
    }
}