using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Player
    {
        public int level = 1;
        public string name = "Rtan";
        public string chad = "전사";
        public int strikePower = 10;
        public int equipStrikePower = 0;
        public int defensivePower = 5;
        public int equipDefensivePower = 0;
        public int maxhealthPoint = 100;
        public int equipMaxhealthPoint = 0;
        public int healthPoint = 100;
        public int gold = 1500;
        public int exp = 0;
        public int maxExp = 50;
        public int dungeonLevel = 1;
        public int manaPoint = 50;
        public int maxManaPoint = 50;

        public List<Skill> skills = new List<Skill>();

        public void ControlLevel()//경험치 양 판단 뒤 레벨 올리기
        {
            if (exp >= maxExp) //경험치가 최대치를 넘거나 동일해졌을 때
            {
                exp = exp - maxExp;
                maxExp = (int)(1.1 * maxExp);
                strikePower = (int)(1.1 * strikePower);
                defensivePower = (int)(1.2 * defensivePower);
                maxhealthPoint = (int)(1.03 * maxhealthPoint);
                level++;
                Utility.ColorText(ConsoleColor.Yellow, "레벨 업!");
                if (exp >= maxExp)
                {
                    this.ControlLevel();
                }
            }
        }
        public void DisplayHpBar()    //체력,마나 막대 표시
        {
            Console.Write($"HP {healthPoint,4}/{(maxhealthPoint + equipMaxhealthPoint),4}({maxhealthPoint}+{equipMaxhealthPoint})\n");
            for (int i = 0; i < 10; i++)
            {
                if (i < (float)healthPoint / (maxhealthPoint + equipMaxhealthPoint) * 10) Utility.ColorText(ConsoleColor.Green, "■", Text.Write);
                else Console.Write("□");
            }
        }
        public void DisplayMpBar()    //체력,마나 막대 표시
        {
            Console.Write($"MP {manaPoint,4}/{maxManaPoint,4} \n");
            for (int i = 0; i < 10; i++)
            {
                if (i < (float)manaPoint / maxManaPoint * 10) Utility.ColorText(ConsoleColor.Blue, "■", Text.Write);
                else Console.Write("□");
            }
        }

        public void CharacterInformation()//상태창 
        {
            Console.Clear();
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine("");
            Console.WriteLine("Lv. " + level);
            Console.WriteLine("이름 : " + name);
            Console.WriteLine("직업 : " + chad);
            Console.WriteLine("공격력 : " + (strikePower + equipStrikePower) + "(" + strikePower + " + " + equipStrikePower + ")");
            Console.WriteLine("방어력 : " + (defensivePower + equipDefensivePower) + "(" + defensivePower + " + " + equipDefensivePower + ")");
            DisplayHpBar(); Console.WriteLine();
            DisplayMpBar(); Console.WriteLine();
            Console.WriteLine("경험치 : " + exp + " / " + maxExp);
            Console.WriteLine("소유 골드 : " + gold + " G");
            Console.WriteLine("0. 나가기");
            if (Utility.GetInput(0, 0) == 0)
            {
                Console.WriteLine("창을 닫습니다.");

                return;
            }
        }

        public void Rest()
        {
            Console.Clear();
            Console.WriteLine("휴식을 선택하셨습니다.");
            Console.WriteLine("여관에 입장하셨습니다.");
            Console.WriteLine("여관에서는 100골드를 소모하고 체력과 마나를 최대치만큼 회복합니다.");
            Console.WriteLine("현재 골드 : " + gold + " G");
            Console.WriteLine("실행하시겠습니까?");
            Console.WriteLine("1. 실행");
            Console.WriteLine("0. 나가기");
            if (Utility.GetInput(0, 1) == 0)
            {
                Console.WriteLine("창을 닫습니다.");
                return;
            }
            else
            {
                if ((gold - 100) < 0) //골드 부족
                {
                    Console.WriteLine("골드가 부족합니다.");
                    Console.WriteLine("0. 나가기");
                    if (Utility.GetInput(0, 0) == 0)
                    {
                        Console.WriteLine("창을 닫습니다.");
                        return;
                    }
                }
                else //골드 소모 + 최대체력(+장비)로 회복
                {
                    gold = gold - 100;
                    healthPoint = maxhealthPoint + equipMaxhealthPoint;
                    manaPoint = maxManaPoint;
                    Console.WriteLine("현재 골드 : " + gold + " G");
                    Console.WriteLine("현재 체력 : " + (healthPoint));
                    Console.WriteLine("현재 마나 : " + (manaPoint));
                    Console.WriteLine("0. 나가기");
                    if (Utility.GetInput(0, 0) == 0)
                    {
                        Console.WriteLine("창을 닫습니다.");
                        return;
                    }
                }
            }
        }

        public void ReleaseEquipMaxHealthPoint() //최대체력 관여 장비를 해제했을 때 관련 수치 조정
        {
            if (equipMaxhealthPoint > 0 && healthPoint >= maxhealthPoint)
            //최대체력 증가 장비가 존재하고, 현재 체력이 기존 최대 체력과 같거나 컸을 때
            {
                healthPoint = maxhealthPoint;
                equipMaxhealthPoint = 0;
                return;
            }
            else
            {
                return;
            }
        }

        public void ChadSelect()
        {
            Console.WriteLine("직업을 선택합니다.");
            Console.WriteLine(" ");
            Utility.ColorText(ConsoleColor.Yellow, "1. 나이트");
            Console.WriteLine("방어 특화 직업으로, 동료를 지키는 든든한 기사입니다.");
            Console.WriteLine(" ");
            Utility.ColorText(ConsoleColor.Yellow, "2. 검사");
            Console.WriteLine("공격 특화 직업으로, 강력한 검으로 적을 처단합니다.");
            Console.WriteLine(" ");
            Utility.ColorText(ConsoleColor.Yellow, "3. 광전사");
            Console.WriteLine("체력 특화 직업으로, 수많은 적을 한번에 공격할 수 있습니다.");
            Console.WriteLine(" ");
            Utility.ColorText(ConsoleColor.DarkRed, "*주의");
            Utility.ColorText(ConsoleColor.DarkRed, "직업 선택 시 레벨, 경험치, 스탯이 초기화됩니다.");
            Console.WriteLine(" ");
            Console.WriteLine("1. 나이트 \n2. 검사 \n3. 광전사 \n0. 나가기");
            switch (Utility.GetInput(0, 3))
            {
                case 0: //창 닫기
                    Console.WriteLine("창을 닫습니다.");
                    return;

                case 1: //나이트
                    Console.WriteLine("나이트를 선택하셨습니다.");
                    Console.WriteLine("초기화를 진행합니다.");
                    chad = "나이트";
                    level = 1;
                    strikePower = 10;
                    defensivePower = 10;
                    maxhealthPoint = 100;
                    healthPoint = 100;
                    exp = 0;
                    maxExp = 50;
                    manaPoint = 50;
                    maxManaPoint = 50;
                    Console.WriteLine("당신의 직업은 나이트입니다.");
                    return;

                case 2: //검사
                    Console.WriteLine("검사를 선택하셨습니다.");
                    Console.WriteLine("초기화를 진행합니다.");
                    chad = "검사";
                    level = 1;
                    strikePower = 20;
                    defensivePower = 5;
                    maxhealthPoint = 100;
                    healthPoint = 100;
                    exp = 0;
                    maxExp = 50;
                    manaPoint = 50;
                    maxManaPoint = 50;
                    Console.WriteLine("당신의 직업은 검사입니다.");
                    return;

                case 3: //광전사
                    Console.WriteLine("광전사를 선택하셨습니다.");
                    Console.WriteLine("초기화를 진행합니다.");
                    chad = "광전사";
                    level = 1;
                    strikePower = 10;
                    defensivePower = 5;
                    maxhealthPoint = 200;
                    healthPoint = 200;
                    exp = 0;
                    maxExp = 50;
                    manaPoint = 50;
                    maxManaPoint = 50;
                    Console.WriteLine("당신의 직업은 광전사입니다.");
                    return;
            }
        }
    }
}
