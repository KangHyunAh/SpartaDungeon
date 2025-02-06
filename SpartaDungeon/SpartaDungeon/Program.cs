namespace SpartaDungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager();
            
            gm.InventoryScreen();
            
        }
    }

    class GameManager
    {
        List<EquipItem> equipItemList;
        public GameManager()        //  tring name, EquipType type, int atk, int def, int maxHp, string discription, int cost
        {
            equipItemList = new List<EquipItem>
            {
            new EquipItem("수련자의 갑옷", EquipType.Amor, 0,2,50, "수련에 도움을 주는 갑옷입니다. ", 1000),
            new EquipItem("무쇠갑옷", EquipType.Amor, 0,4,100, "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000),
            new EquipItem("스파르타의 갑옷", EquipType.Amor, 0,8,200, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500),
            new EquipItem("낡은 검", EquipType.Weapon, 5,0,0, "쉽게 볼 수 있는 낡은 검 입니다. ", 600),
            new EquipItem("청동 도끼", EquipType.Weapon, 10,0,0, "어디선가 사용됐던거 같은 도끼입니다. ", 1500),
            new EquipItem("스파르타의 창", EquipType.Weapon, 20,0,0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500),
            //아이템 추가 부분
            new EquipItem("추가 아이템 1", EquipType.Weapon, 0,2,0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500),
            new EquipItem("추가 아이템 2", EquipType.Weapon, 0,4,0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500)
            };
        }
        public void InventoryScreen()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[장비 아이템 목록]");

            for (int i = 0; i <equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip) equipItemList[i].DisplayEquipItem();
            }
            Console.WriteLine() ;
            for (int i = 0;i <equipItemList.Count; i++)
            {
                if(equipItemList[i].ItemCount>0) equipItemList[i].DisplayinventoryItem();
            }

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
        public void EquipScreen()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 -  장착관리");
            Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[장비 아이템 목록]");

            int index=0;
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip)
                {
                    index++; Console.Write($"{index}.");
                    equipItemList[i].DisplayEquipItem(); 
                }
            }
            Console.WriteLine();
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].ItemCount > 0)
                {
                    index++; Console.Write($"{index}.");
                    equipItemList[i].DisplayinventoryItem();
                }
            }

            Console.WriteLine();
            Console.WriteLine("0.나가기");
            Console.WriteLine();

            int input = Utility.GetInput(0, index);
            switch (input)
            {
                case 0: /*MainScreen*/break;
                default: Equip(input); break;
            }
        }
        public void Equip(int input)
        {
            int index = 0;
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].isEquip)
                {
                    index++;
                    if (index == input)
                    {
                        equipItemList[i].isEquip = false;
                        equipItemList[i].ItemCount++;
                    }
                }
            }
            for (int i = 0; i < equipItemList.Count; i++)
            {
                if (equipItemList[i].ItemCount > 0)
                {
                    index++; 
                    if (index == input)
                    {
                        if(equipItemList.Any(EquipItem=>EquipItem.Type  == equipItemList[i].Type&&EquipItem.isEquip)) //선택 아이템의 장착타입과 같은 장착타입을 가지고 장착중인 장비가 존재한다면
                        {                                                                                               //먼저 장착된 아이템 해제 = 소지수++ 및 장착여부 false
                            equipItemList[equipItemList.FindIndex(EquipItem => EquipItem.Type == equipItemList[i].Type && EquipItem.isEquip)].ItemCount++;
                            equipItemList[equipItemList.FindIndex(EquipItem => EquipItem.Type == equipItemList[i].Type && EquipItem.isEquip)].isEquip = false;
                        }
                        equipItemList[i].ItemCount--;
                        equipItemList[i].isEquip = true;
                    }
                }
            }
            EquipScreen();
        }

    }
}

