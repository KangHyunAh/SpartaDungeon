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
        public string MonsterType { get;}
        
        public Monster(int lv, string name, int health, int atk, int rewards, int exp, string monstertype)
        {
            Name = name;
            Rewards = rewards;
            Exp = exp;
            MonsterType = monstertype;
            if (monstertype == "이벤트")
            {
                Lv = lv;
                Health = health;
                health = Math.Max(health, 0);
                Atk = atk;

            }
            else
            {
                int random = new Random().Next(0, 5);
                Lv = lv + random;
                Health = health + random * 3;
                health = Math.Max(health, 0);
                Atk = atk + random ;
            }
        }
        public Monster Spawn()
        {
            return new Monster(Lv, Name, Health, Atk, Rewards, Exp, MonsterType);
        }
    }
}
