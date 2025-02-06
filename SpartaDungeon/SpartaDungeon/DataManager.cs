using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class DataManager
    {
        public Player CreateCharacter()
        {

            Player user = new Player();
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine($"사용하실 이름을 입력해주세요.");
                Console.WriteLine($"");
                Console.Write($">>");
                string name = Console.ReadLine(); // 닉네임 설정
                Console.WriteLine($"");

                if (name != string.Empty && name != null)
                {
                    Console.WriteLine($"입력하신 이름은 {name} 입니다. 이대로 진행하시겠습니까?");
                    Console.WriteLine($"");
                    Console.WriteLine($"1. 저장");
                    Console.WriteLine($"2. 다시 설정");

                    int selectNum = Utility.GetInput(1, 2);

                    switch (selectNum)
                    {
                        case 1:
                            user.name = name;
                            return user;
                        case 2:
                            continue;
                    }
                }
            }
        }

        public void SaveData()
        {

        }

        public void LoadData()
        {

        }
    }
}
