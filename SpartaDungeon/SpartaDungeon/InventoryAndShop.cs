using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SpartaDungeon.PotionNamespace;

namespace SpartaDungeon
{
    internal class InventoryAndShop
    {
        public void InventoryScreen(GameManager gm)
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[장비 아이템 목록]");

            DisplayInventory(gm.equipItemList, false, false);

            Console.WriteLine();
            Console.WriteLine("1.장착관리");
            Console.WriteLine("2.소비품 아이템 보기");
            Console.WriteLine("0.나가기");
            Console.WriteLine();
            switch (Utility.GetInput(0, 2))
            {
                case 0: /*MainScreen()*/ break;
                case 1: EquipScreen(); break;
                case 2: /*소비품인벤토리Screen();*/break;
            }

            void EquipScreen()
            {
                Console.Clear();
                Console.WriteLine("인벤토리 -  장착관리");
                Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[장비 아이템 목록]");

                int index;
                index = DisplayInventory(gm.equipItemList, true, false);     //인벤토리 목록 표시하기 (true 앞숫자존재)

                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();

                int input = Utility.GetInput(0, index);
                switch (input)
                {
                    case 0: InventoryScreen(gm); break;
                    default: Equip(input); break;
                }
                void Equip(int input)
                {
                    int index = 0;
                    for (int i = 0; i < gm.equipItemList.Count; i++)
                    {
                        if (gm.equipItemList[i].isEquip)       //장착중인 아이템을 선택했을경우. 장착해제 소지수++,장착여부(isEquip)false
                        {
                            index++;
                            if (index == input)
                            {
                                gm.equipItemList[i].ItemCount++;
                                gm.equipItemList[i].isEquip = false;
                            }
                        }
                    }
                    for (int i = 0; i < gm.equipItemList.Count; i++)   //장착중이 아닌 아이템을 선택했을경우. 장착
                    {
                        if (gm.equipItemList[i].ItemCount > 0)
                        {
                            index++;
                            if (index == input)
                            {                                                                                                       //장비타입(부위)별 중복장착 방지
                                if (gm.equipItemList.Any(EquipItem => EquipItem.Type == gm.equipItemList[i].Type && EquipItem.isEquip)) //선택 아이템의 장착타입과 같은 장착타입을 가지고 장착중인 장비가 존재한다면
                                {                                                                                               //먼저 장착된 아이템 해제 = 소지수++ 및 장착여부 false
                                    gm.equipItemList[gm.equipItemList.FindIndex(EquipItem => EquipItem.Type == gm.equipItemList[i].Type && EquipItem.isEquip)].ItemCount++;
                                    gm.equipItemList[gm.equipItemList.FindIndex(EquipItem => EquipItem.Type == gm.equipItemList[i].Type && EquipItem.isEquip)].isEquip = false;
                                }
                                gm.equipItemList[i].ItemCount--;
                                gm.equipItemList[i].isEquip = true;
                            }
                        }
                    }
                    UpdateEquipStatus(gm);    //장비에따른 스텟 업데이트
                    EquipScreen();
                }
            }
        }


        public int DisplayInventory(List<EquipItem> equipItemList, bool hasNum, bool isSaleScreen)   //인벤토리 목록 표시하기 (true 앞숫자 O, false 앞숫자 X), 판매창일경우 True
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
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].ItemCount > 0)
                {
                    index++; Console.Write($"{index}.");
                    if (isSaleScreen) equipItemList[i].DisplayShopItem(isSaleScreen); else equipItemList[i].DisplayinventoryItem();
                }
            }
            return index;
        }


        public void ShopScreen(GameManager gm)
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
            switch (input)
            {
                case 0: /*MainScreen()*/ break;
                case 1: BuyScreen(); break;
                case 2: SaleScreen(); break;
            }
            void BuyScreen()
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gm.player.gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < gm.equipItemList.Count; i++)
                {
                    Console.Write($"{i + 1,-2}.");
                    gm.equipItemList[i].DisplayShopItem(false);
                }

                Console.WriteLine();
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine();


                int input = Utility.GetInput(0, gm.equipItemList.Count);
                switch (input)
                {
                    case 0: ShopScreen(gm); break;
                    default: BuyItem(input - 1); break;
                }

                void BuyItem(int input)
                {
                    if (gm.equipItemList[input].Cost > gm.player.gold)
                    {
                        Console.WriteLine("소지금이 부족합니다.");
                        Console.WriteLine($"필요 :{gm.equipItemList[input].Cost,-5}G  현재 소지금 :{gm.player.gold,6}G");
                    }
                    else
                    {
                        gm.player.gold -= gm.equipItemList[input].Cost;
                        gm.equipItemList[input].ItemCount++;
                        Console.WriteLine($"아이템을 구매하였습니다.  -{gm.equipItemList[input].Cost}G");
                        Console.WriteLine($"현재 소지금 :{gm.player.gold,-6}G");
                        Console.WriteLine();

                    }
                    Console.WriteLine("(아무키입력)");
                    Console.ReadLine();
                    BuyScreen();
                }

            }

            void SaleScreen()
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("원하는 아이템을 판매합니다.(판매가는 원래 가격의 절반이 됩니다.)");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gm.player.gold} G");
                Console.WriteLine();
                Console.WriteLine("[인벤토리 목록]");

                int index;
                index = DisplayInventory(gm.equipItemList, true, true);     //인벤토리 목록 표시하기 (true 앞숫자존재)

                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();

                int input = Utility.GetInput(0, index);
                switch (input)
                {
                    case 0: ShopScreen(gm); break;
                    default: SaleItem(input); break;
                }
            }
            void SaleItem(int input)
            {
                int index = 0;
                for (int i = 0; i < gm.equipItemList.Count; i++)
                {
                    if (gm.equipItemList[i].isEquip)       //장착중인 아이템을 선택했을경우. 장착해제, 판매금(가격/2)받기
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
                                case 2: SaleScreen(); break;
                            }
                        }
                    }
                }
                for (int i = 0; i < gm.equipItemList.Count; i++)   //장착중이 아닌 아이템을 선택했을경우. 판매
                {
                    if (gm.equipItemList[i].ItemCount > 0)
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
                SaleScreen();
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
