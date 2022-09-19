using SpaceSimulation.Commands;
using SpaceSimulation.components;
using SpaceSimulation.Components;
using SpaceSimulation.Helpers;
using SpaceSimulation.Nodes;
using SpaceSimulation.Ships;
using SpaceSimulation.src.Bases;
using SpaceSimulation.src.Commands;
using SpaceSimulation.src.Helpers;
using SpaceSimulation.src.Vehicles;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    class Station : Entity
    {
        public String name { get; set; }
        public int id { get; set; }
        public int empireId { get; set; }
        public int[] goods { get; set; }
        public int[] desiredGoods { get; set; }
        public int facilitySpace;
        public List<Facility> facilities { get; set; }
        public int health { get; set; }
        public Tuple<int, int> location;
        public List<Vehicle> vehicles;
        public List<Vehicle> combatShips;
        private Random r = new Random();

        private List<Node>[] closeNodes;
        public int CLOSE_ORE_COUNT = 10;

        // What the station is working on, and about to distpatch
        public List<Command> pendingCommands;

        public Dictionary<Facility, List<Command>> waitingTasks;

        private int fullTargetCount = 100000000;

        // Create a lit of build commands to process?

        public Station(Tuple<int, int> position, WorldState state, int empireId)
        {
            location = position;
            vehicles = new List<Vehicle>();
            combatShips = new List<Vehicle>();
            this.goods = new int[state.marketplace.goods.Length];
            this.desiredGoods = new int[state.marketplace.goods.Length];
            this.goods[0] += 1000;
            this.goods[1] += 100;

            closeNodes = new List<Node>[WorldState.RESOURCE_COUNT];
            populateCloseNodes(state);
            facilitySpace = 100;
            // Innit base facilities.
            // Probably add a spaceport?
            Refinery smelter = new Refinery();
            Starport port = new Starport();
            Fabricator fab = new Fabricator();
            this.facilities = new List<Facility>();
            facilities.Add(smelter);
            facilities.Add(port);
            facilities.Add(fab);
            pendingCommands = new List<Command>();

            waitingTasks = new Dictionary<Facility, List<Command>>();
            waitingTasks[smelter] = new List<Command>();
            waitingTasks[port] = new List<Command>();
            waitingTasks[fab] = new List<Command>();
            this.empireId = empireId;
        }

        private void populateCloseNodes(WorldState state)
        {
            SortedList<double, Node>[] tmp_nodes = new SortedList<double, Node>[WorldState.RESOURCE_COUNT];
            for (int i =0; i< WorldState.RESOURCE_COUNT; i++)
            {
                SortedList<double, Node> node_list = new SortedList<double, Node>();
                tmp_nodes[i] = node_list;
            }

            foreach (Node n in state.nodes)
            {
                int type = n.type;
                Double distance = Distances.distance(this.location, n.location);
                // TODO Maybe not such an arbitrary radius
                if (distance < 4500)
                {
                    if(tmp_nodes[type].Count < CLOSE_ORE_COUNT)
                    {
                        tmp_nodes[type].Add(distance, n);
                    } else
                    {
                        if (distance < tmp_nodes[type].Keys[CLOSE_ORE_COUNT - 1])
                        {
                            tmp_nodes[type].Remove(tmp_nodes[type].Keys[CLOSE_ORE_COUNT - 1]);
                            try
                            {
                                tmp_nodes[type].Add(distance, n);
                            } catch (Exception e)
                            {
                                // TODO. More elegant soluton could be good!
                                tmp_nodes[type].Add(distance + .000000001, n);
                            }
                        }  
                    }
                }
            }
            for (int i = 0; i < tmp_nodes.Length; i++)
            {
                closeNodes[i] = new List<Node>();
                SortedList<double, Node> items = tmp_nodes[i];
                foreach (var x in items) {
                    closeNodes[i].Add(x.Value);
                }
            }
        }
        public Tuple<int, int> getLocation()
        {
            return location;
        }
        public int getSize()
        {
            return 32;
        }

        public string getSprite()
        {
            return "spacestation";
        }

        public void setLocation(Tuple<int, int> location)
        {
            this.location = location;
        }

        // 1 Check if any facility can take the command
        // 2 Pay for it
        // 3 Submit command to facility
        // 4 - Add to pending commands list
        public bool build(WorldState ws, int target)
        {
            // A little awkward
            // TODO remove null. Need another pattern.
            Build b = new Build(this, target, null);
            // recursively build or gather the requirements?
            // TODO support multiple valid facilities
            foreach (Facility f in this.facilities)
            {
                // TODO Be aware that if multiple facilities can build the same thing, this could cause a bug.
                // It will return and fail as soon as one fails to build, even if the other has capacity.
                if (this.waitingTasks[f].Count > 25)
                {
                    return false;
                }

                if (f.isEligible(b))
                {
                    if (Spending.buildIfPossible(this.goods, ws.marketplace.goods[target].cost))
                    {
                        b = new Build(this, target, f);
                        this.waitingTasks[f].Add(b);
                        return true;
                    }
                }
            }
            return false;
        }

        // TODO assign what kind with weight?
        // Create a build task which takes some time?
        // Create a default starport facility?
        public bool buildVehicle(WorldState ws, Vehicle v1)
        {
            // The check also spends the resources
            BuildVehicle b = new BuildVehicle(this, v1, null);
            foreach (Facility f in this.facilities)
            {
                if (f.isEligible(b))
                {
                    if (Spending.buildIfPossible(this.goods, v1.getCost()))
                    {
                        b = new BuildVehicle(this, v1, f);
                        this.waitingTasks[f].Add(b);
                        return true;
                    }
                }
            }
           // if (pendingCommands.Count < 50) { pendingCommands.Add(b); }
            return false;
        }

        // TODO assign resource with weight?
        // Figures out how mnay miners it wishes to have available.
        // Assignment done separately.
        // Expensive method, call rarely.
        // Perhaps someday choose not to build all miners, or not mine some things?
        public void saturateMines(WorldState ws)
        {
            if (this.vehicles.Count >= fullTargetCount)
            {
                return;
            }

            int minerCount = 0;
            Dictionary<Node, int> nodeAssignmentCount = new Dictionary<Node, int>();
            List<BasicMiner> unassignedMiners = new List<BasicMiner>();
            BasicMiner t1 = new BasicMiner(null, 0);
            double miningRate = t1.miningpower;
            double minCapacity = t1.capacity;
            double speed = t1.speed;
            t1 = null;
            foreach (Vehicle v in vehicles)
            {
                if (v.getVehicleType() == VehicleType.MINER)
                {
                    minerCount++;
                    // It's an active miner, sum the node assignments
                    if (v.command != null)
                    {
                        int tmp = nodeAssignmentCount.GetValueOrDefault(((Mine)v.command).n, 0);
                        nodeAssignmentCount[((Mine)v.command).n] = 1 + tmp;
                    } else
                    {
                        unassignedMiners.Add((BasicMiner)v);
                    }
                }
            }
            int targetCount = 0;
            double fillTime = (minCapacity / miningRate);

            List<Node> flattenedList = new List<Node>();
            foreach (List<Node> l in closeNodes)
            {
                flattenedList.AddRange(l);
            }
            flattenedList.Sort((n1, n2) => 
            (int)(Distances.distance(n2.getLocation(), this.getLocation()) 
            - Distances.distance(n1.getLocation(), this.getLocation())));

            foreach (Node n in flattenedList) {
                // The further away, and the bigger, the more miners we want.
                double distance = Distances.distance(n.getLocation(), this.getLocation());
                double travelTime = (distance / speed);

                double miningRatio = (miningRate / n.outputVolume) * (fillTime / (fillTime + (2 * travelTime)));
                targetCount += (int)(1 / miningRatio);

                targetCount = Math.Max(targetCount, 1);
                // TODO - Is this same? I think rounding truncates the last miner.
                targetCount += 1;
                if (!nodeAssignmentCount.ContainsKey(n)) {
                    nodeAssignmentCount.Add(n, 0);
                }
            }

            for (int i = 0; i < (targetCount - minerCount); i++)
            {
                Tuple<int, int> location = new Tuple<int, int>(this.location.Item1 + r.Next(-1, 1), this.location.Item2 + r.Next(-1, 1));
                BasicMiner v1 = new BasicMiner(location, empireId);
                this.buildVehicle(ws, v1);
            }
            fullTargetCount = targetCount;

            //Assign lacking miners to nodes
            foreach (var obj in nodeAssignmentCount)
            {
                int removedMiners = 0;
                int desiredMiners = (int)(obj.Key.outputVolume * ((Distances.distance(obj.Key.getLocation(), this.getLocation())) / 300));
                // TODO - It seems to not mine if too close. Something unresolved.
                desiredMiners = Math.Max(desiredMiners, 1);
                for (int q = 0; q < (desiredMiners - obj.Value); q++)
                {
                    if (removedMiners < unassignedMiners.Count)
                    {
                        Mine c = new Mine(unassignedMiners[removedMiners], obj.Key, this);
                        unassignedMiners[removedMiners].command = c;
                        removedMiners++;
                    }
                }
            }
        }

        public void moveVehicles(WorldState ws)
        {
            foreach (Vehicle v in this.vehicles)
            {
                if (v.command != null)
                {
                    v.command.execute(ws);
                }
                // TODO add to some idle list?
            }
        }
        public void runFacilities(WorldState ws)
        {
            foreach (Facility f in this.facilities)
            {
                // Remove need for obsessive null checks
                if (f.executingCommands() == null 
                    || f.executingCommands().Count == 0
                    || f.executingCommands()[0] == null
                    || f.executingCommands()[0].getState() == CommandState.SUCCESS)
                {
                    List<Command> actedTasks = new List<Command>();
                    foreach (Command c in waitingTasks[f])
                    {
                        if (f.canTakeCommand(c))
                        {
                            actedTasks.Add(c);
                            f.addCommand(c);
                        }
                    }
                    foreach(Command c in actedTasks)
                    {
                        waitingTasks[f].Remove(c);
                    }
                }
                if (f.executingCommands() != null)
                {
                    f.execute(ws);
                }
                // TODO add to some idle list?
            }
        }
        public void deliverGoods(int type, int quantity)
        {
            this.goods[type] += quantity;
        }
    }
}
