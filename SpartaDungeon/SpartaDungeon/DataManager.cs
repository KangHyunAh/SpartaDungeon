﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SpartaDungeon.PotionNamespace;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Security;
using Foundation;

namespace SpartaDungeon
{
    internal class DataManager
    {
        private readonly string folderPath = "./Save";            // 세이브파일이 존재할 폴더의 위치
        private readonly string filePath = "./Save/testSaveData.json";     // 세이브파일의 위치

        private readonly string decryptionFolderPath = "./Decrypt"; // 복호화 시 필요한 파일들이 존재할 폴더의 위치
        private readonly string keyPath = "./Decrypt/Key.bin";     // Key파일의 위치
        private readonly string ivPath = "./Decrypt/IV.bin";     // IV파일의 위치

        private int selectNum = 0;

        // 캐릭터 생성
        private Player CreateCharacter()
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

                    selectNum = Utility.GetInput(1, 2);

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

            DirectoryInfo decryptionFolder = new DirectoryInfo(decryptionFolderPath);

            // 폴더가 존재하지 않는다면 생성
            if (!folder.Exists)
                folder.Create();

            // 복호화 파일들 보관할 폴더 생성/숨김 처리
            if (!decryptionFolder.Exists)
            {
                decryptionFolder.Create();

                // |= : 파일 시스템에 속성을 추가 할 때 사용 |  &= : 파일 시스템에 속성을 제거 할 때 사용
                decryptionFolder.Attributes |= FileAttributes.Hidden;
            }

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
                questDataString = JsonConvert.SerializeObject(gm.questManager.quests);
                acceptQuestString = JsonConvert.SerializeObject(gm.questManager.acceptedQuests);
                completeQuestString = JsonConvert.SerializeObject(gm.questManager.completedQuests);
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
            if (gm.questManager.acceptedQuests.Count > 0)
                acceptQuestJson = JArray.Parse(acceptQuestString);

            // 완료한 퀘스트가 있다면
            if (gm.questManager.completedQuests.Count > 0)
                completeQuestJson = JArray.Parse(completeQuestString);

            // 배열에 추가
            jsonArr.Add(playerJson);
            jsonArr.Add(itemJson);
            jsonArr.Add(questListJson);

            // 수락한 퀘스트가 있다면
            if (acceptQuestJson != null)
                jsonArr.Add(acceptQuestJson);

            // 완료한 퀘스트가 있다면
            if (completeQuestJson != null)
                jsonArr.Add(completeQuestJson);

            // 데이터를 저장한 배열 암호화
            string encryptJson = Aes256Encrypt(jsonArr.ToString());

            // 암호화 한 데이터를 기반으로 파일 생성
            File.WriteAllText(filePath, encryptJson);
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

                selectNum = Utility.GetInput(1, 2);

                switch (selectNum)
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
        private void DataParsing(Player user, GameManager gm)
        {
            string data = string.Empty;
            string decryptData = string.Empty;

            JArray jsonArr = new JArray();

            JObject playerData = new JObject();
            JObject itemData = new JObject();
            JObject questData = new JObject();
            JArray acceptQuestData = new JArray();
            JArray clearQuestData = new JArray();

            try
            {
                // 암호화 된 json파일 => 스트링으로 변환
                data = File.ReadAllText(filePath);
                // 복호화
                decryptData = Aes256Decrypt(data);

                // 문자열 JArray로 변환
                jsonArr = JArray.Parse(decryptData);

                // 배열에 있는 데이터 JObject/JArray로 변환하여 변수에 하나씩 넣어주기
                playerData = (JObject)jsonArr[0];
                itemData = (JObject)jsonArr[1];
                questData = (JObject)jsonArr[2];
                acceptQuestData = (JArray)jsonArr[3];
                clearQuestData = (JArray)jsonArr[4];
            }
            catch (Exception e)
            {
                Utility.ColorText(ConsoleColor.Red, $"세이브 데이터를 불러오는 중 오류가 발생했습니다. {e.Message}");
                Console.WriteLine($"캐릭터 생성으로 이동합니다.");
                Thread.Sleep(1000);
                user = CreateCharacter();
            }

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
            EquipItem tempItem = new EquipItem("", EquipType.무기, 1, 1, 1, "", new string[] { }, 1, false);
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
            Quest tempQuest = new Quest(5555, " ", " ", " ",1, 1, 1);

            foreach (KeyValuePair<int, Quest> quest in questDict)
            {
                tempQuest = gm.questManager.quests.Where(i => i.Key == quest.Key).Select(i => i.Value as Quest).FirstOrDefault();

                if (tempQuest == null)
                    continue;

                // JObject에 담겨있던 카운트와 완료여부 저장
                tempQuest.CurrentProgress = quest.Value.CurrentProgress;
                tempQuest.IsCompleted = quest.Value.IsCompleted;
                tempQuest.IsAccepted = quest.Value.IsAccepted;
                tempQuest.Status = quest.Value.Status;
            }

            // JArray => HashSet<int> 변환(수락한 퀘스트 목록)
            HashSet<int> acceptList = acceptQuestData.ToObject<HashSet<int>>();

            if (acceptList != null)
            {
                foreach (var i in acceptList)
                {
                    gm.questManager.acceptedQuests.Add(i);
                }
            }

            // JArray => HashSet<int> 변환(클리어한 퀘스트 목록)
            HashSet<int> clearList = clearQuestData.ToObject<HashSet<int>>();

            if (clearList != null)
            {
                foreach (var i in clearList)
                {
                    gm.questManager.completedQuests.Add(i);
                }
            }

        }

