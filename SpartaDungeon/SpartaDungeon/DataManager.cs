using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SpartaDungeon
{
    internal class DataManager
    {
        private const string folderPath = "./Save";
        private const string filePath = "./Save/Data.json";

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

        public void SaveData(Player user)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);

            // 폴더 없다면 생성
            if (!folder.Exists)
                folder.Create();
            try
            {
                // 데이터 직렬화 후 스트링으로 반환
                string playerDataString = JsonConvert.SerializeObject(user);
                // 파일에 스트링 저장
                File.WriteAllText(filePath, playerDataString);
            }
            // 오류 발생 시 로비로 이동
            catch { Console.WriteLine("플레이어 데이터를 저장하는 도중 오류가 발생했습니다. 로비로 돌아갑니다.");}

        }

        public Player LoadData()
        {
            Player loadCharacterData = new Player();

            if (File.Exists("./Save/Data.json"))
            {
                Console.Clear();
                Console.WriteLine("저장 데이터가 존재합니다. 불러오시겠습니까?");
                Console.WriteLine();
                Console.WriteLine("1. 불러오기");
                Console.WriteLine("2. 처음부터 시작");
                Console.Write(">> ");

                int selectNumber = Utility.GetInput(1, 2);

                switch (selectNumber)
                {
                    case 1:
                        DataParsing(ref loadCharacterData);
                        return loadCharacterData;
                    case 2:
                        CreateCharacter();
                        break;
                }

            }
            else CreateCharacter();
            return loadCharacterData;
        }

        public void DataParsing(ref Player user)
        {
            string data = string.Empty;

            try
            {
                // 데이터 => 스트링으로 변환
                data = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"세이브 데이터를 불러오는 중 오류가 발생했습니다. {e}");
            }

            // 스트링 => JObject로 변환
            JObject playerData = JObject.Parse(data);

            // 데이터 적용
            user.name = playerData["name"].ToString();
            user.chad = playerData["chad"].ToString();
            user.level = int.Parse(playerData["level"].ToString());
            user.strikePower = int.Parse(playerData["strikePower"].ToString());
            user.defensivePower = int.Parse(playerData["defensivePower"].ToString());
            user.healthPoint = int.Parse(playerData["healthPoint"].ToString());
            user.gold = int.Parse(playerData["gold"].ToString());
        }
    }
}
