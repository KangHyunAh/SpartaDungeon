﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeon.PotionNamespace;

namespace SpartaDungeon
{
    internal class InventoryAndShop
    {
        public void InventoryScreen(GameManager gm)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gm.player.gold} G");
                Console.WriteLine();
                Console.WriteLine("1.장비 아이템");
                Console.WriteLine("2.소비 아이템");
                Console.WriteLine("0.나가기");
                Console.WriteLine();
                int input = Utility.GetInput(0, 2);
                if (input == 0) break;
                else
                    switch (input)
                    {
                        case 1: EquipScreen(); break;
                        case 2: ConsumableItemInventoryScreen(gm); break;
                    }
            }



            void EquipScreen()
            {
                int ListItemType = 101;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리 -  장착관리");
                    Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine("[장비 아이템 목록]");

                    List<EquipItem> displayItemList = new List<EquipItem>();
                    int index = 0;
                    for (int i = 0; i < gm.equipItemList.Count; i++)
                    {
                        if (gm.equipItemList[i].isEquip && gm.equipItemList[i].Type == ListItemType - 100 - 1)
                        {
                            displayItemList.Add(gm.equipItemList[i]);
                            index++;
                            Console.Write($"  {index,2}.");
                            gm.equipItemList[i].ShowEquipItemList(gm);
                        }
                        if (i == gm.equipItemList.Count - 1 && index == 0)
                        {
                            Utility.ColorText(ConsoleColor.DarkGray, $"  [{(EquipType)(ListItemType - 100 - 1)}] 비어있음");
                        }
                    }
                    Console.WriteLine();
                    for (int i = 0; i < gm.equipItemList.Count; i++)
                    {
                        if (gm.equipItemList[i].ItemCount > 0 && gm.equipItemList[i].Type == ListItemType - 100 - 1)
                        {
                            index++;
                            displayItemList.Add(gm.equipItemList[i]);
                            Console.Write($"{index,2}.");
                            gm.equipItemList[i].ShowEquipItemList(gm);
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("목록바꾸기");
                    Console.WriteLine("101. 무기    102.보조무기  103.머리  104.몸   105.신발");
                    Console.WriteLine("0. 뒤로가기");
                    Console.WriteLine();

                    int input = Utility.GetInputPlus(0, index, new int[] {101,102,103,104,105});
                    if (input == 0) break;
                    else if (input > 100) ListItemType = input;
                    else displayItemList[input - 1].Equip(gm);

                }
            }
        }


        public int DisplayEquipInventory(List<EquipItem> equipItemList, bool hasNum, bool isSaleScreen)   //인벤토리 목록 표시하기 (true 앞숫자 O, false 앞숫자 X), 판매창일경우 True
        {
            int index = 0;
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip)
                {
                    index++; Console.Write(hasNum ? $"{index,2}." : "");
                    equipItemList[i].DisplayEquipItem();
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].ItemCount > 0)
                {
                    index++; Console.Write($"{index,2}.");
                    if (isSaleScreen) equipItemList[i].DisplayShopItem(isSaleScreen); else equipItemList[i].DisplayinventoryItem();
                }
            }
            return index;
        }

        public void ConsumableItemInventoryScreen(GameManager gm)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("소비 아이템을 사용할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[소비 아이템 목록]");

                int index = 0;
                for (int i = 0; i < gm.consumableItemsList.Count; i++)
                {
                    if (gm.consumableItemsList[i].ItemCount != 0)
                    {
                        index++;
                        Console.Write($"{index}.");
                        gm.consumableItemsList[i].DisplayItem();
                    }
                }

                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();

                int input = Utility.GetInput(0, index);
                if (input == 0) break;
                else
                {
                    index = 0;
                    for (int i = 0; i < gm.consumableItemsList.Count; i++)
                    {
                        if (gm.consumableItemsList[i].ItemCount != 0)
                        {
                            index++;
                            if (index == input)
                            {
                                Console.WriteLine($"{gm.consumableItemsList[i].Name}을(를) 사용하시겠습니까?");
                                gm.player.DisplayHpBar();
                                if (gm.consumableItemsList[i].Type == PotionType.Health)
                                {
                                    Utility.ColorText(ConsoleColor.Green, $"{gm.consumableItemsList[i].EffectAmount:+0;-0;}", Text.Write);
                                }

                                Console.WriteLine();

                                gm.player.DisplayMpBar();
                                if (gm.consumableItemsList[i].Type == PotionType.Mana)
                                {
                                    Utility.ColorText(ConsoleColor.Green, $"{gm.consumableItemsList[i].EffectAmount:+0;-0;}", Text.Write);
                                }
                                Console.WriteLine("\n1.사용     0.취소");

                                switch (Utility.GetInput(0, 1))
                                {
                                    case 1: gm.consumableItemsList[i].Use(gm.player); break;
                                    case 0: break;
                                }
                            }
                        }
                    }
                }
            }

        }

        public void ShopScreen(GameManager gm)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gm.player.gold} G");
                Console.WriteLine();
                Console.WriteLine("1. 구매하기");
                Console.WriteLine("2. 판매하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();


                int input = Utility.GetInput(0, 2);
                if (input == 0) break;
                else
                    switch (input)
                    {
                        case 1: BuyScreen(); break;
                        case 2: SaleScreen(); break;
                    }
            }

            void BuyScreen()
            {
                int ListItemType = 101;
                string[] ItemTypeString = { "무기", "보조무기", "머리", "몸", "신발", "소모품" };

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine($"{gm.player.gold} G");
                    Console.WriteLine();
                    Console.Write("[아이템 목록] ");
                    Console.WriteLine($"[{ItemTypeString[ListItemType - 100 - 1]}]");

                    int index = 0;
                    if (ListItemType >= 101 && ListItemType <= 105)
                    {
                        for (int i = 0; i < gm.equipItemList.Count; i++)
                        {
                            if ((gm.equipItemList[i].Type == ListItemType - 100 - 1) && !gm.equipItemList[i].IsBossItem)
                            {
                                index++;
                                Console.Write($"{index,-2}.");
                                gm.equipItemList[i].DisplayShopItem(false);
                                Console.WriteLine();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gm.consumableItemsList.Count; i++)
                        {
                            index++;
                            Console.Write($"{index}.");
                            gm.consumableItemsList[i].DisplayItem();
                        }
                    }



                    Console.WriteLine();
                    Console.WriteLine("목록바꾸기");
                    Console.WriteLine("101. 무기    102.보조무기  103.머리  104.몸   105.신발  106.소모품");
                    Console.WriteLine("0. 뒤로가기");
                    Console.WriteLine();
                    int input = Utility.GetInputPlus(0, index, new int[] { 101, 102, 103, 104, 105, 106 });
                    if (input == 0) break;
                    switch (input)
                    {
                        case 101:
                        case 102:
                        case 103:
                        case 104:
                        case 105:
                        case 106: ListItemType = input; break;
                        default: { if (ListItemType != 106) BuyItem(input); else BuyConsumItem(input - 1); } break;
                    }
                }


                void BuyItem(int input)
                {
                    int index = 0;
                    for (int i = 0; i < gm.equipItemList.Count; i++)
                    {
                        if (gm.equipItemList[i].Type == ListItemType - 100 - 1)
                        {
                            index++;
                            if (index == input)
                            {
                                if (gm.equipItemList[i].Cost > gm.player.gold)
                                {
                                    Console.WriteLine("소지금이 부족합니다.");
                                    Console.WriteLine($"필요 :{gm.equipItemList[i].Cost,-5}G  현재 소지금 :{gm.player.gold,6}G");
                                }
                                else
                                {
                                    gm.player.gold -= gm.equipItemList[i].Cost;
                                    gm.equipItemList[i].ItemCount++;
                                    Console.WriteLine($"아이템을 구매하였습니다.  -{gm.equipItemList[i].Cost}G");
                                    Console.WriteLine($"현재 소지금 :{gm.player.gold,-6}G");
                                    Console.WriteLine();

                                }
                                Console.WriteLine("(아무키입력)");
                                Console.ReadLine();
                            }
                        }
                    }

                }
                void BuyConsumItem(int input)
                {
                    if (gm.consumableItemsList[input].Cost > gm.player.gold)
                    {
                        Console.WriteLine("소지금이 부족합니다.");
                        Console.WriteLine($"필요 :{gm.consumableItemsList[input].Cost,-5}G  현재 소지금 :{gm.player.gold,6}G");
                    }
                    else
                    {
                        gm.player.gold -= gm.consumableItemsList[input].Cost;
                        gm.consumableItemsList[input].ItemCount++;
                        Console.WriteLine($"아이템을 구매하였습니다.  -{gm.consumableItemsList[input].Cost}G");
                        Console.WriteLine($"현재 소지금 :{gm.player.gold,-6}G");
                        Console.WriteLine();

                    }
                    Console.WriteLine("(아무키입력)");
                    Console.ReadLine();
                }

            }

            void SaleScreen()
            {
                int SaleItemType = 101;
                string[] ItemTypeString = { "무기", "보조무기", "머리", "몸", "신발", "소모품" };

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("상점");
                    Console.WriteLine("아이템을 판매합니다. (판매가는 원래 가격의 절반이 됩니다.)");
                    Console.WriteLine();
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine($"{gm.player.gold} G");
                    Console.WriteLine();
                    Console.Write("[인벤토리 목록] ");
                    Console.WriteLine($"[{ItemTypeString[SaleItemType - 100 - 1]}]");

                    int index = 0;
                    if (SaleItemType >= 101 && SaleItemType <= 105)
                    {
                        for (int i = 0; i < gm.equipItemList.Count; i++)
                        {
                            if (gm.equipItemList[i].isEquip && gm.equipItemList[i].Type == SaleItemType - 100 - 1)
                            {
                                index++; Console.Write($"{index,2}");
                                gm.equipItemList[i].DisplayEquipItem();
                            }
                        }
                        Console.WriteLine();
                        for (int i = 0; i < gm.equipItemList.Count; i++)
                        {
                            if (gm.equipItemList[i].ItemCount > 0 && gm.equipItemList[i].Type == SaleItemType - 100 - 1)
                            {
                                index++; Console.Write($"{index}.");
                                gm.equipItemList[i].DisplayShopItem(true);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gm.consumableItemsList.Count; i++)
                        {
                            if (gm.consumableItemsList[i].ItemCount > 0)
                            {
                                index++; Console.Write($"{index}.");
                                gm.consumableItemsList[i].DisplayItem();
                            }
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("목록바꾸기");
                    Console.WriteLine("101. 무기    102.보조무기  103.머리  104.몸   105.신발  106.소모품");
                    Console.WriteLine("0. 뒤로가기");
                    Console.WriteLine();

                    int input = Utility.GetInputPlus(0, index, new int[] { 101, 102, 103, 104, 105, 106 });
                    if (input == 0) break;
                    switch (input)
                    {
                        case 101:
                        case 102:
                        case 103:
                        case 105:
                        case 106: SaleItemType = input; SaleScreen(); break;
                        default: { if (SaleItemType != 106) SaleItem(input); else SaleConsumItem(input); } break;
                    }

                    void SaleItem(int input)
                    {
                        while (true)
                        {
                            int index = 0;
                            for (int i = 0; i < gm.equipItemList.Count; i++)
                            {
                                if (gm.equipItemList[i].isEquip && gm.equipItemList[i].Type == SaleItemType - 100 - 1)       //장착중인 아이템을 선택했을경우. 장착해제, 판매금(가격/2)받기
                                {
                                    index++;
                                    if (index == input)
                                    {
                                        Console.WriteLine($"{gm.equipItemList[i].Name} 판매가 {gm.equipItemList[i].Cost / 2}G");
                                        Console.WriteLine("장착중인 아이템입니다! 해제하고 판매하시겠습니까?\n");
                                        Console.WriteLine("1.해제 후 판매      2.취소\n");
                                        switch (Utility.GetInput(1, 2))
                                        {
                                            case 1: { gm.equipItemList[i].isEquip = false; gm.player.gold += gm.equipItemList[i].Cost / 2; UpdateEquipStatus(gm); } break;
                                            case 2: break;
                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < gm.equipItemList.Count; i++)   //장착중이 아닌 아이템을 선택했을경우. 판매
                            {
                                if (gm.equipItemList[i].ItemCount > 0 && gm.equipItemList[i].Type == SaleItemType - 100 - 1)
                                {
                                    index++;
                                    if (index == input)
                                    {
                                        gm.equipItemList[i].ItemCount--;
                                        gm.player.gold += gm.equipItemList[i].Cost / 2;
                                    }
                                }
                            }
                            Console.WriteLine("아이템을 판매하였습니다. (아무키입력)");
                            Console.ReadLine();
                        }

                    }
                }






                void SaleConsumItem(int input)
                {
                    int index = 0;
                    for (int i = 0; i < gm.consumableItemsList.Count; i++)
                    {
                        if (gm.consumableItemsList[i].ItemCount != 0)
                        {
                            index++;
                            if (index == input)
                            {
                                Console.Clear();
                                Console.WriteLine($"{gm.consumableItemsList[i].Name} 이 아이템을 판매하시겠습니까?");
                                Console.WriteLine($"판매가 {gm.consumableItemsList[i].Cost / 2}G |소지수 {gm.consumableItemsList[i].ItemCount} |소지금 {gm.player.gold}G");
                                Console.WriteLine("1.판매     2.취소");
                                switch (Utility.GetInput(1, 2))
                                {
                                    case 1: { gm.player.gold += gm.consumableItemsList[i].Cost / 2; gm.consumableItemsList[i].ItemCount--; } break;
                                    case 2: break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateEquipStatus(GameManager gm)     //장비에따른 스텟 업데이트
        {
            gm.player.equipStrikePower = 0; gm.player.equipDefensivePower = 0; gm.player.equipMaxhealthPoint = 0;
            for (int i = 0; i < gm.equipItemList.Count; i++)
            {
                if (gm.equipItemList[i].isEquip)
                {
                    gm.player.equipStrikePower += gm.equipItemList[i].Atk;
                    gm.player.equipDefensivePower += gm.equipItemList[i].Def;
                    gm.player.equipMaxhealthPoint += gm.equipItemList[i].MaxHp;
                }
            }
        }
    }
}
