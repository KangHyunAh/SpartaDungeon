using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeon;
using MyPotionSystem = SpartaDungeon.PotionNamespace;
using Player = SpartaDungeon.Player;


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
            ItemCount = 0;
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
            Console.Write($"   {Name,-8}|소지수{ItemCount,-5}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine($"{Description}");
        }
        public void DisplayShopItem()
        {
            Console.Write($"   {Name,-8}|{Cost,5}|소지수{ItemCount,-5}");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine($"{Description}");
        }
    }

    public enum PotionType
    {
        Health,
        Mana,
    }

    namespace PotionNamespace
    {


        internal class ConsumableItem
        {


            public string Name { get; }
            public PotionType Type { get; }
            public string Description { get; }
            public int EffectAmount { get; }
            public int Cost { get; }
            public int ItemCount { get; set; }

            public ConsumableItem(string name, PotionType type, int effectAmount, string description, int cost)
            {
                Name = name;
                Type = type;
                Description = description;
                EffectAmount = effectAmount;
                Cost = cost;
                ItemCount = 1;
            }

            public void Use(Player player)
            {
                if (ItemCount > 0)
                {
                    switch (Type)
                    {
                        case PotionType.Health:
                            player.Heal(EffectAmount);
                            Console.WriteLine($"{Name}을 사용하여 체력을 {EffectAmount} 회복했습니다");
                            break;
                        case PotionType.Mana:
                            player.RestoreMana(EffectAmount);
                            Console.WriteLine($"{Name}을 사용하여 마나를 {EffectAmount} 회복했습니다");
                            break;
                    }
                    ItemCount--;
                }

                else
                {
                    Console.WriteLine("포션이 부족합니다!");
                }
            }

            public void DisplayItem()
            {
                Console.WriteLine($"[P]{Name} | 효과:{EffectAmount} | 설명: {Description} | 개수: {ItemCount}");
            }
        }
        internal class Player
        {
            public int Health { get; private set; } = 100;
            public int Mana { get; private set; } = 50;

            public void Heal(int amount)
            {
                Health += amount;
                if (Health > 100) Health = 100;
            }

            public void RestoreMana(int amount)
            {
                Mana += amount;
                if (Mana > 50) Mana = 50;
            }

            public void ShowStatus()
            {
                Console.WriteLine($"체력: {Health}, 마나: {Mana}");
            }
        }

        class PotionTest
        {
            public static void RunTest()
            {
                Player player = new Player();
                ConsumableItem healthPotion = new ConsumableItem("체력포션", PotionType.Health, 30, "체력을 회복하는 포션", 50);
                ConsumableItem manaPotion = new ConsumableItem("마나포션", PotionType.Mana, 20, "마나를 회복하는 포션", 40);

                player.ShowStatus();
                healthPotion.Use(player);
                player.ShowStatus();
                manaPotion.Use(player);
                player.ShowStatus();
            }
        }
    }
}