using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class StartScene
    {
        public void Lobby(GameManager gm)
        {
            DataManager dataManager = new DataManager();
            Dungeon dungeonManager = new Dungeon();

            while (true)
            {
                dataManager.SaveData(gm);

                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 캐릭터 정보");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine($"4. 던전 입장({dungeonManager.DungeonLv} 층)");
                Console.WriteLine("5. 퀘스트");
                Console.WriteLine("6. 휴식하기");
                Console.WriteLine();

                int selectNumber = Utility.GetInput(1, 6);

                switch (selectNumber)
                {
                    case 1:
                        gm.player.CharacterInformation();
                        break;
                    case 2:
                        gm.inventoryAndShop.InventoryScreen(gm.player, gm.equipItemList);
                        break;
                    case 3:
                        gm.inventoryAndShop.ShopScreen(gm.player, gm.equipItemList);
                        break;
                    case 4:
                        dungeonManager.Battle(gm.player, gm.monsters, gm.monsterList, gm.bossmonsterList);
                        break;
                    case 5:
                        ShowQuestMenu(gm);
                        break;
                    case 6:
                        gm.player.Rest();
                        break;
                }
            }
        }

        private void ShowQuestMenu(GameManager gm)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("====[퀘스트 목록]====");
                gm.QuestManager.ShowAllQuests();
                Console.WriteLine();
                Console.WriteLine("퀘스트를 수락하려면 ID를 입력하세요.(0 입력시 돌아가기)");

                int questID = Utility.GetInput(0, 99);
                if (questID == 0)
                    return;

                if (gm.QuestManager.AcceptQuest(questID))
                {
                    Console.WriteLine("퀘스트가 수락되었습니다!");
                }

                else
                {
                    Console.WriteLine("해당 ID의 퀘스트를 찾을 수 없습니다.");
                }

                Console.WriteLine("\n아무키나 누르면 돌아갑니다");
                Console.ReadKey();
            }
        } 
    }
}
