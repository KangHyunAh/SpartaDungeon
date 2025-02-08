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
        public int Atk { get; set; }
        public int Rewards { get; set; }
        public int Exp { get; set; }
        public string MonsterType { get; set; }
        
        public Monster(int lv, string name, int health, int atk, int rewards, int exp, string monstertype)
        {
            int random = new Random().Next(0, 10);
            Lv = lv + random;
            Name = name;
            Health = health + random * 10;
            health = Math.Max(health, 0);
            Atk = atk + random*2;
            Rewards = rewards;
            Exp = exp;
            MonsterType = monstertype;
        }
        public Monster Spawn()
        {
            return new Monster(Lv, Name, Health, Atk, Rewards, Exp, MonsterType);
        }
    }
}
