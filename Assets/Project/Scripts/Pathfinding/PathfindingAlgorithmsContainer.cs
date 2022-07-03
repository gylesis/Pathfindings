using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pathfinding
{
    public class PathfindingAlgorithmsContainer
    {
        private readonly Dictionary<Type, IPathfindingAlgorithm> _algorithms;
        
        public PathfindingAlgorithmsContainer(IPathfindingAlgorithm[] algorithms)
        {
            _algorithms = algorithms.ToDictionary(x => x.GetType());
        }

        public IPathfindingAlgorithm GetAlgorithm<TAlgorithmType>()
        {
            return _algorithms[typeof(TAlgorithmType)];
        }
        
    }
}