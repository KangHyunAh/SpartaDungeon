using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using SpartaDungeon.PotionNamespace;

namespace SpartaDungeon
{
    internal class Program
    {
        static void Main(string[] args)     //메인게임진행
        {
            GameManager gm = new GameManager();
            
            StartScene start = new StartScene();
            gm.player = new DataManager().LoadData(gm);
            start.Lobby(gm);
        }
    }

    class GameManager
    {
        public StartScene startScene = new StartScene();
        public InventoryAndShop inventoryAndShop = new InventoryAndShop();
        public Player player = new Player();



        public Dungeon dungeon = new Dungeon();
        public List<Monster> monsters = new List<Monster>();
        public List<Monster> monsterList;
        public List<Monster> bossmonsterList;

        public List<EquipItem> equipItemList;
        public List<ConsumableItem> consumableItemsList;

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

            consumableItemsList = new List<ConsumableItem>
            {
                new ConsumableItem("하급 HP회복 포션",PotionType.Health,50,"소량의 HP를 회복시켜주는 약이다",200),
                new ConsumableItem("중급 HP회복 포션",PotionType.Health,100,"적당한 HP를 회복시켜주는 약이다",500),
                new ConsumableItem("상급 HP회복 포션",PotionType.Health,200,"많은양의 HP를 회복시켜주는 약이다",1000),
                //new ConsumableItem("하급 MP회복 포션",PotionType.Mana,20,"소량의 MP를 회복시켜주는 약이다",200),
                //new ConsumableItem("중급 MP회복 포션",PotionType.Mana,50,"적당한 MP를 회복시켜주는 약이다",500),
                //new ConsumableItem("상급 MP회복 포션",PotionType.Mana,100,"많은양의 MP를 회복시켜주는 약이다",1000)

            };
            


            monsterList = new List<Monster>
            {
                new Monster(1,"one",1,1,1),
                new Monster(2,"two",2,2,2),
                new Monster(3,"three",3,3,3),
            };
            monsterList = new List<Monster>
            {
                new Monster(10,"10",10,10,10),
                new Monster(11,"11",11,11,11),
                new Monster(11,"11",12,12,12),
            };

        }
        

        public void MainScreen()
        {


        }

    }
}

