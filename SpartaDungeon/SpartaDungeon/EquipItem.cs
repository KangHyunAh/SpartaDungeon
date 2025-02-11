using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeon;
using PotionPlayer = SpartaDungeon.PotionNamespace.PotionPlayer;


namespace SpartaDungeon
{
    public enum EquipType
    {
        무기,
        보조,
        머리,
        몸,
        신발
    }
    internal class EquipItem

    {
        public string Name { get; }
        public int Type { get; }
        public string Description { get; }
        public string[] JobLimit { get; }
        public int Atk { get; }
        public int Def { get; }
        public int MaxHp { get; }
        public int Cost { get; }
        private int _itemCount;
        public int ItemCount { get { return _itemCount; } set { if (value < 0) _itemCount = 0; else _itemCount = value; } }
        public bool isEquip { get; set; }
        public bool IsBossItem { get; set; }

        public EquipItem(string name, EquipType type, int atk, int def, int maxHp, string discription, string[] jobLimit, int cost, bool isBossItem)
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

        public void Equip(GameManager gm)
        {
            if (JobLimit.Contains(gm.player.chad))
            {
                EquipItem equipedItem = gm.equipItemList.FirstOrDefault(item => item.isEquip == true && item.Type == Type);
                if (equipedItem == null) { isEquip = true; ItemCount--; }
                else if (equipedItem == this) { isEquip = false; ItemCount++; }
                else
                {
                    Console.Clear();
                    Console.WriteLine("장착중인 장비가 있습니다. 해제하고 장착하시겠습니까?");

                    Utility.RealTabWrite($"{equipedItem.Name}", true, 18);
                    Console.Write("==>");
                    Utility.RealTabWrite($"{Name}", true, 18); Console.WriteLine();

                    Utility.RealTabWrite($"[atk{equipedItem.Atk,3:+0;-0}]", true, 18); Console.Write("==>"); Utility.RealTabWrite($"[atk{Atk,3:+0;-0}]", true, 9);
                    if (equipedItem.Atk > Atk) Utility.ColorText(ConsoleColor.Red, $"{(Atk - equipedItem.Atk),3:+0;-0}", Text.Write);
                    else if (equipedItem.Atk < Atk) Utility.ColorText(ConsoleColor.Green, $"{(Atk - equipedItem.Atk),3:+0;-0}", Text.Write);
                    Console.WriteLine();
                    Utility.RealTabWrite($"[def{equipedItem.Def,3:+0;-0}]", true, 18); Console.Write("==>"); Utility.RealTabWrite($"[def{Def,3:+0;-0}]", true, 9);
                    if (equipedItem.Def > Def) Utility.ColorText(ConsoleColor.Red, $"{(Def - equipedItem.Def),3:+0;-0}", Text.Write);
                    else if (equipedItem.Def < Def) Utility.ColorText(ConsoleColor.Green, $"{(Def - equipedItem.Def),3:+0;-0}", Text.Write);
                    Console.WriteLine();
                    Utility.RealTabWrite($"[maxHP{equipedItem.MaxHp,3:+0;-0}]", true, 18); Console.Write("==>"); Utility.RealTabWrite($"[maxHP{MaxHp,4:+0;-0}]", true, 9);
                    if (equipedItem.MaxHp > MaxHp) Utility.ColorText(ConsoleColor.Red, $"{(MaxHp - equipedItem.MaxHp),4:+0;-0}", Text.Write);
                    else if (equipedItem.MaxHp < MaxHp) Utility.ColorText(ConsoleColor.Green, $"{(MaxHp - equipedItem.MaxHp),4:+0;-0}", Text.Write);
                    Console.WriteLine();

                    Console.WriteLine("1.장착      2.취소");

                    int input = Utility.GetInput(1, 2);
                    if (input == 1)
                    {
                        equipedItem.isEquip = false;
                        equipedItem.ItemCount++;
                        isEquip = true;
                        ItemCount--;
                    }
                }
                gm.inventoryAndShop.UpdateEquipStatus(gm);
            }
            else
            {
                Utility.ColorText(ConsoleColor.Red, "해당 장비를 착용할 수 없는 직업입니다.");
                Console.Write("아무키입력");
                Console.ReadLine();
            }
        }
        public void DisplayEquipItem()
        {
            Console.Write("[E]"); Utility.RealTabWrite($"{Name}", true, 16); Console.Write("|"); Utility.RealTabWrite(JobLimit.Length == 3 ? $"[공용]" : $"[{JobLimit[0]}전용]", true, 12); Console.Write($"|{Description}\n");
            Console.Write("\t");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine();
        }
        public void DisplayinventoryItem()
        {
            Utility.RealTabWrite($"{Name}", true, 14); Console.Write($"|{Description}\n");
            Console.Write($"   소지 {ItemCount,-2}개|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine();
            Console.WriteLine();
        }
        public void DisplayShopItem(bool isSale) //true일경우 판매가 표시 false경우 구매가 표시
        {
            Console.WriteLine($"{Name,-8} |" + (JobLimit.Length == 3 ? "[공용]" : $"[{JobLimit[0]}전용]") + $" |{Description}");
            Console.Write(!isSale ? $"    가격 {Cost,5}" : $"\t판매가 {Cost / 2,-5}");
            Console.Write($"|소지 개수 {ItemCount,-2}|");
            if (Atk != 0) Console.Write($"atk{Atk,4:+0;-0;0}|");
            if (Def != 0) Console.Write($"def{Def,4:+0;-0;0}|");
            if (MaxHp != 0) Console.Write($"maxHp{MaxHp,5:+0;-0;0}|");
            Console.WriteLine();
        }

        public void ShowEquipItemList(GameManager gm, bool isShop = false, bool isSale = false) //장비아이템 정보 출력 (이름, 부위, 설명 / 가격, 스텟, 스텟증감, 착욕가능직업)
        {
            if (isEquip) Console.Write("[E]"); Utility.RealTabWrite($"{Name}", true, 16); Utility.RealTabWrite($"|[{(EquipType)Type}]", true, 7); Console.WriteLine($"|{Description}");//첫줄
            //둘째줄
            if (ItemCount >= 2) Console.Write($"X{ItemCount,2}");
            if (isShop) Utility.RealTabWrite($"{Cost}G |", true, 16); else if (isSale) Utility.RealTabWrite($"{Cost / 2}G |", true, 16);
            if (Atk != 0) Utility.RealTabWrite($"[atk{Atk,3:+0;-0}]", true, 11);    //스텟
            if (Def != 0) Utility.RealTabWrite($"[def{Def,3:+0;-0}]", true, 11);
            if (MaxHp != 0) Utility.RealTabWrite($"[maxHP{MaxHp,4:+0;-0}]", true, 12);
            Console.Write("  ");
            int deltaAtk, deltaDef, deltaMaxHp;
            EquipItem equipedItem = gm.equipItemList.FirstOrDefault(item => item.isEquip == true && item.Type == Type);
            if (equipedItem == null)
            {
                deltaAtk = Atk; deltaDef = Def; deltaMaxHp = MaxHp;
            }
            else
            {
                deltaAtk = Atk - equipedItem.Atk; deltaDef = Def - equipedItem.Def; deltaMaxHp = MaxHp - equipedItem.MaxHp;
            }
            Console.Write("능력치 증감");
            if (deltaAtk == 0) Utility.ColorText(ConsoleColor.DarkGray, $"[atk +0] ", Text.Write);
            else if (deltaAtk > 0) Utility.ColorText(ConsoleColor.Cyan, $"[atk{deltaAtk,3:+0;-0}] ", Text.Write);
            else Utility.ColorText(ConsoleColor.Red, $"[atk{deltaAtk,3:+0;-0}] ", Text.Write);

            if (deltaDef == 0) Utility.ColorText(ConsoleColor.DarkGray, $"[def +0] ", Text.Write);
            else if (deltaDef > 0) Utility.ColorText(ConsoleColor.Cyan, $"[def{deltaDef,3:+0;-0}] ", Text.Write);
            else Utility.ColorText(ConsoleColor.Red, $"[def{deltaDef,3:+0;-0}] ", Text.Write);

            if (deltaMaxHp == 0) Utility.ColorText(ConsoleColor.DarkGray, $"[maxHP +0] ", Text.Write);
            else if (deltaMaxHp > 0) Utility.ColorText(ConsoleColor.Cyan, $"[maxHP{deltaMaxHp,3:+0;-0}] ", Text.Write);
            else Utility.ColorText(ConsoleColor.Red, $"[maxHP{deltaMaxHp,3:+0;-0}] ", Text.Write);

            Console.Write("  ");

            if (JobLimit.Length == 3) Console.Write("[공용]");
            else
            {
                for (int i = 0; i < JobLimit.Length; i++)
                {
                    if (JobLimit.Contains(gm.player.chad)) Console.Write($"[{JobLimit[i]} 장착가능] ");
                    else Utility.ColorText(ConsoleColor.Red, $"[{JobLimit[i]} 장착가능]", Text.Write);
                }
            }
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
                            if (player.healthPoint + EffectAmount > player.maxhealthPoint + player.equipMaxhealthPoint) player.healthPoint = player.maxhealthPoint + player.equipMaxhealthPoint; else player.healthPoint += EffectAmount;
                            player.DisplayHpBar();
                            Console.WriteLine($"\n{Name}을 사용하여 체력을 {EffectAmount} 회복했습니다");
                            break;
                        case PotionType.Mana:
                            if (player.manaPoint + EffectAmount > player.maxManaPoint) player.manaPoint = player.maxManaPoint; else player.healthPoint += EffectAmount;
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