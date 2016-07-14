using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeColmeia
{
    
    public enum BeeState
    {
        Idle,
        FlyingToFlower,
        GatheringNectar,
        ReturningToHive,
        MakingHoney,
        Retired
    };

    [Serializable]
    public class Bee
    {
        private const double HoneyConsumed = 0.5;
        private const int MoveRate = 3;
        private const double MinimumFlowerNectar = 1.5;
        private const int CareeSpan = 1000;
        public int Age { get; private set; }
        public bool InsideHive { get; private set; }
        public double NectarCollected { get; private set; }
        private Point location;
        public Point Location { get { return location; } }
        private int ID;
        private Flower destinationFlower;
        public BeeState CurrentState {get; private set;}

        private World world;
        private Hive hive;

        public delegate void BeeMessage(int ID, string Message);
        public BeeMessage MessageSender;

        public Bee(int id, Point location, World world, Hive hive)
        {
            this.ID = id;
            this.location = location;
            Age = 0;
            InsideHive = true;
            destinationFlower = null;
            NectarCollected = 0;
            CurrentState = BeeState.Idle;
            this.world = world;
            this.hive = hive;
        }

        public void Go(Random random)
        {
            Age++;
            BeeState oldSTate = CurrentState;
            switch (CurrentState)
            {
                case BeeState.Idle:
                    if(Age > CareeSpan){
                        CurrentState = BeeState.Retired;
                    }
                    else if(world.Flowers.Count > 0
                        && hive.ConsumeHoney(HoneyConsumed))
                    {
                        Flower flower = world.Flowers[random.Next(world.Flowers.Count)];
                        if (flower.Nectar >= MinimumFlowerNectar && flower.Alive)
                        {
                            destinationFlower = flower;
                            CurrentState = BeeState.FlyingToFlower;
                        }
                    }
                    break;
                case BeeState.FlyingToFlower:
                    if(!world.Flowers.Contains(destinationFlower)){
                        CurrentState = BeeState.ReturningToHive;
                    }
                    else if (InsideHive)
                    {
                        if (MoveTowardsLocation(hive.getLocation("Saída")))
                        {
                            InsideHive = false;
                            location = hive.getLocation("Entrada");
                        }
                    }
                    else
                        if (MoveTowardsLocation(destinationFlower.Location))
                            CurrentState = BeeState.GatheringNectar;
                    break;
                case BeeState.GatheringNectar:
                    double nectar = destinationFlower.HarvestNectar();
                    if(nectar > 0)
                        NectarCollected += nectar;
                    else
                        CurrentState = BeeState.ReturningToHive;
                    break;
                case BeeState.ReturningToHive:
                    if(!InsideHive){
                        if(MoveTowardsLocation(hive.getLocation("Entrada"))){
                            InsideHive = true;
                            location = hive.getLocation("Saída");
                        }
                    }
                    else{
                        if (MoveTowardsLocation(hive.getLocation("Fábrica de Mel")))
                            CurrentState = BeeState.MakingHoney;
                    }
                    break;
                case BeeState.MakingHoney:
                    if(NectarCollected < 0.5){
                        NectarCollected = 0;
                        CurrentState = BeeState.Idle;
                    }
                    else{
                        if (hive.AddHoney(0.5))
                            NectarCollected -= 0.5;
                        else
                            NectarCollected = 0;
                    }
                    break;
                case BeeState.Retired:
                    //não faz nada, já trabalho d+ :]
                    break;
                default:
                     //não faz nada,
                    break;
            }
            if (oldSTate != CurrentState
                && MessageSender != null)
                MessageSender(ID, CurrentState.ToString());
        }

        private bool MoveTowardsLocation(Point destination)
        {
            if (destination != null)
            {
                if (Math.Abs(destination.X - location.X) <= MoveRate &&
                    Math.Abs(destination.Y - location.Y) <= MoveRate)
                    return true;
                if (destination.X > location.X)
                    location.X += MoveRate;
                else if (destination.X < location.X)
                    location.X -= MoveRate;
                if (destination.Y > location.Y)
                    location.Y += MoveRate;
                else if (destination.Y < location.Y)
                    location.Y -= MoveRate;
            }
            return false;
        }        
    }
}
