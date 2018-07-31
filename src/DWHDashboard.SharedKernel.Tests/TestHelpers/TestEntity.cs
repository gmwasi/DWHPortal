using System;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.SharedKernel.Tests.TestHelpers
{
    class TestEntity : Entity<Guid>
    {
        public string Name { get; set; }

        public TestEntity(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public TestEntity(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id}|{Name}";
        }
    }
}