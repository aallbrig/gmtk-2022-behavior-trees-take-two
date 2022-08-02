using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface ITargetSystem
    {
        public ITargetingSystemConfiguration Configuration { get; }
        public IEnumerable<IGameAgent> Friendlies { get; }
        public IEnumerable<IGameAgent> Enemies { get; }
        public IEnumerable<IGameAgent> Neutrals { get; }
    }
}