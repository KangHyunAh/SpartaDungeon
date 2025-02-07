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
        private const string folderPath = "./Save";            // 세이브파일이 존재할 폴더의 위치
        private const string filePath = "./Save/Data.json";     // 플레이어 정보 세이브파일의 위치
        private const string itemfilePath = "./Save/ItemData.json";     // 아이템 정보 세이브파일의 위치

        // 캐릭터 생성
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

                // 설정한 닉네임이 유효하다면 진행
                // 유효하지 않다면 다시 입력할 수 있도록 반복문 처음으로 이동
                if (name != string.Empty && name != null)
                {
                    Console.WriteLine($"입력하신 이름은 {name} 입니다. 이대로 진행하시겠습니까?");
                    Console.WriteLine($"");
                    Console.WriteLine($"1. 저장");
                    Console.WriteLine($"2. 다시 설정");

                    int selectNum = Utility.GetInput(1, 2);

                    switch (selectNum)
                    {
                        // 저장 선택 시 저장 후 Player 클래스 리턴
                        case 1:
                            user.name = name;
                            return user;
                        // 다시 설정을 선택 할 시 반복문 처음으로 이동
                        case 2:
                            continue;
                    }
                }
            }
        }

        // 저장
        public void SaveData(Player user, List<EquipItem> items)
        {
            // 폴더 주소 설정
            DirectoryInfo folder = new DirectoryInfo(folderPath);

            // 폴더가 존재하지 않는다면 생성
            if (!folder.Exists)
                folder.Create();

            // 플레이어 정보 저장
            try
            {
                // 데이터 직렬화 후 스트링으로 반환
                string playerDataString = JsonConvert.SerializeObject(user);
                // 파일에 스트링 저장
                File.WriteAllText(filePath, playerDataString);
            }
            // 오류 발생 시
            catch { Console.WriteLine("플레이어 데이터를 저장하는 도중 오류가 발생했습니다."); return; }

            // 키값 : 아이템 이름, 밸류 값 : 아이템 갯수, 장착 여부(정수로 변환)
            Dictionary<string, int[]> itemDict = new Dictionary<string, int[]>();

            // 아이템 이름과 보유갯수, 장착여부
            foreach (var item in items)
            {
                itemDict.Add(item.Name, [item.ItemCount, Convert.ToInt32(item.isEquip)]);
            }

            // 아이템 정보 저장
            try
            {
                string ItemDataString = JsonConvert.SerializeObject(itemDict);
                File.WriteAllText(itemfilePath, ItemDataString);
            }
            catch { Console.WriteLine("아이템 데이터를 저장하는 도중 오류가 발생했습니다."); return; }
        }

        // 불러오기
        public Player LoadData(List<EquipItem> items)
        {
            Player loadCharacterData = new Player();

            // 파일 위치에 파일이 존재한다면 if문 실행
            // 존재 하지 않는다면 캐릭터 생성으로 이동
            if (File.Exists(filePath))
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
                        DataParsing(loadCharacterData,items);
                        return loadCharacterData;
                    case 2:
                        loadCharacterData = CreateCharacter();
                        break;
                }
            }

            else
            {
                loadCharacterData = CreateCharacter();
            }

            return loadCharacterData;
        }

        // 데이터 불러오기, 불러온 데이터 파싱
        public void DataParsing(Player user,List<EquipItem> items)
        {
            string data = string.Empty;

            try
            {
                // json파일 => 스트링으로 변환
                data = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"세이브 데이터를 불러오는 중 오류가 발생했습니다. {e}");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }

            // 직렬화 된 문자열 직렬화 해제
            JObject playerData = JObject.Parse(data);

            // 데이터 적용
            try
            {
                user.name = playerData["name"].ToString();
                user.chad = playerData["chad"].ToString();
                user.level = int.Parse(playerData["level"].ToString());
                user.strikePower = int.Parse(playerData["strikePower"].ToString());
                user.defensivePower = int.Parse(playerData["defensivePower"].ToString());
                user.maxhealthPoint = int.Parse(playerData["maxhealthPoint"].ToString());
                user.healthPoint = int.Parse(playerData["healthPoint"].ToString());
                user.gold = int.Parse(playerData["gold"].ToString());
            }
            // 오류 발생 시 캐릭터 생성으로 이동
            catch
            {
                Console.WriteLine($"세이브 데이터를 불러오는 중 오류가 발생했습니다.");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }

            try
            {
                data = File.ReadAllText(itemfilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"세이브 데이터를 불러오는 중 오류가 발생했습니다. {e}");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }

            JObject itemData = JObject.Parse(data);

            // 불러온 데이터 딕셔너리로 변환
            Dictionary<string, int[]> itemDict = itemData.ToObject<Dictionary<string, int[]>>();

            // 참조할 아이템을 담을 인스턴스
            EquipItem tempItem = new EquipItem("", EquipType.Weapon, 1, 1, 1, "", 1);

            foreach (KeyValuePair<string, int[]> item in itemDict)
            {
                // equipItemList 안에서 item의 키값(아이템 이름)과 일치하는 아이템을 찾아 참조 및 값 변경
                // 일치하는 아이템이 없다면 null 반환
                tempItem = items.Where(i => i.Name == item.Key).OfType<EquipItem>().FirstOrDefault();

                // 일치하는 아이템 없을 시 다음 루프로
                if (tempItem == null)
                    continue;

                tempItem.ItemCount = item.Value[0];
                tempItem.isEquip = Convert.ToBoolean(item.Value[1]);

                // 장착 중이라면 해당 아이템의 능력치 적용
                if (tempItem.isEquip)
                {
                    user.equipStrikePower += tempItem.Atk;
                    user.equipDefensivePower += tempItem.Def;
                    user.equipMaxhealthPoint += tempItem.MaxHp;
                }
            }


        }
    }
}