        // AES-256 암호화
        private string Aes256Encrypt(string data)
        {
            byte[] encrypted;

            // Aes 클래스는 IDisposable 인터페이스를 구현하고 있기 때문에 사용이 끝나면 반드시 리소스를 해제 해주어야 함
            using (Aes aes = Aes.Create())
            {
                // 키 사이즈 설정(128, 192, 256비트) 키 사이즈가 클수록 더 높은 보안 (기본값 : 256)
                aes.KeySize = 256;

                // 블록 사이즈 설정(128이 표준이기 때문에 항상 128로 사용) (기본값 : 128)
                aes.BlockSize = 128;

                // 암호화 모드 설정 (ECB, CBC, CFB, OFB, CTR, GCM) 일반적으로 CBC, CTR, GCM 사용 (기본값 : CBC)
                aes.Mode = CipherMode.CBC;

                // 패딩 모드 설정 (PKCS7, ZeroPadding, ANSI X.923, ISO 10126) PKCS7이 가장 많이 사용됨 (기본값 : PKCS7)
                aes.Padding = PaddingMode.PKCS7;

                // 복호화 시 필요한 Key, IV(Initialize Vector) 생성
                aes.GenerateKey();
                aes.GenerateIV();

                // CreateDecryptor 또는 CreateEncryptor 메서드를 사용해 암호화/복호화 인터페이스 생성
                ICryptoTransform encrpytor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Key와 IV 값 암호화 후 정해진 경로에 파일 저장
                // 사용자의 운영체제가 윈도우/맥 이라면 Key/IV 암호화, 윈도우/맥이 아니라면 그대로 저장
                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        LockKeyOrIV(keyPath, aes.Key);
                        LockKeyOrIV(ivPath, aes.IV);
                    }

                    // 테스트 불가능
                    //else if (OperatingSystem.IsMacOS())
                    //{
                    //    AddToKeyOrIVForMac("AESKey", aes.Key);
                    //    AddToKeyOrIVForMac("AESIV", aes.IV);
                    //}

                    else
                    {
                        File.WriteAllBytes(keyPath, aes.Key);
                        File.WriteAllBytes(ivPath, aes.IV);
                    }
                }
                catch
                {
                    File.WriteAllBytes(keyPath, aes.Key);
                    File.WriteAllBytes(ivPath, aes.IV);
                }

