using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum EquipType
    {
        Weapon,
        SubWeapon,
        Head,
        Amor,
        Boots
    }
    internal class EquipItem

    {
        public string Name { get; }
        public EquipType Type { get; }
        public string Description { get; }
        public int Atk { get; }
        public int Def { get; }
        public int MaxHp { get; }
        public int Cost { get; }
        public int ItemCount { get; set; }
        public bool isEquip { get; set; }

        public EquipItem(string name, EquipType type, int atk, int def, int maxHp, string discription, int cost)
        {
            Name = name;
            Type = type;
            Description = discription;
            Atk = atk;
            Def = def;
            MaxHp = maxHp;
            Cost = cost;
            ItemCount = 1;
            isEquip = false;
        }
        public void DisplayEquipItem()
        {
            Console.Write($"[E]{Name,-8}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine($"{Description}");
        }
        public void DisplayinventoryItem()
        {
            Console.Write($"   {Name,-8}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine($"{Description}");
        }

        public void AddEquipItem_toInventory()
        {

        }
    }
}