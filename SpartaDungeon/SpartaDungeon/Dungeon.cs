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
        public int EnterHp { get; set; }

        Player player { get; set; } = null;
        List<Monster> monsters { get; set; } = null;

        Random random = new Random();

        public void Battle(Player _player, List<Monster> _monsters, List<Monster> monsterList,List<Monster> bossMonsterList)
        {
            EnterHp = _player.healthPoint;
            player = _player;
            monsters = _monsters;

            ScreenText("Battle!!");
            Console.WriteLine("[몬스터 정보]");
            Console.WriteLine();
            if (!(player.dungeonLevel%3 == 0))
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
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv. {player.level} {player.name} ({player.chad})");
            Hpbar();
            Console.WriteLine();
            Console.WriteLine("1. 스킬");
            Console.WriteLine("2. 기본 공격");
            Console.WriteLine();
            int input = Utility.GetInput(1, 2);
            if(input ==1)
                SkillChoiceBattle();
            else if (input == 2)
                TargetBattle();
        }

        public void SkillChoiceBattle()
        {
            ScreenText("스킬 선택");
            for(int i = 0; i < player.skills.Count; i++)
            {
                Console.WriteLine($"{i+1}.{player.skills[i].Name}");
                Console.WriteLine($"  {player.skills[i].Description}") ;
                Console.WriteLine();
            }
            Console.WriteLine((player.skills[1].Count));
            int input = Utility.GetInput(1, player.skills.Count);

            //if (player.skills[input - 1].Count > 0)
            //{
            //    PlayerSkillAttack(input - 1, 0);
            //}
            //else
            {
                ScreenText("Battle!!");

                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i].Health > 0)
                        Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                    else
                        Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} [Dead]");
                }

                Console.WriteLine();
                Hpbar();
                Console.WriteLine();
                Console.WriteLine($"1~{monsters.Count}. 대상을 선택해주세요.");
                while (true)
                {
                    int skilltarget = Utility.GetInput(1, monsters.Count);
                    if (monsters[input - 1].Health < 0)
                        Console.WriteLine("이미 사망한 대상입니다.");
                    else
                    {
                        PlayerSkillAttack(input - 1, skilltarget - 1);
                        break;
                    }
                }
            }
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
            Hpbar();
            Console.WriteLine();
            Console.WriteLine($"1~{monsters.Count}. 대상을 선택해주세요.");
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
        public void BattleUseItem()
        {
            ScreenText("소모아이템");
        }

        public void PlayerSkillAttack(int skillnum, int target)
        {
            ScreenText("Battle!!");

            Console.WriteLine($"{player.name}의 {player.skills[skillnum].Name} 발동!");
            Console.WriteLine();
            for(int i = 0;i < monsters.Count;i++)
            { 
                Console.WriteLine($"-Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
            }
            player.skills[skillnum].AttackSkill(monsters, player, target);
            Console.WriteLine();
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine();
            for (int i = 0; i < monsters.Count; i++)
            {
                if(monsters[i].Health >0)
                    Console.WriteLine($"-Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                else
                    Console.WriteLine($"-Lv. {monsters[i].Lv} {monsters[i].Name} Hp : [Dead]");
            }

            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
                MonsterAttack();
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
            int critical = random.Next(15, 100);
            if (critical < 15)
            {
                damage = (int)((float)damage * 1.6);
            }
            int evasion = random.Next(0, 10);

            if(evasion > 0)
                monsters[target].Health -= damage;

            if (evasion == 0)
                Console.WriteLine("헛방질");
            else if (critical < 15)
                Console.WriteLine($"{monsters[target].Name} 을(를) 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!");
            else
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

                        int Critical = random.Next(0, 100);
                        if (Critical < 15)
                        {
                            damage = (int)((float)damage * 1.6);
                        }
                        damage = Math.Max(0,damage);

                        int evasion = random.Next(0, 10);
                        if(evasion > 0)
                            player.healthPoint -= damage;

                        if (evasion == 0)
                        {
                            Console.WriteLine($"{monster.Name}의 공격을 {player.name}은(는) 회피하였습니다.");
                            Hpbar();
                            Thread.Sleep(300);
                        }
                        else if (damage == 0)
                        {
                            Console.WriteLine($"{monster.Name}의 공격을 {player.name}은(는) 높은 방어력으로 공격을 막아냈습니다.");
                            Hpbar();
                            Thread.Sleep(300);
                        }
                        else
                        {
                            if (player.healthPoint > 0)
                            {
                                if (Critical < 15)
                                {
                                    Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!");
                                    Hpbar();
                                    Thread.Sleep(300);
                                }
                                else
                                {
                                    Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}]");
                                    Hpbar();
                                    Thread.Sleep(300);
                                }
                            }
                            else
                            {
                                player.healthPoint = 0;
                                if (Critical < 15)
                                {
                                    Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!\n[Hp {playerHp} => [Dead]");
                                    Hpbar();
                                    Thread.Sleep(300);
                                }
                                else
                                {
                                    Console.WriteLine($"{monster.Name}은 {player.name}에게 공격을 맞췄습니다. [데미지 : {damage}][Hp {playerHp} => [Dead]");
                                    Hpbar();
                                    Thread.Sleep(300);
                                }
                            }
                            
                        }
                        Console.WriteLine();
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
                int goldSum = 0;
                int expSum = 0;
                for (int i = 0; i < monsters.Count; i++)
                {
                    player.gold += monsters[i].Rewards;
                    goldSum += monsters[i].Rewards;
                    player.exp += (monsters[i].Exp+player.dungeonLevel*3);
                    expSum += (monsters[i].Exp + player.dungeonLevel * 3);
                }

                player.dungeonLevel += 1;
                Console.WriteLine("Victory");
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
                Console.WriteLine($"획득 경험치 {expSum}");
                Console.WriteLine($"획득 골드 : {goldSum} G");
                player.ControlLevel();
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
                monsters[i].Atk += player.dungeonLevel * 5;
                monsters[i].Health += player.dungeonLevel * 3;
            }
        }
        public void ScreenText(string tag)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine(tag);
            Console.WriteLine();
        }
        public void Hpbar()
        {
            int viewHp = (int)((float)player.healthPoint / 10);
            Console.WriteLine($"Hp. {player.healthPoint}");
            for (int i = 0; i < viewHp; i++)
            {
                Console.Write("■");
            }
            for (int i = 0; i < 10 - viewHp; i++)
            {
                Console.Write("□");
            }
        }
    }
}
