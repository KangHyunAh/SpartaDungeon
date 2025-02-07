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
        public void Lobby(Player user, GameManager gm)
        {
            DataManager dataManager = new DataManager();

            while (true)
            {
                dataManager.SaveData(user, gm.equipItemList);

                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 캐릭터 정보");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine($"4. 던전 입장(현재 층)"); // (현재 층) 안에 현재 층수 표시
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine();

                int selectNumber = Utility.GetInput(1, 5);

                switch (selectNumber)
                {
                    case 1:
                        user.CharacterInformation();
                        break;
                    case 2:
                        gm.InventoryScreen();
                        break;
                    case 3:
                        gm.ShopScreen(); // 미완성
                        break;
                    case 4:
                        // 던전 이동 함수
                        break;
                    case 5:
                        user.Rest();
                        break;
                }
            }
        }
    }
}
