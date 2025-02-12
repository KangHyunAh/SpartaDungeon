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
            QuestManager questManager = new QuestManager();
            Player player = new Player();
            DataManager dataManager = new DataManager();
            Dungeon dungeonManager = new Dungeon(questManager, player);

            int padding = 0;
            string paddingStr = string.Empty;

            while (true)
            {
                if (gm.player.gold < 100 && gm.player.healthPoint <= 0)
                    ReviveEvent(gm);

                dataManager.SaveData(gm);

                padding = gm.player.dungeonLevel < 100 ? 2 : 3;
                paddingStr = new(' ', 37 - padding);

                Console.Clear();

                Console.WriteLine("┌──────────────────────────────────────────────────────┐");
                Console.WriteLine("│ 스파르타 마을에 오신 여러분 환영합니다.              │");
                Console.WriteLine("│ 이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다. │");
                Console.WriteLine("│┌────────────────────────────────────────────────────┐│");
                Console.WriteLine("││  ####   ####       #      ####     #####      #    ││");
                Console.WriteLine("││ #   #    #  #     ###      #  #    # # #     ###   ││");
                Console.WriteLine("││ #        #  #     # #      #  #      #       # #   ││");
                Console.WriteLine("││  ###     #  #    #  #      ###       #      #  #   ││");
                Console.WriteLine("││     #    ###     #####     #  #      #      #####  ││");
                Console.WriteLine("││ #   #    #       #   #     #  #      #      #   #  ││");
                Console.WriteLine("││ ####    ###     ##   ##   ###  #    ###    ##   ## ││");
                Console.WriteLine("│└────────────────────────────────────────────────────┘│");

                Console.WriteLine("│ 1. 캐릭터 정보                                       │");
                Console.WriteLine("│                                                      │");
                Console.WriteLine("│ 2. 인벤토리                                          │");
                Console.WriteLine("│                                                      │");
                Console.WriteLine("│ 3. 상점                                              │");
                Console.WriteLine("│                                                      │");
                Console.WriteLine($"│ 4. 던전 입장({gm.player.dungeonLevel.ToString($"D{padding}")}층){paddingStr}│");
                Console.WriteLine("│                                                      │");
                Console.WriteLine("│ 5. 퀘스트                                            │");
                Console.WriteLine("│                                                      │");
                Console.WriteLine("│ 6. 휴식하기                                          │");
                Console.WriteLine("│                                                      │");
                Console.WriteLine("│ 7. 게임종료                                          │");
                Console.WriteLine("└──────────────────────────────────────────────────────┘");

                int selectNumber = Utility.GetInput(1, 8);

                switch (selectNumber)
                {
                    case 1:
                        gm.player.CharacterInformation();
                        break;
                    case 2:
                        gm.inventoryAndShop.InventoryScreen(gm);
                        break;
                    case 3:
                        gm.inventoryAndShop.ShopScreen(gm);
                        break;
                    case 4:
                        if (gm.player.healthPoint > 0)
                            gm.dungeon.Battle(gm);
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("현재 체력이 0인 상태입니다. 회복 후 입장해주세요.");
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    case 5:
                        ShowQuestMenu(gm);
                        break;
                    case 6:
                        gm.player.Rest();
                        break;
                    case 7:
                        Environment.Exit(0);
                        return;
                    case 8:
                        if (gm.player.healthPoint > 0)
                            gm.dungeon.Battle(gm,true);
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("현재 체력이 0인 상태입니다. 회복 후 입장해주세요.");
                            Thread.Sleep(1500);
                            continue;
                        }
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
                gm.questManager.ShowAllQuests();
                Console.WriteLine();
                Console.WriteLine("1. 퀘스트 수락");
                Console.WriteLine("0. 나가기");

                int choice = Utility.GetInput(0, 99);
                if (choice == 0)
                    return;

                if (choice == 1)
                {
                    Console.WriteLine("수락할 퀘스트를 입력하세요:");
                    int questId = Utility.GetInput(0, 99);
                    if (questId == 0) continue;

                    if (gm.questManager.AcceptQuest(questId))
                    {
                        Console.WriteLine("퀘스트가 수락되었습니다!");
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }

                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

        
                
                Console.WriteLine("\n아무키나 누르면 돌아갑니다.");
                Console.ReadKey();
            }
        }

        private void ReviveEvent(GameManager gm)
        {
            const int time = 200;

            if (gm.player.healthPoint < 0)
                gm.player.healthPoint = 0;

            if (gm.player.manaPoint < 0)
                gm.player.manaPoint = 0;

            int enterHP = gm.player.healthPoint;
            int enterMP = gm.player.manaPoint;

            int maxHp = gm.player.maxhealthPoint + gm.player.equipMaxhealthPoint;
            int maxMp = gm.player.maxManaPoint;

            gm.player.healthPoint = gm.player.maxhealthPoint + gm.player.equipMaxhealthPoint;
            gm.player.manaPoint = gm.player.maxManaPoint;

            Console.Clear();

            Console.WriteLine($"전투에서 패배하였습니다.");

            Thread.Sleep(time);
            Console.WriteLine($"");
            Console.WriteLine($"만신창이가 된 몸으로 정신없이 도망쳤다…");

            Thread.Sleep(time);
            Console.WriteLine($"더 이상 걸을 수 없다.");

            Thread.Sleep(time);
            Console.WriteLine($"벽에 기대 앉았다.");

            Thread.Sleep(time);
            Console.WriteLine($"스르르 눈이 감긴다...");

            Thread.Sleep(time);
            Console.WriteLine($"");
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 저기요! 괜찮으십니까?");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 대체 이 안에서 무슨 일이 있었던 겁니까?");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 우선 치료해드리겠습니다.");

            Thread.Sleep(time);
            Console.WriteLine($"");
            Console.WriteLine($"HP : {enterHP} / {maxHp} => HP : {gm.player.healthPoint} / {maxHp}");
            Console.WriteLine($"MP : {enterMP} / {maxMp} => MP : {gm.player.manaPoint} / {maxMp}");
            Console.WriteLine($"");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 가장 가까운 마을까지 부축하겠습니다.");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 금방 도착할겁니다.");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 앞으로는 무리하지 마십시오.");

            Thread.Sleep(time);
            Utility.ColorText(ConsoleColor.Yellow, "지나가던 성직자 ", Text.Write); Console.WriteLine($": 신의 가호가 당신과 함께하길.");

            Thread.Sleep(time);
            Console.WriteLine($"");
            Console.WriteLine($"성직자는 어느새 저 멀리 사라졌다.");

            Thread.Sleep(time);
            Console.WriteLine($"");

            Thread.Sleep(time);
            Console.WriteLine($"");
            Console.WriteLine($"0. 마을로 이동하기");
            Console.WriteLine($"");


            if (Utility.GetInput(0, 0) == 0)
                return;
        }
    }
}
