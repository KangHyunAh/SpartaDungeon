using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
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

        public void Equip(GameManager gm)   //아이템 장착
        {
            if (JobLimit.Contains(gm.player.chad))      //직업제한여부 검색
            {
                EquipItem equipedItem = gm.equipItemList.FirstOrDefault(item => item.isEquip == true && item.Type == Type);     //같은 부위에 이미 장착된 장비의 유무
                if (equipedItem == null) { isEquip = true; ItemCount--; }                                                   //이미 장착된 장비가 없다면 = 해당 아이템 장착
                else if (equipedItem == this) { isEquip = false; ItemCount++; }                                             //이미 장착된 장비가 자신이라면 = 해당 아이템 장착해제
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n장착중인 장비가 있습니다. 해제하고 장착하시겠습니까?\n");

                    Utility.RealTabWrite($"{equipedItem.Name}", true, 18);
                    Console.Write("==> ");
                    Utility.RealTabWrite($"{Name}", true, 18); Console.WriteLine("\n");

                    Utility.RealTabWrite($"[atk{equipedItem.Atk,3:+0;-0}]", true, 18); Console.Write("==> "); Utility.RealTabWrite($"[atk{Atk,3:+0;-0}]", true, 9);
                    if (equipedItem.Atk > Atk) Utility.ColorText(ConsoleColor.Red, $"{(Atk - equipedItem.Atk),3:+0;-0}", Text.Write);
                    else if (equipedItem.Atk < Atk) Utility.ColorText(ConsoleColor.Cyan, $"{(Atk - equipedItem.Atk),3:+0;-0}", Text.Write);
                    Console.WriteLine();
                    Utility.RealTabWrite($"[def{equipedItem.Def,3:+0;-0}]", true, 18); Console.Write("==> "); Utility.RealTabWrite($"[def{Def,3:+0;-0}]", true, 9);
                    if (equipedItem.Def > Def) Utility.ColorText(ConsoleColor.Red, $"{(Def - equipedItem.Def),3:+0;-0}", Text.Write);
                    else if (equipedItem.Def < Def) Utility.ColorText(ConsoleColor.Cyan, $"{(Def - equipedItem.Def),3:+0;-0}", Text.Write);
                    Console.WriteLine();
                    Utility.RealTabWrite($"[maxHP{equipedItem.MaxHp,3:+0;-0}]", true, 18); Console.Write("==> "); Utility.RealTabWrite($"[maxHP{MaxHp,4:+0;-0}]", true, 9);
                    if (equipedItem.MaxHp > MaxHp) Utility.ColorText(ConsoleColor.Red, $"{(MaxHp - equipedItem.MaxHp),4:+0;-0}", Text.Write);
                    else if (equipedItem.MaxHp < MaxHp) Utility.ColorText(ConsoleColor.Cyan, $"{(MaxHp - equipedItem.MaxHp),4:+0;-0}", Text.Write);
                    Console.WriteLine();

                    Console.WriteLine("\n1.장착      0.취소");

                    int input = Utility.GetInput(0, 1);
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

        public void SaleEquipItem(GameManager gm)
        {
            Console.Clear();
            Console.WriteLine("이 아이템을 판매합니까?\n");
            Console.Write($"{Name}  판매가 "); Utility.ColorText(ConsoleColor.White, $"{Cost / 2}G",Text.Write); Console.WriteLine($"    | 소지금 {gm.player.gold}G");
            Console.WriteLine("\n1.판매     0.취소");
            if(Utility.GetInput(0, 1) == 1)
            {
                ItemCount--;
                gm.player.gold += Cost / 2;
                Console.Write("판매완료  소지금 "); Utility.ColorText(ConsoleColor.Yellow, $"{gm.player.gold,5}G ", Text.Write); Utility.ColorText(ConsoleColor.Green, $"+{Cost / 2}");
                Console.Write("아무키입력"); Console.ReadLine();
            }
        }
        public void BuyEquipItem(GameManager gm)
        {
            if (Cost > gm.player.gold)
            {
                Console.WriteLine("소지금이 부족합니다.");
                Console.WriteLine($"필요 :{Cost,-5}G  현재 소지금 :{gm.player.gold,6}G");
            }
            else
            {
                gm.player.gold -= Cost;
                ItemCount++;
                Console.WriteLine($"아이템을 구매하였습니다.  -{Cost}G");
                Console.WriteLine($"현재 소지금 :{gm.player.gold,-6}G");
                Console.WriteLine();

            }
            Console.WriteLine("(아무키입력)");
            Console.ReadLine();
        }

        public void DisplayEquipItemList(GameManager gm,bool hideCount = false, bool isBuy = false, bool isSale = false) //장비아이템 정보 출력 (이름, 부위, 설명 / 개수, 가격, 스텟, 스텟증감, 착욕가능직업)
        {   //첫째줄
            if (isEquip) { Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("[E]"); } 
            Utility.RealTabWrite($"{Name}", true, 16); 
            Utility.RealTabWrite($"[{(EquipType)Type}] ", true, 7); 
            Console.WriteLine($" - {Description}");
            if (isEquip) Console.ResetColor();
            
            //둘째줄
            if (ItemCount >= 1 && !hideCount) Console.Write($"  소지X{ItemCount,2} "); else Console.Write("          ");

            if (isBuy) 
            {
                Console.ForegroundColor = Cost <= gm.player.gold ? ConsoleColor.White : ConsoleColor.DarkGray;
                Utility.RealTabWrite($"{Cost}G |", false, 10); Console.ResetColor();
            }
            else if (isSale)
            {
                Console.ForegroundColor = ConsoleColor.White; Utility.RealTabWrite($"판매가{Cost / 2,5}G |", false, 15); Console.ResetColor();
            }
            else Console.Write("         |");
            if (Atk != 0) Utility.RealTabWrite($"[atk{Atk,3:+0;-0}]", true, 9); else Console.Write("         ");   //스텟
            if (Def != 0) Utility.RealTabWrite($"[def{Def,3:+0;-0}]", true, 9); else Console.Write("         ");
            if (MaxHp != 0) Utility.RealTabWrite($"[maxHP{MaxHp,4:+0;-0}]", true, 10); else Console.Write("          ");
            Console.Write("|");
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

           
            public bool Use(Player player)
            {
                bool useSuccess=false;
                Console.Clear();
                Console.WriteLine($"{Name}을(를) 사용하시겠습니까?");
                player.DisplayHpBar();
                if (Type == PotionType.Health)
                {
                    Utility.ColorText(ConsoleColor.Green, $"{EffectAmount:+0;-0;}", Text.Write);
                }

                Console.WriteLine();

                player.DisplayMpBar();
                if (Type == PotionType.Mana)
                {
                    Utility.ColorText(ConsoleColor.Green, $"{EffectAmount:+0;-0;}", Text.Write);
                }
                Console.WriteLine("\n1.사용     0.취소");

                if (Utility.GetInput(0, 1) == 1)
                {
                    switch (Type)
                    {
                        case PotionType.Health:
                            if (player.healthPoint < player.maxhealthPoint + player.equipMaxhealthPoint)
                            {
                                player.healthPoint += EffectAmount; ItemCount--;
                                player.DisplayHpBar();
                                Console.WriteLine($"\n{Name}을 사용하여 체력을 {EffectAmount} 회복했습니다");
                                useSuccess = true;
                            }
                            else Console.WriteLine("이미 최대체력입니다!");
                            break;
                        case PotionType.Mana:
                            if (player.manaPoint < player.maxManaPoint)
                            {
                                player.manaPoint += EffectAmount; ItemCount--;
                                player.DisplayMpBar();
                                Console.WriteLine($"\n{Name}을 사용하여 마나를 {EffectAmount} 회복했습니다");
                                useSuccess = true;
                            }
                            else Console.WriteLine("이미 최대마나입니다!");
                            break;
                    }
                    Console.WriteLine("아무키 입력");
                    Console.ReadLine();
                    
                }
                return useSuccess;

            }
            public void BuyConsumItem(Player player)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"{Name}을(를) 구매합니다.");
                Console.WriteLine();
                Utility.ColorText(ConsoleColor.White, $"가격 {Cost}G", Text.Write); Console.Write($" |소지수 {ItemCount} |"); Utility.ColorText(ConsoleColor.Yellow, $"소지금 {player.gold}G");
                Console.WriteLine();
                Console.WriteLine($"구매할 개수를 입력해주세요. (최대 {player.gold/Cost})     0.판매취소");
                Console.WriteLine();
                int input = Utility.GetInput(0, player.gold/Cost);
                if (input != 0) { player.gold -= Cost * input; ItemCount += input; }
            }
            public void SaleConsumItem(Player player)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"{Name}을(를) 판매합니다.");
                Console.WriteLine();
                Utility.ColorText(ConsoleColor.White, $"판매가 {Cost / 2}G",Text.Write); Console.Write($" |소지수 {ItemCount} |"); Utility.ColorText(ConsoleColor.Yellow, $"소지금 {player.gold}G");
                Console.WriteLine();
                Console.WriteLine($"판매할 개수를 입력해주세요. (최대 {ItemCount})     0.판매취소");
                Console.WriteLine();
                int input = Utility.GetInput(0, ItemCount);
                if (input != 0 ) { player.gold += (Cost / 2)*input; ItemCount-=input; }
            }

            public void DisplayItem(bool isSale=false)
            {
                Console.WriteLine($"[P]{Name} | 효과:{EffectAmount,3} | 설명: {Description}");
                Console.Write(!isSale ? "       |가격 " : "       |판매가 "); 
                Utility.ColorText(ConsoleColor.White, $"{(!isSale ? Cost : Cost/2 ),5}G ", Text.Write); 
                Console.WriteLine($"|소지 X{ItemCount,2}");
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