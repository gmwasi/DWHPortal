﻿using System.Collections.Generic;
using System.Linq;
using DWHDashboard.SharedKernel.Tests.TestHelpers;
using FizzWare.NBuilder;

namespace DWHDashboard.SharedKernel.Data.Tests.TestHelpers
{
    public class Factory
    {
        private static List<TestCar> _testCars = new List<TestCar>();

        public static void Init()
        {
            _testCars = new List<TestCar>();
            _testCars = TestCars();
        }


    
        public static List<TestCar> TestCars()
        {
            if (_testCars.Count > 0) return _testCars;

            var list = Builder<TestCar>.CreateListOfSize(2)
                .Build()
                .ToList();

            return list;
        }
    }
}
