using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Monster
    {
        public int Lv {  get; set; }
        public string Name { get; }
        public int Health { get; set; }
        public int Atk {  get; set; }

        public Monster(int lv, string name, int health, int atk)
        { 
            Lv = lv;
            Name = name;
            Health = health;
            Atk = atk;
        }
    }
}
