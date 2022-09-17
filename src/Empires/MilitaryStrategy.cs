using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using SpaceSimulation.src.Commands;
using SpaceSimulation.src.Helpers;
using SpaceSimulation.src.Ships;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    class MilitaryStrategy : Strategy
    {
        WorldState ws;
        Empire empire;
        List<Vehicle> warships;

        public MilitaryStrategy(WorldState ws, Empire e)
        {
            this.ws = ws;
            this.empire = e;
            warships = new List<Vehicle>();
        }

        public int getScore()
        {
            throw new NotImplementedException();
        }

        private Boolean hasHp(Vehicle v)
        {
            if (v.health <=0) {
                ws.removeObject(v);
                return false;
            }
            return true;
        }

        public List<EmpireCommand> executeStrategy()
        {
            // More efficient to do it at damage time instead of on every iteration.
            warships = warships.FindAll(hasHp);

            foreach (Station s in this.empire.stations){
                if (s.combatShips.Count > 0)
                {
                    warships.AddRange(s.combatShips);
                    s.combatShips = new List<Vehicle>();
                }
            }

            foreach (Vehicle v in warships) {
                if (v.command == null)
                {
                    Command c = new SearchAndDestroy(v);
                    v.command = c;
                } else
                {
                    v.command.execute(ws);
                }
            }

            return null;
        }
  
    }
}
