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
        public int defensivePower = 5;
        public int healthPoint = 100;
        public int gold = 1500;

        public void CharacterInformation() 
        {
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine("");

            Console.WriteLine("Lv. "+level);
            Console.WriteLine("Chad : "+ chad);
            Console.WriteLine("공격력 : " + strikePower);
            Console.WriteLine("방어력 : " + defensivePower);
            Console.WriteLine("체력 : " + healthPoint);
            Console.WriteLine("소유 골드 : " + gold + " G");

            Console.WriteLine("0. 나가기");
            Utility.GetInput(0, 0);
        }

    }
}
