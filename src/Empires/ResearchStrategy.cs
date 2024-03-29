﻿using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    class ResearchStrategy : Strategy
    {
        WorldState ws;
        Empire empire;

        public ResearchStrategy(WorldState ws, Empire e)
        {
            this.ws = ws;
            this.empire = e;
        }
        public List<EmpireCommand> executeStrategy()
        {
            throw new NotImplementedException();
        }

        public int getScore()
        {
            throw new NotImplementedException();
        }
    }
}
