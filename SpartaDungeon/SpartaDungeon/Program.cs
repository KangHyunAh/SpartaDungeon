using System.Data;
using System.Numerics;
using System.Security.Cryptography;

namespace SpartaDungeon
{
    internal class Program
    {
        static void Main(string[] args)     //메인게임진행
        {
            GameManager gm = new GameManager();
            
            StartScene start = new StartScene();
            gm.player = new DataManager().LoadData(gm.equipItemList);
            start.Lobby(gm.player, gm);
        }
    }

    class GameManager
    {
        public Player player = new Player();
        public List<EquipItem> equipItemList;
        public GameManager()        //게임매니저 생성자
        {
            equipItemList = new List<EquipItem>     //  string 이름, EquipType 장착부위, int 공격력, int 방어력, int 쵀대채력, string 설명, int 가격
            {
            new EquipItem("수련자의 갑옷", EquipType.Amor, 0,1,50, "수련에 도움을 주는 갑옷입니다. ", 800),
            new EquipItem("무쇠갑옷", EquipType.Amor, 0,3,100, "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000),
            new EquipItem("스파르타의 갑옷", EquipType.Amor, 0,5,200, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500),
            new EquipItem("낡은 검", EquipType.Weapon, 5,0,0, "쉽게 볼 수 있는 낡은 검 입니다. ", 500),
            new EquipItem("청동 도끼", EquipType.Weapon, 10,0,0, "어디선가 사용됐던거 같은 도끼입니다. ", 1500),
            new EquipItem("스파르타의 창", EquipType.Weapon, 20,0,0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500),
            new EquipItem("수련자의 헬멧", EquipType.Head, 0,1,0, "수련에 도움을 주는 헬멧입니다. ", 500),
            new EquipItem("무쇠 투구", EquipType.Head, 0,3,0, "강철로 만들어져 튼튼한 투구입니다. ", 1500),
            new EquipItem("스파르타의 투구", EquipType.Head, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 투구입니다. ", 2500),
            new EquipItem("수련자의 부츠", EquipType.Boots, 0,1,0, "수련에 도움을 주는 부츠입니다. ", 500),
            new EquipItem("무쇠 각반", EquipType.Boots, 0,3,0, "무쇠로 만들어진 갑옷하의 입니다. ", 1500),
            new EquipItem("스파르타 각반", EquipType.Boots, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 각반입니다. ", 2500),
            new EquipItem("나무방패", EquipType.SubWeapon, 0,2,0, "나무로 만들어진 기본적인 방패입니다. ", 500),
            new EquipItem("강철방패", EquipType.SubWeapon, 0,5,0, "강철로 만들어진 튼튼한 방패입니다. ", 1500),
            new EquipItem("타워 실드", EquipType.Weapon, 0,10,0, "몸전체를 가릴수있는 커다란 방패입니다. ", 2500),
            };
        }
        public void InventoryScreen()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[장비 아이템 목록]");

            DisplayInventory(false,false);

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
        }
        int DisplayInventory(bool hasNum,bool isSaleScreen)   //인벤토리 목록 표시하기 (true 앞숫자 O, false 앞숫자 X), 판매창일경우 True
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
                    if(isSaleScreen) equipItemList[i].DisplayShopItem(isSaleScreen); else equipItemList[i].DisplayinventoryItem();
                }
            }
            return index;
        }
        public void EquipScreen()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 -  장착관리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[장비 아이템 목록]");

            int index;
            index = DisplayInventory(true,false);     //인벤토리 목록 표시하기 (true 앞숫자존재)

            Console.WriteLine();
            Console.WriteLine("0.나가기");
            Console.WriteLine();

