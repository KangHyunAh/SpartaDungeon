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

        public void Battle(Player player, List<Monster> monsters, List<Monster> monsterList)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();

            for (int i = 0; i < random.Next(1, monsters.Count+DungeonLv); i++)
            {
                monsters.Add(monsterList[random.Next(0,monsterList.Count)]);
                monsters[i].Lv += DungeonLv;
                monsters[i].Atk += DungeonLv;
                monsters[i].Health += DungeonLv * 10;
            }

            for(int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"-Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
            }

            Console.WriteLine();
            Console.WriteLine("내정보");
            Console.WriteLine($"Lv. {player.level} {player.name} ({player.chad})");
            Console.WriteLine($"Hp. {player.healthPoint}");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            int input = Utility.GetInput(0, 0);
            if (input == 0)
                ReadyBattle(player, monsters);
        }

        public void ReadyBattle(Player player, List<Monster> monsters)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Health > 0)
                    Console.WriteLine($"-[{i+1}] Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                else
                    Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} [Dead]");
            }
            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");
            int input = Utility.GetInput(1, monsters.Count);
            PlayerAttack(player, monsters,input-1);

        }

        public void PlayerAttack(Player player, List<Monster> monsters, int target)
        {
            Console.Clear();
            

            Console.WriteLine();
            Console.WriteLine("Battle!!");
            Console.WriteLine();

            Console.WriteLine($"{player.name} 의 공격!");
            monsters[target].Health -= (int)Math.Ceiling(random.NextDouble() * (player.strikePower / 10 + player.strikePower - player.strikePower / 10));
            Console.WriteLine($"{monsters[target].Name} 을(를) 맞췄습니다. [데미지 : {player.strikePower}]");

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
            int playerHp = player.healthPoint;
            foreach (Monster monster in monsters)
            {
                if (monster.Health > 0)
                {
                    Console.WriteLine($"{monster.Name} 의 공격!");
                    player.healthPoint -= monster.Atk;
                    Console.WriteLine($"{player.name} 을(를) 맞췄습니다. [데미지 : {monster.Atk}][Hp {playerHp} => {player.healthPoint}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            if (input == 0)
            {
                int livemonsters = monsters.Count;
                foreach (Monster monster in monsters)
                {
                    if(monster.Health <= 0)
                        livemonsters--;
                }
                if (!(player.healthPoint > 0 && livemonsters <= 0))
                    BattleResult(player, monsters);
                else
                    ReadyBattle(player, monsters);
            }
                

        }

        public void BattleResult(Player player, List<Monster> monsters)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Battle!! - Result");
            Console.WriteLine();
            if (player.healthPoint < 0)
            {
                Console.WriteLine("Victory");

                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
            }
            else
                Console.WriteLine("You Lose");
            monsters.Clear();
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);

        }
    }
}
