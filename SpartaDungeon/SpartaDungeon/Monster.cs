using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Monster
    {
        public int Lv { get; set; } = 0;
        public string Name { get; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Atk { get; set; }
        public int Rewards { get; set; }
        public int Exp { get; set; }
        public string MonsterType { get; }

        public Monster(string name, int health, int atk, int rewards, int exp, string monstertype)
        {
            Name = name;
            Rewards = rewards;
            Exp = exp;
            MonsterType = monstertype;
            if (monstertype == "이벤트")
            {
                Lv = 1;
                Health = health;
                MaxHealth = health;
                health = Math.Max(health, 0);
                Atk = atk;

            }
            else
            {
                Lv = atk / 10+1;
                int random = new Random().Next(0, 5);
                Health = health + random * 3;
                health = Math.Max(health, 0);
                MaxHealth = health + random * 3;
                Rewards += Lv * 100;
                Exp += Lv * 5;
                Atk = atk + random;
            }
        }
        public Monster Spawn()
        {
            return new Monster(Name, Health, Atk, Rewards, Exp, MonsterType);
        }
    }
}
