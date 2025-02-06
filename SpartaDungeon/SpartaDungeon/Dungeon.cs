using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Dungeon
    {
        public int DungeonLv {  get; set; }
        Random random = new Random();
        public Dungeon() 
        {
            DungeonLv = 1;
        }

        public void Battle(Player player, List<Monster> monsters)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            foreach (Monster monster in monsters)
            {
                Console.WriteLine($"Lv. {monster.Lv} {monster.Name} Hp : {monster.Health}");
            }
            Console.WriteLine();
            Console.WriteLine("내정보");
            Console.WriteLine($"Lv. {player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"Hp {player.Hp}/{player.MaxHp}");
            Console.WriteLine();
            Console.WriteLine("0. 공격");
            int input = Utility.GetInput(0, 0);
            if (input == 1)
                PlayerAttack(player, monsters);
        }
        public void PlayerAttack(Player player, List<Monster> monsters)
        {
            Console.Clear();
            

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            foreach (Monster monster in monsters)
            {
                Console.WriteLine($"{player.Name} 의 공격!");
                monster.Health -= (int)Math.Ceiling(random.NextDouble() * player.Atk / 10 + player.Atk - player.Atk / 10);
                Console.WriteLine($"{monster.Name} 을(를) 맞췄습니다. [데미지 : {player.Atk}]");
            }
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            if (input == 0)
                MonsterAttack(player, monsters);
        }

        public void MonsterAttack(Player player, List<Monster> monsters)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            int playerHp = player.Hp;
            foreach (Monster monster in monsters)
            {
                if (monster.Health > 0)
                {
                    Console.WriteLine($"{monster.Name} 의 공격!");
                    player.Hp -= monster.Atk;
                    Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {monster.Atk}][Hp {playerHp} => {player.Hp}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            if (input == 0)
                Battle(player, monsters);
        }

        public void BattleResult(Player player, List<Monster> monsters)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!! - Result");
            Console.WriteLine();
            if (player.Hp < 0)
                Console.WriteLine("Victory");
            else
                Console.WriteLine("You Lose");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            if (input == 0)
                Battle(player,monsters);
        }
    }
}
