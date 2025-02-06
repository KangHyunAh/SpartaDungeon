using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Utility
{
    //입력키 확인
    public static int GetInput(int min, int max)
    {
        while (true) //return이 되기 전까지 반복
        {
            Console.Write("원하시는 행동을 입력해주세요.\n>>");

            //int.TryParse는 int로 변환이 가능한지 bool값을 반환, 가능(true)할 경우 out int input으로 숫자도 반환
            if (int.TryParse(Console.ReadLine(), out int input) && (input >= min) && (input <= max))
                return input;

            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
        }
    }
}