                // 스트림 : 데이터가 연속적으로 이동하는 통로
                // MemoryStream : 메모리 내 바이트 배열을 스트림으로 다루기 위한 클래스 // IDisposable
                using (MemoryStream ms = new MemoryStream())
                {
                    // CryptoStream : 데이터의 암호화/복호화를 위한 클래스(암호화 : CryptoStreamMode.Write, 복호화 : CryptoStreamMode.Read) // IDisposable
                    using (CryptoStream cs = new CryptoStream(ms, encrpytor, CryptoStreamMode.Write))
                    {
                        // StreamReader : 바이트 스트림을 문자열로 읽기 위한 클래스 // IDisposable
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            // 문자열을 StreamWriter를 통해 CryptoStream에 쓰면,
                            // CryptoStream이 암호화하여 MemoryStream에 저장함
                            sw.Write(data);
                        }
                        // ms에 암호화 된 데이터 byte[]로 변환
                        encrypted = ms.ToArray();
                    }
                }
            }

            // byte[]을 문자열로 변환
            return Convert.ToBase64String(encrypted);
        }

        // AES-256 복호화
        private string Aes256Decrypt(string data)
        {
            string result = string.Empty;
            byte[] bytes = Convert.FromBase64String(data);

            using (Aes aes = Aes.Create())
            {
                // Aes 기본 설정 사용 (키 사이즈 256, 블록 사이즈 128, 암호화 모드 CBC, 패딩모드 PKCS7)

                try
                {
                    // PC에 저장된 암호화 된 Key, IV(Initialize Vector) 값을 복호화해서 가져오기
                    if (OperatingSystem.IsWindows())
                    {
                        aes.Key = UnLockKeyOrIV(keyPath);
                        aes.IV = UnLockKeyOrIV(ivPath);
                    }
                    // 사용자의 운영체제가 MacOS 일 경우 (테스트 불가능)
                    //else if (OperatingSystem.IsMacOS())
                    //{
                    //    aes.Key = SearchToKeyOrIVForMac("AESKey");
                    //    aes.IV = SearchToKeyOrIVForMac("AESIV");
                    //}
                    else
                    {
                        aes.Key = File.ReadAllBytes(keyPath);
                        aes.IV = File.ReadAllBytes(ivPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"키 또는 IV를 가져오는 데 실패했습니다. : {ex.Message}");

                    aes.Key = File.ReadAllBytes(keyPath);
                    aes.IV = File.ReadAllBytes(ivPath);
                }

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            // MemoryStream에 저장된 암호화된 데이터를 CryptoStream이 복호화하고,
                            // StreamReader가 이를 문자열로 변환하여 읽음
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }

            return result;
        }

        // 경로에 저장된 암호화 된 Key, IV 복호화 (윈도우에서만 작동)
        private byte[] UnLockKeyOrIV(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
        }

        // 저장 시 생성한 Key, IV 암호화 (윈도우에서만 작동)
        private void LockKeyOrIV(string path, byte[] data)
        {
            byte[] lockData = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);
            File.WriteAllBytes(path, lockData);
        }

        //// Key,IV 키체인에 추가 (Mac) (테스트 불가능)
        //private void AddToKeyOrIVForMac(string name, byte[] data)
        //{
        //    SecRecord secRecord = new SecRecord(SecKind.GenericPassword)
        //    {
        //        Service = "SpartaTextRPG05", // 키체인 서비스 이름 설정
        //        Account = name, // 계정 이름 설정 (Key, IV 식별자)
        //        ValueData = NSData.FromArray(data) // 넣을 데이터(Key 또는 IV)
        //    };

        //    // 키체인에 새 항목 추가
        //    SecStatusCode result = SecKeyChain.Add(secRecord);

        //    // 새항목 추가 시 이미 동일한 항목이 존재한다면 기존 항목 업데이트
        //    if (result == SecStatusCode.DuplicateItem)
        //    {
        //        SecRecord record = new SecRecord(SecKind.GenericPassword)
        //        {
        //            Service = "SpartaTextRPG05",
        //            Account = name,
        //        };
        //        SecStatusCode updateResult = SecKeyChain.Update(record, secRecord);
        //    }
        //}

        //// 키체인에서 Key,IV 가져오기 (Mac) (테스트 불가능)
        //private byte[] SearchToKeyOrIVForMac(string name)
        //{
        //    SecRecord secRecord = new SecRecord(SecKind.GenericPassword)
        //    {
        //        Service = "SpartaTextRPG05",
        //        Account = name
        //    };

        //    SecStatusCode status;
        //    // 키체인에서 항목 검색 후 status에 결과 추가, result에 값 넣어주기
        //    SecRecord result = SecKeyChain.QueryAsRecord(secRecord, out status);

        //    // result에 값이 들어갔다면 byte[]로 변환 후 리턴
        //    if (status == SecStatusCode.Success)
        //    {
        //        return result.ValueData.ToArray();
        //    }
        //    else
        //    {
        //        throw new Exception("키체인에서 값을 찾지 못했습니다.");
        //    }
        //}
    }
}
