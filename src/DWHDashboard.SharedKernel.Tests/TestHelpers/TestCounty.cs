using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.SharedKernel.Tests.TestHelpers
{
    public class TestCounty : Entity<int>
    {
        public string Name { get; set; }

        public TestCounty()
        {
        }

        public TestCounty(int id, string name) : base(id)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id}|{Name}";
        }
    }
}
