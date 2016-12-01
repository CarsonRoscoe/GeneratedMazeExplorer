using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public static class Console {
        public static class Name {
            public const string LeftWall = "LeftWall";
            public const string RightWall = "RightWall";
            public const string Ceiling = "Ceiling";
            public const string Floor = "Floor";
            public const string Background = "Background";
            public const string Ball = "Ball";
            public const string PlayerOne = "PlayerOne";
            public const string PlayerTwo = "PlayerTwo";
            public const string Rules = "Rules";
        }

        public static class Attribute {
            public const string Color = "Color";
            public const string Speed = "Speed";
            public const string MaxScore = "MaxScore";
        }

        public static class Value {
            public const string Slow = "Slow";
            public const string Fast = "Fast";
            public static Color Black = new Color( 0, 0, 0 );
            public static Color White = new Color( 1, 1, 1 );
            public static Color Red = new Color( 1, 0, 0 );
            public static Color Green = new Color( 0, 1, 0 );
            public static Color Blue = new Color( 0, 0, 1 );
        }
    }
}
