using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Vehicles;

namespace SpaceSimulation.src.Commands
{
    class BuildVehicle : Command
    {
        public Station s;
        public Vehicle v;

        public CommandState state;
        public int progress;
        public int workPower;
        public int neededWork;

        public BuildVehicle(Station s, Vehicle v, Facility facility)
        {
            this.s = s;
            this.v = v;
            progress = 0;
            // Janky decision to allow null facility. Should be refactored
            if (facility != null) { workPower = facility.getWorkPower(); }
            this.neededWork = 0;
        }
        public void execute(WorldState ws)
        {
            if (state.Equals(CommandState.SUCCESS))
            {
                return;
            }
            if (this.neededWork == 0)
            {
                state = CommandState.PROGRESS;
                neededWork = v.buildEffort;
            }
            progress += workPower;
            if (progress >= neededWork)
            {
                state = CommandState.SUCCESS;
                s.vehicles.Add(v);
            }

        }
        public CommandState getState()
        {
            return state;
        }
    }
}
