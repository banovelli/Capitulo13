using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeColmeia
{
    [Serializable]
    public class Flower
    {
        private Point location;
         public Point Location { get { return location; } }
        private int age;
            public int Age { get { return age; } }
        private bool alive;
            public bool Alive { get { return alive; } }
        private double nectar;
            public double Nectar { get { return nectar; } }
            public double NectarHarvested { get; set; }
        private int lifespan;

        private const int LifeSpanMin = 15000;
        private const int LifeSpanMax = 30000;
        private const double InitialNectar = 1.5;
        private const double MaxNextar = 5;
        private const double NectarAddedPerTurn = 0.01;
        private const double NectarGatheredPerTurn = 0.3;

        public Flower(Point location, Random random)
        {            
            this.location = location;
            age = 0;
            alive = true;
            nectar = InitialNectar;
            NectarHarvested = 0;
            lifespan = random.Next(LifeSpanMin, LifeSpanMax + 1);
        }

        public double HarvestNectar()
        {
            if (nectar > NectarGatheredPerTurn)
            {
                NectarHarvested += NectarGatheredPerTurn;
                nectar -= NectarGatheredPerTurn;
                return NectarGatheredPerTurn;
            }
            else{
                return 0;
            }

            //if (nectar > 0)
            //{
            //    if (nectar > NectarGatheredPerTurn)
            //    {
            //        NectarHarvested += NectarGatheredPerTurn;                    
            //        nectar -= NectarGatheredPerTurn;                    
            //    }
            //    else{
            //        NectarHarvested += nectar;
            //        nectar = 0;
            //    }
            //}
            ////else { } nada a fazer retorno 0
            //return NectarHarvested;
        }

        public void Go()
        {
            age++;
            if (age > lifespan) //must die
            {
                alive = false;
            }
            else
            {               
                nectar = (nectar + NectarAddedPerTurn) <= MaxNextar ? (nectar + NectarAddedPerTurn) : MaxNextar;
            }
        }
    }
}
