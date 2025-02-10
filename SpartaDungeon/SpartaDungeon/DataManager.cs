using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SpartaDungeon.PotionNamespace;

namespace SpartaDungeon
{
    internal class DataManager
    {
        private const string folderPath = "./Save";            // 세이브파일이 존재할 폴더의 위치
        //private const string filePath = "./Save/SaveData.json";     // 세이브파일의 위치
        private const string filePath = "./Save/testSaveData.json";     // 세이브파일의 위치


        // 캐릭터 생성
        public Player CreateCharacter()
        {
            Player player = new Player();

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
                        // 저장 선택 시 직업 선택으로 이동
                        case 1:
                            break;
                        // 다시 설정을 선택 할 시 반복문 처음으로 이동
                        case 2:
                            continue;
                    }
                }
                Console.Clear();

                // 직업 선택 및 name변수에 저장한 이름 저장
                player.ChadSelect();
                player.name = name;

                if (player.chad == "전사")
                    continue;

                return player;
            }
        }

        // 저장
        public void SaveData(GameManager gm)
        {
            // 폴더 주소 설정
            DirectoryInfo folder = new DirectoryInfo(folderPath);

            // 폴더가 존재하지 않는다면 생성
            if (!folder.Exists)
                folder.Create();

            string playerDataString = string.Empty;

            // 플레이어 정보 저장
            try
            {
                // 데이터 직렬화 후 스트링으로 반환
                playerDataString = JsonConvert.SerializeObject(gm.player);
            }
            // 오류 발생 시
            catch { Utility.ColorText(ConsoleColor.Red, "플레이어 데이터를 저장하는 중 오류가 발생했습니다."); Thread.Sleep(1000); return; }

            // 키값 : 아이템 이름, 밸류 값 : 아이템 갯수, 장착 여부(정수로 변환)
            Dictionary<string, int[]> itemDict = new Dictionary<string, int[]>();

            // 장비 아이템 이름과 보유갯수, 장착여부
            foreach (var item in gm.equipItemList)
            {
                itemDict.Add(item.Name, [item.ItemCount, Convert.ToInt32(item.isEquip)]);
            }

            // 소비 아이템 이름과 보유갯수
            foreach (var item in gm.consumableItemsList)
            {
                itemDict.Add(item.Name, [item.ItemCount]);
            }

            string itemDataString = string.Empty;

            // 아이템 정보 저장
            try
            {
                itemDataString = JsonConvert.SerializeObject(itemDict);
            }
            catch { Utility.ColorText(ConsoleColor.Red, "아이템 데이터를 저장하는 중 오류가 발생했습니다."); Thread.Sleep(1000); return; }


            string questDataString = string.Empty;
            string acceptQuestString = string.Empty;
            string completeQuestString = string.Empty;

            try
            {
                questDataString = JsonConvert.SerializeObject(gm.QuestManager.quests);
                acceptQuestString = JsonConvert.SerializeObject(gm.QuestManager.acceptedQuests);
                completeQuestString = JsonConvert.SerializeObject(gm.QuestManager.completedQuests);
            }
            catch { Utility.ColorText(ConsoleColor.Red, "퀘스트 데이터를 저장하는 중 오류가 발생했습니다."); Thread.Sleep(1000); return; }


            // 직렬화 한 데이터들을 담을 배열
            JArray jsonArr = new JArray();

            // 문자열 JObject로 변환
            //   배열  JArray로 변환
            JObject playerJson = JObject.Parse(playerDataString);
            JObject itemJson = JObject.Parse(itemDataString);
            JObject questListJson = JObject.Parse(questDataString);

            JArray acceptQuestJson = new JArray();
            JArray completeQuestJson = new JArray();

            // 수락한 퀘스트가 있다면
            if (gm.QuestManager.acceptedQuests.Count > 0)
                acceptQuestJson = JArray.Parse(acceptQuestString);

            // 완료한 퀘스트가 있다면
            if (gm.QuestManager.completedQuests.Count > 0)
                completeQuestJson = JArray.Parse(completeQuestString);

            // 배열에 추가
            jsonArr.Add(playerJson);
            jsonArr.Add(itemJson);
            jsonArr.Add(questListJson);

            // 수락한 퀘스트가 있다면
            if(acceptQuestJson != null)
                jsonArr.Add(acceptQuestJson);

            // 완료한 퀘스트가 있다면
            if(completeQuestJson != null)
                jsonArr.Add(completeQuestJson);

            // 암호화(인코딩)
            byte[] bytes = Encoding.UTF8.GetBytes(jsonArr.ToString());
            string encodingJson = Convert.ToBase64String(bytes);

            // 문자열로 변환 후 파일 생성
            File.WriteAllText(filePath, encodingJson);
        }

        // 불러오기
        public Player LoadData(GameManager gm)
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
                Console.WriteLine();

                int selectNumber = Utility.GetInput(1, 2);

                switch (selectNumber)
                {
                    case 1:
                        DataParsing(loadCharacterData, gm);
                        break;
                    case 2:
                        loadCharacterData = CreateCharacter();
                        break;
                }
            }

            else
            {
                loadCharacterData = CreateCharacter();
            }

            SkillManager.SkillInit(loadCharacterData);

            return loadCharacterData;
        }

        // 데이터 불러오기, 불러온 데이터 파싱
        public void DataParsing(Player user, GameManager gm)
        {
            string data = string.Empty;

            try
            {
                // json파일 => 스트링으로 변환
                data = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Utility.ColorText(ConsoleColor.Red, $"세이브 데이터를 불러오는 중 오류가 발생했습니다. {e.Message}");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }

            // 암호화 된 데이터 디코딩
            byte[] bytes = Convert.FromBase64String(data);
            string decoding = Encoding.UTF8.GetString(bytes);

            // 문자열 JArray로 변환
            JArray jsonArr = JArray.Parse(decoding);

            // 배열에 있는 데이터 JObject/JArray로 변환하여 변수에 하나씩 넣어주기
            JObject playerData = (JObject)jsonArr[0];
            JObject itemData = (JObject)jsonArr[1];
            JObject questData = (JObject)jsonArr[2];
            JArray acceptQuestData = (JArray)jsonArr[3];
            JArray clearQuestData = (JArray)jsonArr[4];

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
                user.exp = int.Parse(playerData["exp"].ToString());
                user.maxExp = int.Parse(playerData["maxExp"].ToString());
                user.dungeonLevel = int.Parse(playerData["dungeonLevel"].ToString());
                user.manaPoint = int.Parse(playerData["manaPoint"].ToString());
                user.maxManaPoint = int.Parse(playerData["maxManaPoint"].ToString());
            }
            // 오류 발생 시 캐릭터 생성으로 이동
            catch
            {
                Utility.ColorText(ConsoleColor.Red, $"세이브 데이터를 불러오는 중 오류가 발생했습니다.");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }


            // 불러온 데이터 딕셔너리로 변환
            Dictionary<string, int[]> itemDict = itemData.ToObject<Dictionary<string, int[]>>();

            // 참조할 아이템을 담을 인스턴스
            EquipItem tempItem = new EquipItem("", EquipType.Weapon, 1, 1, 1, "", new string[] { }, 1, false);
            ConsumableItem tempConsumable = new ConsumableItem("", PotionType.Health, 1, "", 1);

            foreach (KeyValuePair<string, int[]> item in itemDict)
            {
                string name = item.Key;

                // 포션이라면 갯수 불러온 후 다음 루프로 이동
                if (name.Contains("HP") || name.Contains("MP"))
                {
                    tempConsumable = gm.consumableItemsList.Where(i => i.Name == item.Key).OfType<ConsumableItem>().FirstOrDefault();
                    tempConsumable.ItemCount = item.Value[0];
                    continue;
                }

                // equipItemList 안에서 item의 키값(아이템 이름)과 일치하는 아이템을 찾아 참조 및 값 변경
                // 일치하는 아이템이 없다면 null 반환
                tempItem = gm.equipItemList.Where(i => i.Name == item.Key).OfType<EquipItem>().FirstOrDefault();

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

            // 퀘스트 불러오기
            // JObject => Dictionary<int, Quest> 변환
            Dictionary<int, Quest> questDict = questData.ToObject<Dictionary<int, Quest>>();

            // where로 꺼내올 퀘스트 담을 인스턴스
            Quest tempQuest = new Quest(5555, " ", " ", 1, 1, 1);

            foreach (KeyValuePair<int, Quest> quest in questDict)
            {
                tempQuest = gm.QuestManager.quests.Where(i => i.Key == quest.Key).OfType<Quest>().FirstOrDefault();

                if (tempQuest == null)
                    continue;

                // JObject에 담겨있던 카운트와 완료여부 저장
                tempQuest.CurrentCount = quest.Value.CurrentCount;
                tempQuest.IsCompleted = quest.Value.IsCompleted;
            }

            // JArray => HashSet<int> 변환(수락한 퀘스트 목록)
            HashSet<int> acceptList = acceptQuestData.ToObject<HashSet<int>>();

            if (acceptList != null)
            {
                foreach (var i in acceptList)
                {
                    gm.QuestManager.acceptedQuests.Add(i);
                }
            }

            // JArray => HashSet<int> 변환(클리어한 퀘스트 목록)
            HashSet<int> clearList = clearQuestData.ToObject<HashSet<int>>();
            
            if(clearList != null)
            {
                foreach (var i in clearList)
                {
                    gm.QuestManager.completedQuests.Add(i);
                }
            }

        }
    }
}
