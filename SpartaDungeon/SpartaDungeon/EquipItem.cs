using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.ConsumableItem;


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
        private int _itemCount;
        public int ItemCount { get { return _itemCount; } set { if (value < 0) _itemCount = 0; else _itemCount = value; } }
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
        public void DisplayinventoryItem()    //true면 판매가도표시
        {
            Console.Write($"   {Name,-8}|배낭 안 소지수{ItemCount,-2}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine($"{Description}");
        }
        public void DisplayShopItem(bool isSale) //true일경우 판매가 표시 false경우 구매가 표시
        {
            Console.Write($"   {Name,-8}|");
            Console.Write(!isSale?$"가격 {Cost,5}":$"판매가 {Cost/2,-5}");
            Console.Write($"|배낭 안 소지수{ItemCount,-2}");
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

    internal class ConsumableItem : IConsumable
    {

        public interface IConsumable
        {
            void Use(Player player);
        }
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
                       // player.Heal(EffectAmount);
                        Console.WriteLine($"{Name}을 사용하여 체력을 {EffectAmount} 회복했습니다");
                        break;
                    case PotionType.Mana:
                       // player.RestoreMana(EffectAmount);
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
}