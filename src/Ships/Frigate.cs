﻿using SpaceSimulation.Commands;
using SpaceSimulation.Ships;
using SpaceSimulation.src.Vehicles;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Ships
{

    // Pull out to JSON definition and just load to make extensible to more ships?
    class Frigate : Vehicle
    {
        public Frigate(Tuple<int, int> location, int empireId) : base(location, empireId)
        {
            this.speed = 2;
            this.name = "frigate";
            this.location = location;
            this.speed = 3;
            this.health = 100;
            this.armor = 5;
            this.shield = 30;
            this.shield_recharge = 1;
            this.buildEffort = 20;
            this.weapons = new Weapon[6];
            // Write a debug statement
            Console.WriteLine("Frigate created at: " + location.Item1 + "-" + location.Item2);
        }
        public int id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int fuel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int fuelPerUnit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        override public string getSprite()
        {
            return "frigate1";
        }

        override public VehicleType getVehicleType()
        {
            return VehicleType.FRIGATE;
        }
        override public int getSize()
        {
            return 22;
        }

        public override List<int[]> getCost()
        {
            // TODO fix cost.
            List<int[]> cost = new List<int[]>();
            int[] c = new int[2];
            c[0] = 7;
            c[1] = 5;
            cost.Add(c);
            c = new int[2];
            c[0] = 8;
            c[1] = 5;
            cost.Add(c);
            return cost;
        }
    }
}
