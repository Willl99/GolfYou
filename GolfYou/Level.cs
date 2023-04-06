using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GolfYou
{
    public class Level
    {
        public List<char> layout;
        public int width;
        public int length;
        public List<Vector2> coordinates;

        public Level(string name)
        {
            TextReader reader = File.OpenText(name);

            coordinates = new List<Vector2>();

            layout = new List<char>();
            string readline = reader.ReadLine();
            string[] line;
            while (readline != null)
            {
                line = readline.Split(',');
                width = line.Length;
                foreach (string x in line)
                {
                    //TODO add in string parser
                }
                readline = reader.ReadLine();
            }
            length = layout.Count / width;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++) coordinates.Add(new Vector2(j, i));
            }
        }
    }
}

