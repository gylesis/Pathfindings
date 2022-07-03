using System;
using System.Collections.Generic;
using System.Linq;
using Project.UI;

namespace Project.Grid.Cells
{
    public class DrawStrategiesContainer
    {
        private Dictionary<Type, IDrawStrategy> _drawStrategies;

        public DrawStrategiesContainer(IDrawStrategy[] strategies) => 
            _drawStrategies = strategies.ToDictionary(x => x.GetType());

        public IDrawStrategy GetStrategy<TDrawStrategy>() => 
            _drawStrategies[typeof(TDrawStrategy)];
    }
}