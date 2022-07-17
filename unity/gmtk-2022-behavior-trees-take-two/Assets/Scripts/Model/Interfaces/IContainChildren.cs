using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IContainChildren<T>
    {
        public List<T> GetChildren();
    }
}