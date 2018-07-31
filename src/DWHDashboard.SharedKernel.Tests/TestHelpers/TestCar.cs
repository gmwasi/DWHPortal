using System;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.SharedKernel.Tests.TestHelpers
{
    public class TestCar : Entity<Guid>
    {
        public string Name { get; set; }

        public TestCar()
        {
        }

        public TestCar(string name)
        {
            Name = name;
        }
        
        public override string ToString()
        {
            return $"{Id}|{Name}";
        }
    }
}
