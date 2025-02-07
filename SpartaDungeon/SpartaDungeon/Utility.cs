using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Text
{
    Write,
    WriteLine
}

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

    // 출력 텍스트 색 변경
    public static void ColorText(ConsoleColor color, string text, Text type = Text.WriteLine)
    {
        Console.ForegroundColor = color;
        switch (type)
        {
            // 줄바꿈 X
            case Text.Write:
                Console.Write(text);
                break;
            // 줄바꿈 O
            case Text.WriteLine:
                Console.WriteLine(text);
                break;
        }
        Console.ResetColor();
    }
}
