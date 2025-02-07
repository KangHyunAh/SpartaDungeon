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
        public int EnterHp { get; set; }

        Player player { get; set; }
        List<Monster> monsters { get; set; }


        Random random = new Random();
        public Dungeon()
        {
            DungeonLv = 1;
        }

        public void Battle(Player _player, List<Monster> _monsters, List<Monster> monsterList,List<Monster> bossMonsterList)
        {
            ScreenText("Battle!!");

            EnterHp = _player.healthPoint;
            player = _player;
            monsters = _monsters;


            if(!(DungeonLv%3 == 0))
            {
                MonsterSpawn(monsterList, random.Next(1, 5));
            }
            else
            {
                MonsterSpawn(bossMonsterList, 1);
                MonsterSpawn(monsterList,random.Next(1,5));
            }

            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"-Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
            }

            Console.WriteLine();
            Console.WriteLine("내정보");
            Console.WriteLine($"Lv. {player.level} {player.name} ({player.chad})");
            Console.WriteLine($"Hp. {player.healthPoint}");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            if (input == 0)
                TargetBattle();
        }

        public void TargetBattle()
        {
            ScreenText("Battle!!");

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Health > 0)
                    Console.WriteLine($"-[{i+1}] Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                else
                    Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} [Dead]");
            }

            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");
            while(true)
            {
                int input = Utility.GetInput(1, monsters.Count);
                if (monsters[input - 1].Health < 0)
                    Console.WriteLine("이미 사망한 대상입니다.");
                else
                {
                    PlayerAttack(input - 1);
                    break;
                }
            }

        }

        public void PlayerAttack(int target)
        {
            ScreenText("Battle!!");

            int[] monsternum = new int[monsters.Count];
            for (int i = 0; i < monsters.Count; i++)
            {
                monsternum[i] = monsters[i].Health;
            }
            Console.WriteLine($"{player.name} 의 공격!");
            int damage = (int)Math.Ceiling(random.NextDouble() * ((float)(player.strikePower+player.equipStrikePower) * 0.1) + (player.strikePower + player.equipStrikePower) - (float)(player.strikePower + player.equipStrikePower) *0.05);
            monsters[target].Health -= damage;
            Console.WriteLine($"{monsters[target].Name} 을(를) 맞췄습니다. [데미지 : {damage}]");

            Console.WriteLine();

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Health <= 0 && monsternum[i] >0)
                {
                    Console.WriteLine($"Lv. {monsters[i].Lv} {monsters[i].Name}");
                    Console.WriteLine($"HP. {monsternum[i]} => Dead");
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
            {
                int livemonsters = monsters.Count(m => m.Health > 0);
                if (!(player.healthPoint > 0 && livemonsters > 0))
                    BattleResult();
                else
                    MonsterAttack();
            }
        }

        public void MonsterAttack()
        {
            ScreenText("Battle!!");


            foreach (Monster monster in monsters)
            {
                int playerHp = player.healthPoint;
                if (monster.Health > 0)
                {
                    if(player.healthPoint > 0)
                    {
                        Console.WriteLine($"{monster.Name} 의 공격!");
                        int damage = monster.Atk - player.defensivePower;
                        damage = Math.Max(0,damage);
                        player.healthPoint -= damage;
                        if(damage ==0)
                            Console.WriteLine($"{monster.Name}의 공격을 {player.name}은(는) 높은 방어력으로 공격을 막아냈습니다.");
                        else
                        {
                            if (player.healthPoint > 0)
                                Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}][Hp {playerHp} => {player.healthPoint}]");
                            else
                                Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}][Hp {playerHp} => [Dead]");
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
            {
                int livemonsters = monsters.Count(m => m.Health > 0);
                if (!(player.healthPoint > 0 && livemonsters > 0))
                    BattleResult();
                else
                    TargetBattle();
            }
                

        }

        public void BattleResult()
        {
            ScreenText("Battle - Result");

            if (player.healthPoint > 0)
            {
                int sum = 0;
                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i].Health <= 0)
                    {
                        player.gold += monsters[i].Rewards;
                        sum += monsters[i].Rewards;
                    }
                }

                DungeonLv += 1;
                Console.WriteLine("Victory");

                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
                Console.WriteLine($"획득 골드 : {sum} G");
            }
            else
                Console.WriteLine("You Lose");

            Console.WriteLine();
            Console.WriteLine($"Lv. {player.level} {player.name}");
            Console.WriteLine($"Hp. {EnterHp} => {player.healthPoint}");
            monsters.Clear();
            Console.WriteLine();
            Console.WriteLine("0. 메뉴로");
            Console.WriteLine();
            Utility.GetInput(0, 0);
        }

        public void MonsterSpawn(List<Monster> monsterList, int spawnNum)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                Monster monster = monsterList[random.Next(0, monsterList.Count)];
                monsters.Add(monster.Spawn());
                monsters[i].Atk += DungeonLv * 5;
                monsters[i].Health += DungeonLv * 3;
            }
        }
        public void ScreenText(string tag)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine(tag);
            Console.WriteLine();
        }
    }
}
