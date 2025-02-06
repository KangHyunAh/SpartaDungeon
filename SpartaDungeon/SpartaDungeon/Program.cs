using System.Numerics;

namespace SpartaDungeon
{
    public class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager();

            gm.InventoryScreen();
        }

        public void SHopScreen()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"/player.Gold G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < equipItemList.Count; i++)
            {
                equipItemList[i].DisplayShopItem();
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
        }


    }
}

