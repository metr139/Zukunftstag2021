﻿using System;

namespace LichtSpiel
{
    public class FarbenZufallsGenerator
    {
        private Random _random;

        public FarbenZufallsGenerator()
        {
            _random = new Random();
        }

        public Farbe GibFarbe()
        {
            int zahl = _random.Next(1, 4);
            Farbe farbe = (Farbe)zahl;

            return farbe;
        }
    }
}
