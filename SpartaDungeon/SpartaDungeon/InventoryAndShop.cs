using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
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
                Utility.ColorText(ConsoleColor.White, $"{gm.player.gold} G");
                Console.WriteLine();
                Console.WriteLine("1.아이템 목록 보기");
                Console.WriteLine("0.나가기");
                Console.WriteLine();
                int input = Utility.GetInput(0, 1);
                if (input == 0) break;
                else
                    switch (input)
                    {
                        case 1: EquipScreen(gm); break;
                    }
            }
        }
        public void EquipScreen(GameManager gm, bool isSaleScreen = false)
        {
            int ListItemType = 101;
            int page = 1;

            while (true)
            {
                List<EquipItem> displayItemList = new List<EquipItem>();
                List<ConsumableItem> displayConsumList = new List<ConsumableItem>();
                for (int i = 0; i < gm.consumableItemsList.Count; i++)
                {
                    if (gm.consumableItemsList[i].ItemCount > 0) { displayConsumList.Add(gm.consumableItemsList[i]); }
                }
                for (int i = 0; i < gm.equipItemList.Count; i++)
                {
                    if (gm.equipItemList[i].ItemCount > 0 && gm.equipItemList[i].Type == ListItemType - 100 - 1) displayItemList.Add(gm.equipItemList[i]);
                }
                EquipItem equipedItem = gm.equipItemList.FirstOrDefault(item => item.isEquip == true && item.Type == ListItemType - 100 - 1);

                Console.Clear();
                Console.WriteLine(!isSaleScreen ? "인벤토리 -  장착관리" : "상점 - 아이템 판매");
                Console.WriteLine(!isSaleScreen ? "보유중인 아이템을 관리할 수 있습니다." : "아이템을 판매합니다. (판매가는 원래 가격의 절반이 됩니다.)");
                Console.WriteLine(ListItemType != 106 ? $"[{(EquipType)(ListItemType - 100 - 1)}]" : $"[소모품]");

                if (ListItemType != 106)
                {
                    Console.WriteLine("[장착중]");
                    Console.Write("11.");
                    if (equipedItem == null) Utility.ColorText(ConsoleColor.DarkGray, $"   [{(EquipType)(ListItemType - 100 - 1)}] 빈 슬롯\n");
                    else equipedItem.DisplayEquipItemList(gm, true);

                    Console.WriteLine();

                    Console.WriteLine("[장비 인벤토리 목록]");
                    for (int i = 0; i < 5; i++)
                    {
                        if (i < displayItemList.Count - (5 * (page - 1)))
                        {
                            Console.Write($" {i + 1}.");
                            displayItemList[i + (5 * (page - 1))].DisplayEquipItemList(gm,false,isSaleScreen,isSaleScreen);
                            Console.WriteLine();
                        }
                        else Console.WriteLine("\n\n");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("[소모품 인벤토리 목록]");
                    for (int i = 0; i < 6; i++)
                    {
                        if (i < displayConsumList.Count - (6 * (page - 1)))
                        {
                            Console.Write($" {i + 1}.");
                            displayConsumList[i + (6 * (page - 1))].DisplayItem(isSaleScreen);
                            Console.WriteLine();
                        }
                        else Console.WriteLine("\n\n");
                    }

                }

                if (ListItemType != 106)
                {
                    Utility.ColorText(page == 1 ? ConsoleColor.DarkGray : ConsoleColor.White, "            <<이전페이지(22)", Text.Write);
                    Utility.ColorText(ConsoleColor.White, $"    페이지 {page,2}/{(displayItemList.Count / 5) + 1 - (displayItemList.Count != 0 && displayItemList.Count % 5 == 0 ? 1 : 0),2}    ", Text.Write);
                    Utility.ColorText(page == (displayItemList.Count / 5) + 1 - (displayItemList.Count != 0 && displayItemList.Count % 5 == 0 ? 1 : 0) ? ConsoleColor.DarkGray : ConsoleColor.White, "    (33)다음페이지>>", Text.Write);
                }
                else
                {
                    Utility.ColorText(page == 1 ? ConsoleColor.DarkGray : ConsoleColor.White, "            <<이전페이지(22)", Text.Write);
                    Utility.ColorText(ConsoleColor.White, $"    페이지 {page,2}/{(displayConsumList.Count / 6) + 1 - (displayConsumList.Count != 0 && displayConsumList.Count % 6 == 0 ? 1 : 0),2}    ", Text.Write);
                    Utility.ColorText(page == (displayConsumList.Count / 6) + 1 - ((displayConsumList.Count !=0 && displayConsumList.Count % 6 == 0)? 1 : 0) ? ConsoleColor.DarkGray : ConsoleColor.White, "    (33)다음페이지>>", Text.Write);

                }

                Console.WriteLine();
                Console.WriteLine("목록바꾸기");
                Console.WriteLine("101. 무기    102.보조무기  103.머리  104.몸   105.신발   106.소모품");
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine();

                int input = Utility.GetInputPlus(0, 5, ListItemType == 106 ? new int[] { 22, 33, 101, 102, 103, 104, 105, 106 } : new int[] { 11, 22, 33, 101, 102, 103, 104, 105, 106 });
                if (input == 0) break;
                else if (input > 100) { ListItemType = input; page = 1; }
                else if (input == 11)
                {
                    if (equipedItem != null)
                    {
                        if (!isSaleScreen) equipedItem.Equip(gm);
                        else
                        {
                            Console.Clear();
                            Utility.ColorText(ConsoleColor.Red, "\n장착중인 아이템입니다!", Text.Write);
                            Console.WriteLine(" 해제하고 판매하시겠습니까?\n");
                            Console.Write($"{equipedItem.Name}   판매가 ");
                            Utility.ColorText(ConsoleColor.White, $"{equipedItem.Cost / 2}G", Text.Write);
                            Console.WriteLine($"    | 소지금 {gm.player.gold}G");
                            Console.WriteLine("\n1.해제 후 판매      0.취소");
                            if (Utility.GetInput(0, 1) == 1)
                            {
                                equipedItem.isEquip = false; gm.player.gold += (displayItemList[input - 1].Cost / 2);
                                Console.Write("판매완료  소지금 "); Utility.ColorText(ConsoleColor.Yellow, $"{gm.player.gold,5}G ", Text.Write); Utility.ColorText(ConsoleColor.Green, $"+{equipedItem.Cost / 2}");
                                Console.Write("아무키입력"); Console.ReadLine();
                            }
                        }
                    }
                }
                else if (input == 22) page = (page == 1 ? 1 : page - 1);
                else if (input == 33)
                {
                    if (ListItemType != 106) page = (page == ((displayItemList.Count / 5) + 1 - (displayItemList.Count!=0 && displayItemList.Count % 5 == 0 ? 1 : 0)) ? page : page + 1);
                    else page = (page == ((displayConsumList.Count / 6) + 1 - (displayConsumList.Count != 0 && displayConsumList.Count % 6 == 0 ? 1 : 0)) ? page : page + 1);
                }
                else
                {
                    if (ListItemType != 106)
                    {
                        if (!isSaleScreen) displayItemList[((page - 1) * 5) + (input - 1)].Equip(gm);
                        else displayItemList[((page - 1) * 5) + (input - 1)].SaleEquipItem(gm);
                    }
                    else
                    {
                        if (!isSaleScreen) { displayConsumList[input - 1].Use(gm.player); }
                        else displayConsumList[input - 1].SaleConsumItem(gm.player);
                    }
                }

            }


        }

        public void ConsumableItemInventoryScreen(GameManager gm,bool isDungeon = false)
        {
            while (true)
            {
                List<ConsumableItem> consumableItemInventoryList = new List<ConsumableItem>();
                for (int i = 0; i< gm.consumableItemsList.Count; i++)
                {
                    if (gm.consumableItemsList[i].ItemCount !=0) consumableItemInventoryList.Add(gm.consumableItemsList[i]);
                }
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("소비 아이템을 사용할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[소비 아이템 목록]");

                for (int i = 0; i < consumableItemInventoryList.Count; i++)
                {
                    Console.Write($"{i+1}.");
                    consumableItemInventoryList[i].DisplayItem();
                }

                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();

                int input = Utility.GetInput(0, consumableItemInventoryList.Count);
                if (input == 0)
                {
                    if (isDungeon) gm.dungeon.ReadyBattle(); 
                    break; 
                }
                else
                {
                    consumableItemInventoryList[input - 1].Use(gm.player);
                    if (isDungeon) { gm.dungeon.ItemLimits--; gm.dungeon.ReadyBattle(); return; }
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
                Utility.ColorText(ConsoleColor.White, $"{gm.player.gold} G");
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
                        case 2: EquipScreen(gm,true); break;
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
                    Utility.ColorText(ConsoleColor.Yellow, $"{gm.player.gold} G");
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
                                gm.equipItemList[i].DisplayEquipItemList(gm, false, true, false);
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
                            Console.WriteLine();
                        }
                    }



                    Console.WriteLine();
                    Console.WriteLine("목록바꾸기");
                    Console.WriteLine("101. 무기    102.보조무기  103.머리  104.몸   105.신발  106.소모품");
                    Console.WriteLine("0. 뒤로가기");
                    Console.WriteLine();
                    int input = Utility.GetInputPlus(0, index, new int[] { 101, 102, 103, 104, 105, 106 });
                    if (input == 0) break;
                    else if (input>100) ListItemType = input;
                    else { if (ListItemType != 106) BuyItem(input); else BuyConsumItem(input - 1); }
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
