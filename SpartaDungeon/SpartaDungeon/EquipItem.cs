﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeon;
using PotionPlayer = SpartaDungeon.PotionNamespace.PotionPlayer;


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
        public int Type { get; }
        public string Description { get; }
        public string[] JobLimit {  get; }
        public int Atk { get; }
        public int Def { get; }
        public int MaxHp { get; }
        public int Cost { get; }
        private int _itemCount;
        public int ItemCount { get { return _itemCount; } set { if (value < 0) _itemCount = 0; else _itemCount = value; } }
        public bool isEquip { get; set; }
        public bool IsBossItem {  get; set; }

        public EquipItem(string name, EquipType type, int atk, int def, int maxHp, string discription,string[] jobLimit ,int cost,bool isBossItem)
        {
            Name = name;
            Type = (int)type;
            JobLimit = jobLimit;
            Description = discription;
            Atk = atk;
            Def = def;
            MaxHp = maxHp;
            Cost = cost;
            ItemCount = 0;
            isEquip = false;
            IsBossItem = isBossItem;
        }
        public void DisplayEquipItem()
        {
            Console.Write("[E]"); Utility.RealTab($"{Name}", true, 16);Console.Write("|");Utility.RealTab(JobLimit.Length == 3 ? $"[공용]" : $"[{JobLimit[0]}전용]", true, 12);Console.Write($"|{Description}\n");
            Console.Write("\t");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine();
        }
        public void DisplayinventoryItem() 
        {
            Utility.RealTab($"{Name}", true, 14); Console.Write($"|{Description}\n");
            Console.Write($"   소지 {ItemCount,-2}개|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine() ;
            Console.WriteLine();
        }
        public void DisplayShopItem(bool isSale) //true일경우 판매가 표시 false경우 구매가 표시
        {
            Console.WriteLine($"{Name,-8} |" + (JobLimit.Length == 3 ? "[공용]" : $"[{JobLimit[0]}전용]")+$" |{Description}");
            Console.Write(!isSale?$"    가격 {Cost,5}":$"\t판매가 {Cost/2,-5}");
            Console.Write($"|소지 개수 {ItemCount,-2}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine();
        }
        
    }

    public enum PotionType
    {
        Health,
        Mana,
    }

    namespace PotionNamespace
    {


        public class ConsumableItem
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
                ItemCount = 0;
            }

            public void Use(Player player)
            {
                if (ItemCount > 0)
                {
                    switch (Type)
                    {
                        case PotionType.Health:
                            if (player.healthPoint+EffectAmount > player.maxhealthPoint + player.equipMaxhealthPoint) player.healthPoint = player.maxhealthPoint + player.equipMaxhealthPoint; else player.healthPoint += EffectAmount;
                            player.DisplayHpBar();
                            Console.WriteLine($"\n{Name}을 사용하여 체력을 {EffectAmount} 회복했습니다");
                            break;
                        case PotionType.Mana:
                            if (player.manaPoint +EffectAmount > player.maxManaPoint) player.manaPoint = player.maxManaPoint; else player.healthPoint += EffectAmount;
                            player.DisplayMpBar();
                            Console.WriteLine($"\n{Name}을 사용하여 마나를 {EffectAmount} 회복했습니다");
                            break;
                    }
                    ItemCount--;
                    Console.WriteLine("아무키 입력");
                    Console.ReadLine();
                }

                else
                {
                    Console.WriteLine("포션이 부족합니다!");
                }
            }

            public void DisplayItem()
            {
                Console.WriteLine($"[P]{Name} | 효과:{EffectAmount} | 설명: {Description} |가격: {Cost} |개수: {ItemCount}");
            }
        }
        public class PotionPlayer
        {
            public int Health { get; private set; } = 100;
            public int Mana { get; private set; } = 50;

            public void Heal(int amount)
            {
                Health += amount;
                if (Health < 100) Health = 100;
            }

            public void RestoreMana(int amount)
            {
                Mana += amount;
                if (Mana < 50) Mana = 50;
            }

            public void ShowStatus()
            {
                Console.WriteLine($"체력: {Health}, 마나: {Mana}");
            }
        }

        //class PotionTest
        //{
        //    public static void RunTest()
        //    {
        //        PotionPlayer player = new PotionPlayer();
        //        ConsumableItem healthPotion = new ConsumableItem("체력포션", PotionType.Health, 30, "체력을 회복하는 포션", 50);
        //        ConsumableItem manaPotion = new ConsumableItem("마나포션", PotionType.Mana, 20, "마나를 회복하는 포션", 40);

        //        player.ShowStatus();
        //        healthPotion.Use(player);
        //        player.ShowStatus();
        //        manaPotion.Use(player);
        //        player.ShowStatus();
        //    }
        //}
    }
}