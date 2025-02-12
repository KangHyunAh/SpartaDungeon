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
            gm.startScene.Lobby(gm);
        }
    }

    class GameManager
    {
        public StartScene startScene = new StartScene();
        public InventoryAndShop inventoryAndShop = new InventoryAndShop();
        public Player player = new Player();
        public Dungeon dungeon;
        public QuestManager questManager = new QuestManager();



        public List<Monster> monsters = new List<Monster>();
        public List<Monster> monsterList;
        public List<Monster> bossmonsterList;

        public List<EquipItem> equipItemList;
        public List<ConsumableItem> consumableItemsList;

        

        public GameManager()        //게임매니저 생성자
        {
            questManager = new QuestManager();
            player = new Player();
            dungeon = new Dungeon(questManager, player);

            //장비아이템 리스트
            equipItemList = new List<EquipItem>     //  string 이름, EquipType 장착부위, int 공격력, int 방어력, int 쵀대채력, string 설명,string[] 착용가능 직업목록, int 가격,bool 보스아이템 여부
            {
            //무기 목록 EquipType.Weapon
                new EquipItem("낡은 검", EquipType.무기, 5,0,0, "쉽게 볼 수 있는 낡은 검 입니다. ",new string []{"나이트","검사","광전사" }, 500,false),
                new EquipItem("청동 도끼", EquipType.무기, 10,0,0, "어디선가 사용됐던거 같은 도끼입니다. ",new string []{"나이트","검사","광전사" }, 1500,false),
                new EquipItem("스파르타의 창", EquipType.무기, 20,0,0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("기사의 장창", EquipType.무기, 10,10,0, "기사들이 사용하는 크고묵직한 창입니다. ",new string []{"나이트"}, 2500,false),
                new EquipItem("검사의 장검", EquipType.무기, 15,5,0, "길어서 다루는데 기술이 필요한 장검입니다. ",new string []{"검사"}, 2500,false),
                new EquipItem("광전사의 도끼", EquipType.무기, 20,0,0, "오로지 높은 공격력을 위한 도끼입니다. ",new string []{"광전사"}, 2500,false),
                new EquipItem("기사단장 창", EquipType.무기, 20,20,0, "행방불명된 전 기사단장이 쓰던 창입니다. ",new string []{"나이트"}, 6000,true),
                new EquipItem("검성의 검", EquipType.무기, 30,10,0, "풍문으로 떠돌던 검성의 검입니다. ",new string []{"검사"}, 6000,true),
                new EquipItem("광화보스 도끼", EquipType.무기, 40,0,0, "들고만 있어도 정신이나갈거같은 도끼입니다. ",new string []{"광전사"}, 6000,true),
            //보조 목록 EquipType.SubWeapon
                new EquipItem("나무방패", EquipType.보조, 0,2,0, "나무로 만들어진 기본적인 방패입니다. ",new string []{"나이트","검사","광전사" }, 500,false),
                new EquipItem("강철방패", EquipType.보조, 0,5,0, "강철로 만들어진 튼튼한 방패입니다. ",new string []{"나이트","검사","광전사" }, 1500,false),
                new EquipItem("타워 실드", EquipType.보조, 0,20,0, "몸전체를 가릴수있는 크고무거운 방패입니다. ",new string []{"나이트" }, 2500,false),
                new EquipItem("베테랑 견갑", EquipType.보조, 5,10,0, "잘다루기만 하면 유용한 어깨 방어구입니다. ",new string []{"검사" }, 2500,false),
                new EquipItem("쌍수도끼", EquipType.보조, 15,0,0, "양손에 들고 찍습니다. 아픕니다. ",new string []{"광전사" }, 2500,false),
                new EquipItem("티타늄문짝", EquipType.보조, 0,30,0, "고대문명 유적에서 떼온 문짝입니다.크고 튼튼합니다. ",new string []{"나이트" }, 6000,true),
                new EquipItem("검성의 검집", EquipType.보조, 10,20,0, "검성의 검과 세트입니다.방어,공격 둘다가능합니다. ",new string []{"검사" }, 6000,true),
                new EquipItem("미친쌍도끼", EquipType.보조, 30,0,0, "양손에 들고 쎄게 찍습니다. 많이아픕니다. ",new string []{"광전사" }, 6000,true),
            //머리 목록 EquipType.Head
                new EquipItem("수련자의 헬멧", EquipType.머리, 0,1,0, "수련에 도움을 주는 헬멧입니다. ",new string []{"나이트","검사","광전사" }, 500,false),
                new EquipItem("무쇠 투구", EquipType.머리, 0,3,0, "강철로 만들어져 튼튼한 투구입니다. ",new string []{"나이트","검사","광전사" }, 1500,false),
                new EquipItem("스파르타의 투구", EquipType.머리, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 투구입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("스파르타의 투구", EquipType.머리, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 투구입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("스파르타의 투구", EquipType.머리, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 투구입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("스파르타의 투구", EquipType.머리, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 투구입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("보스1의 투구", EquipType.머리, 0,10,0, "보스1이 사용하던 투구입니다. ",new string []{"나이트","검사","광전사" }, 3000,true),
            //몸 목록 EquipType.Amor
                new EquipItem("수련자의 갑옷", EquipType.몸, 0,1,50, "수련에 도움을 주는 갑옷입니다. ",new string []{"나이트","검사","광전사" }, 800,false),
                new EquipItem("무쇠갑옷", EquipType.몸, 0,3,100, "무쇠로 만들어져 튼튼한 갑옷입니다. ",new string []{"나이트","검사","광전사" }, 2000,false),
                new EquipItem("스파르타의 갑옷", EquipType.몸, 0,5,200, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",new string []{"나이트","검사","광전사" }, 3500,false),
                new EquipItem("보스1의 갑옷", EquipType.몸, 0,10,300, "보스1이 사용하던 갑옷입니다. ",new string []{"나이트","검사","광전사" }, 4000,true),
            //신발 목록 EquipType.Boots
                new EquipItem("수련자의 부츠", EquipType.신발, 0,1,0, "수련에 도움을 주는 부츠입니다. ",new string []{"나이트","검사","광전사" }, 500,false),
                new EquipItem("무쇠 각반", EquipType.신발, 0,3,0, "무쇠로 만들어진 갑옷하의 입니다. ",new string []{"나이트","검사","광전사" }, 1500,false),
                new EquipItem("스파르타 각반", EquipType.신발, 0,5,0, "스파르타의 전사들이 사용했다는 전설의 각반입니다. ",new string []{"나이트","검사","광전사" }, 2500,false),
                new EquipItem("보스1의 각반", EquipType.신발, 0,10,0, "보스1이 사용하던 각반입니다. ",new string []{"나이트","검사","광전사" }, 3000,true),

            };

            consumableItemsList = new List<ConsumableItem>  //string 이름, PotionType 포션타입, int 효과량,  string 아이템설명, int 가격
            {
                new ConsumableItem("하급 HP회복 포션",PotionType.Health,50,"소량의 HP를 회복시켜주는 약이다",200),
                new ConsumableItem("중급 HP회복 포션",PotionType.Health,100,"적당한 HP를 회복시켜주는 약이다",500),
                new ConsumableItem("상급 HP회복 포션",PotionType.Health,200,"많은양의 HP를 회복시켜주는 약이다",1000),
                new ConsumableItem("하급 MP회복 포션",PotionType.Mana,20,"소량의 MP를 회복시켜주는 약이다",200),
                new ConsumableItem("중급 MP회복 포션",PotionType.Mana,50,"적당한 MP를 회복시켜주는 약이다",500),
                new ConsumableItem("상급 MP회복 포션",PotionType.Mana,100,"많은양의 MP를 회복시켜는 약이다",1000)

            };



            monsterList = new List<Monster>
            {
                new Monster("나이트의 흔적",1,10,100,5,"나이트"),
                new Monster("검사의 흔적",1,10,100,5,"검사"),
                new Monster("광전사의 흔적",1,10,100,5,"광전사"),
                new Monster("미믹",1,1,10000,10,"이벤트")

            };
            bossmonsterList = new List<Monster>
            {
                new Monster("[Boss]영웅의 기억",50,30,10,10,"보스"),
            };

           
            
            questManager.AddQuest(new Quest(1, "흔적 퇴치", "5마리의 흔적을 잡으세요", 5, 50, 80));
            questManager.AddQuest(new Quest(2, "보스 퇴치", "영웅의 기억을 잡으세요", 1, 200, 300 ));


        }


        public void MainScreen()
        {


        }

    }
}

