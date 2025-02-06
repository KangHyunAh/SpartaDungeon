using System;
using System.Collections.Generic;
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

        public void CharacterInformation() 
        {
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine("");

            Console.WriteLine("Lv. "+ level);
            Console.WriteLine("이름 : " + name);
            Console.WriteLine("Chad : "+ chad);
            Console.WriteLine("공격력 : " + strikePower +" + "+ equipStrikePower);
            Console.WriteLine("방어력 : " + defensivePower+" + " + equipDefensivePower);
            Console.WriteLine("체력 : " + healthPoint);
            Console.WriteLine("최대 체력 : " + healthPoint+ " + " + equipMaxhealthPoint);
            Console.WriteLine("소유 골드 : " + gold + " G");

            Console.WriteLine("0. 나가기");
            if(Utility.GetInput(0, 0) == 0)
            {
                Console.WriteLine("창을 닫습니다.");
                return;
            }
        }

        public void Rest() 
        {
            Console.WriteLine("휴식을 선택하셨습니다.");
            Console.WriteLine("100골드를 소모하고 체력을 최대치만큼 회복합니다.");
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
                if ((gold - 100) < 0) //골드 소모 + 최대체력으로 회복, 부족하면 함수 이탈
                {
                    gold = gold - 100;
                    healthPoint = maxhealthPoint;
                    Console.WriteLine("현재 골드 : " + gold + " G");
                    Console.WriteLine("현재 체력 : " + healthPoint);
                    return;
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                    return;
                }
            }
        }
    }
}
