using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public static int GetInputPlus(int min, int max, int[] additionalNumsArr)  //범위내 숫자와 별개로 여러 다른 숫자를 배열로받아 추가로 선택가능
    {
        while (true) //return이 되기 전까지 반복
        {
            Console.Write("원하시는 행동을 입력해주세요.\n>>");

            //int.TryParse는 int로 변환이 가능한지 bool값을 반환, 가능(true)할 경우 out int input으로 숫자도 반환
            if (int.TryParse(Console.ReadLine(), out int input) && ((input >= min) && (input <= max)||additionalNumsArr.Contains(input)))
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

    public static void RealTab(string String,bool LeftofRight,int gap)
    {
        Regex regex = new Regex("[가-힣]");
        MatchCollection koreanNum = regex.Matches(String);
        if (LeftofRight) Console.Write(String);
        Console.Write(new string(' ', gap >= String.Length+koreanNum.Count ? (gap - (koreanNum.Count + String.Length)) : 0));
        if (!LeftofRight) Console.Write(String);
    }





    public enum ArtEnum
    {
        Dungeon,
        Shop

    }
    public static void Art(ArtEnum artEnum,int gapSpaceInt = 0)     //그림 그리기 메서드
    {
        switch (artEnum)
        {
            case ArtEnum.Dungeon: DungeonArt(gapSpaceInt);break;
            case ArtEnum .Shop: ShopArt(gapSpaceInt); break;
        }


        void DungeonArt(int gapSpaceInt)
        {
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("  _______    ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine(" /  ___  \\   ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("|  /   \\  |  ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("| /_____\\ |  ");
        }

        void ShopArt(int gapSpaceInt)
        {
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("    <---->   ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("    /    \\   ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("   / shop \\  ");
            Console.Write(new string(' ', gapSpaceInt)); Console.WriteLine("   \\______/  ");
        }

    }
}
