using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using System;

namespace GreenFoxAcademy.SpaceSettlers.Factories
{
    public class ObjectFactory<T> where T : class
    {
        public T Build(Enum type, Kingdom kingdom)
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { type, kingdom });
        }
    }
}