            int input = Utility.GetInput(0, index);
            switch (input)
            {
                case 0: InventoryScreen();  break;
                default: Equip(input); break;
            }
            void Equip(int input)
            {
            int index = 0;
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip)       //장착중인 아이템을 선택했을경우. 장착해제 소지수++,장착여부(isEquip)false
                {
                    index++;
                    if (index == input)
                    {
                        equipItemList[i].ItemCount++;
                        equipItemList[i].isEquip = false;
                    }
                }
            }
            for (int i = 0; i < equipItemList.Count; i++)   //장착중이 아닌 아이템을 선택했을경우. 장착
            {
                if (equipItemList[i].ItemCount > 0)
                {
                    index++;
                    if (index == input)
                    {                                                                                                       //장비타입(부위)별 중복장착 방지
                        if (equipItemList.Any(EquipItem => EquipItem.Type == equipItemList[i].Type && EquipItem.isEquip)) //선택 아이템의 장착타입과 같은 장착타입을 가지고 장착중인 장비가 존재한다면
                        {                                                                                               //먼저 장착된 아이템 해제 = 소지수++ 및 장착여부 false
                            equipItemList[equipItemList.FindIndex(EquipItem => EquipItem.Type == equipItemList[i].Type && EquipItem.isEquip)].ItemCount++;
                            equipItemList[equipItemList.FindIndex(EquipItem => EquipItem.Type == equipItemList[i].Type && EquipItem.isEquip)].isEquip = false;
                        }
                        equipItemList[i].ItemCount--;
                        equipItemList[i].isEquip = true;
                    }
                }
            }
            UpdateEquipStatus();    //장비에따른 스텟 업데이트
            EquipScreen();
            }


        }
        public void UpdateEquipStatus()     //장비에따른 스텟 업데이트
        {
            player.equipStrikePower = 0; player.equipDefensivePower = 0; player.equipMaxhealthPoint = 0;
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip)
                {
                    player.equipStrikePower += equipItemList[i].Atk;
                    player.equipDefensivePower += equipItemList[i].Def;
                    player.equipMaxhealthPoint += equipItemList[i].MaxHp;
                }
            }
        }
        public void ShopScreen()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G");
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
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < equipItemList.Count; i++)
                {
                    Console.Write($"{i + 1,-2}.");
                    equipItemList[i].DisplayShopItem(false);
                }

                Console.WriteLine();
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine();


                int input = Utility.GetInput(0, equipItemList.Count);
                switch (input)
                {
                    case 0: ShopScreen(); break;
                    default: BuyItem(input - 1); break;
                }
            }
            void BuyItem(int input)
            {
                if (equipItemList[input].Cost > player.gold)
                {
                    Console.WriteLine("소지금이 부족합니다.");
                    Console.WriteLine($"필요 :{equipItemList[input].Cost,-5}G  현재 소지금 :{player.gold,6}G");
                }
                else
                {
                    player.gold -= equipItemList[input].Cost;
                    equipItemList[input].ItemCount++;
                    Console.WriteLine($"아이템을 구매하였습니다.  -{equipItemList[input].Cost}G");
                    Console.WriteLine($"현재 소지금 :{player.gold,-6}G");
                    Console.WriteLine();

                }
                Console.WriteLine("(아무키입력)");
                Console.ReadLine();
                BuyScreen();
            }
            void SaleScreen()
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("원하는 아이템을 판매합니다.(판매가는 원래 가격의 절반이 됩니다.)");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.gold} G");
                Console.WriteLine();
                Console.WriteLine("[인벤토리 목록]");

                int index;
                index = DisplayInventory(true,true);     //인벤토리 목록 표시하기 (true 앞숫자존재)

                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();

                int input = Utility.GetInput(0, index);
                switch (input)
                {
                    case 0: ShopScreen(); break;
                    default: SaleItem(input); break;
                }
            }
            void SaleItem(int input)
            {
                int index = 0;
                for (int i = 0; i < equipItemList.Count; i++)
                {
                    if (equipItemList[i].isEquip)       //장착중인 아이템을 선택했을경우. 장착해제, 판매금(가격/2)받기
                    {
                        index++;
                        if (index == input)
                        {
                            Console.WriteLine($"{equipItemList[i].Name} 판매가 {equipItemList[i].Cost/2}G") ;
                            Console.WriteLine("장착중인 아이템입니다! 해제하고 판매하시겠습니까?\n");
                            Console.WriteLine("1.해제 후 판매      2.취소\n");
                            switch (Utility.GetInput(1, 2))
                            {
                                case 1: { equipItemList[i].isEquip = false; player.gold += equipItemList[i].Cost / 2; UpdateEquipStatus(); } break;
                                case 2: SaleScreen(); break;
                            }
                        }
                    }
                }
                for (int i = 0; i < equipItemList.Count; i++)   //장착중이 아닌 아이템을 선택했을경우. 판매
                {
                    if (equipItemList[i].ItemCount > 0)
                    {
                        index++;
                        if (index == input)
                        {
                            equipItemList[i].ItemCount--;
                            player.gold += equipItemList[i].Cost / 2;
                        }
                    }
                }
                Console.WriteLine("아이템을 판매하였습니다. (아무키입력)");
                Console.ReadLine();
                SaleScreen();
            }
        }



    }
}

