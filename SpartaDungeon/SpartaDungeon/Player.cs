using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Player
    {
        public int Level { get; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int Atk { get; set; }
        public int EquipAtk { get; set; }
        public int TotailAtk { get; set; }

        public int Def { get; }
        public int EquipDef { get; set; }
        public int TotalDef { get; set; }

        public int Gold { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; }

        public bool IsDead {  get; set; }

        public Player()
        {
            Level = 1;
            Name = string.Empty;
            Job = "전사";
            Atk = 10;
            EquipAtk = 0;
            TotailAtk = Atk + EquipAtk;
            Def = 5;
            EquipDef = 0;
            TotalDef = Def + EquipDef;
            Gold = 1500;
            Hp = 100;
            MaxHp = 100;
        }
    }
}
