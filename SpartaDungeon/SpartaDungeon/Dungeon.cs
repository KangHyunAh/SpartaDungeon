using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Dungeon
    {
        public int EnterHp { get; set; }

        Player player { get; set; } = null;
        List<Monster> monsters { get; set; }
        List<Monster> monsterList { get; set; }
        List<Monster> bossmonsterList { get; set; }

        Random random = new Random();

        public void Battle(Player _player, List<Monster> _monsters, List<Monster> _monsterList,List<Monster> _bossMonsterList)
        {
            EnterHp = _player.healthPoint;
            player = _player;
            monsters = _monsters;
            monsterList = _monsterList;
            bossmonsterList = _bossMonsterList;

            if (!(player.dungeonLevel%3 == 0))
            {
                MonsterSpawn(monsterList, random.Next(1, 5));
            }
            else
            {
                MonsterSpawn(bossmonsterList, 1);
                MonsterSpawn(monsterList,random.Next(1,5));
            }
            ReadyBattle();
        }
        public void ReadyBattle()
        {
            ScreenText("Battle!!");
            Console.WriteLine("[몬스터 정보]");
            Console.WriteLine();

            MonsterInfo();

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv. {player.level} [{player.name}]({player.chad})");
            Hpbar();
            Mpbar();
            Console.WriteLine();
            Console.WriteLine("1. 스킬");
            Console.WriteLine("2. 기본 공격");
            Console.WriteLine();
            int input = Utility.GetInput(1, 2);
            if (input == 1)
                SkillChoiceBattle();
            else
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

            int input = Utility.GetInput(1, 2);
            if (player.skills[input - 1].Type == SkillType.Attack)
                TargetBattle(true, input);
            else if (player.skills[input - 1].Type == SkillType.Heal)
                PlayerSkillHeal(input - 1);
        }

        public void TargetBattle(bool useskill = false, int skillnum = 0)
        {
            ScreenText("Battle!!");

            MonsterInfo();

            Console.WriteLine();
            Hpbar();
            Console.WriteLine();
            Console.WriteLine();
            if (useskill && player.skills[skillnum - 1].Count == 5)
            {
                Console.WriteLine("0. 전체기");
                int input = Utility.GetInput(0,0);
                PlayerSkillAttack(skillnum - 1, 0);
            }
            else if(useskill && player.skills[skillnum - 1].Count > 1)
            {
                Console.WriteLine("0. 무작위 공격");
                int input = Utility.GetInput(0, 0);
                PlayerSkillAttack(skillnum - 1, 0);
            }
            else
            {
                Console.WriteLine($"1~{monsters.Count}. 대상을 선택해주세요.");
                while (true)
                {
                    int input = Utility.GetInput(1, monsters.Count);
                    if (monsters[input - 1].Health < 0)
                        Console.WriteLine("이미 사망한 대상입니다.");
                    else if (useskill)
                    {
                        PlayerSkillAttack(skillnum - 1, input - 1);
                        break;
                    }
                    else if (!useskill)
                    {
                        PlayerBasicAttack(input - 1);
                        break;
                    }
                }
            }

        }

        public void PlayerSkillHeal(int skillnum)
        {
            ScreenText("Battle!! - 도핑");
            Console.WriteLine($"{player.name}가 {player.skills[skillnum].Name}을(를) 사용!");
            Hpbar();
            Mpbar();
            player.skills[skillnum].HealSkill(player);
            Console.WriteLine();
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine();
            Hpbar();
            Mpbar();
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
                MonsterAttack();
        }

        public void PlayerSkillAttack(int skillnum, int target)
        {
            ScreenText("Battle!! - Player의 턴");

            Console.Write($"{player.name}의 {player.skills[skillnum].Name} 발동!  ");
            if (player.skills[skillnum].UseHp != 0)
                Console.Write($"[Hp 소모 : {player.skills[skillnum].UseHp}] ");
            if(player.skills[skillnum].UseMp != 0)
                Console.Write($"[Mp 소모 : {player.skills[skillnum].UseMp}]");

            Console.WriteLine();
            Console.WriteLine();

            List<Monster> livemonster = new List<Monster>();

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Health > 0)
                {
                    livemonster.Add(monsters[i]);
                    Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                }
            }

            player.skills[skillnum].AttackSkill(monsters, player, target);
            Console.WriteLine();
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine();

            for (int i = 0; i < livemonster.Count; i++)
            {
                if (monsters[i].Health > 0)
                    Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} Hp : {monsters[i].Health}");
                else
                {
                    Utility.ColorText(ConsoleColor.DarkGray, $"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} [Dead]");
                }
            }
            livemonster.Clear();

            Console.WriteLine();
            Hpbar();
            Mpbar();
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
            {
                if(monsters.Count(m => m.Health > 0) != 0)
                    MonsterAttack();
                else
                    BattleResult();
            }
        }

        public void PlayerBasicAttack(int target)
        {
            ScreenText("Battle!! - Player의 턴");

            int[] monsternum = new int[monsters.Count];
            for (int i = 0; i < monsters.Count; i++)
            {
                monsternum[i] = monsters[i].Health;
            }
            Console.WriteLine($"{player.name} 의 공격!");

            int damage = (int)Math.Ceiling(random.NextDouble() * ((float)(player.strikePower + player.equipStrikePower) * 0.2) + (float)(player.strikePower + player.equipStrikePower) * 0.9);
            int critical = random.Next(15, 100);
            if (critical < 15)
            {
                damage = (int)((float)damage * 1.6);
            }
            int evasion = random.Next(0, 10);

            if(evasion > 0)
                monsters[target].Health -= damage;

            if (evasion == 0)
                Console.WriteLine("몬스터가 공격을 회피했다!");
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
            ScreenText("Battle!! - Monster의 턴");


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
                    ReadyBattle();
            }
                

        }

        public void BattleResult()
        {
            ScreenText("Battle - Result");

            if (player.healthPoint > 0)
            {
                int goldSum = 0;
                int expSum = 0;
                float getAtk = 0;
                for (int i = 0; i < monsters.Count; i++)
                {
                    if(monsters[i].MonsterType == "보스")
                        getAtk += (float)monsters[i].Atk / 5;
                    else if (monsters[i].MonsterType == player.chad)
                        getAtk += (float)monsters[i].Atk / 10;
                    else
                        getAtk += (float)monsters[i].Atk / 20;
                    if (getAtk >= 1)
                        player.strikePower += (int)getAtk;
                    player.gold += monsters[i].Rewards;
                    goldSum += monsters[i].Rewards;
                    player.exp += (monsters[i].Exp+player.dungeonLevel*3);
                    expSum += (monsters[i].Exp + player.dungeonLevel * 3);
                }

                player.dungeonLevel += 1;
                Utility.ColorText(ConsoleColor.Yellow, "Victory");
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 잡았습니다.");
                if((int)getAtk>=1)
                    Console.WriteLine($"몬스터들의 흔적을 읽어 {(int)getAtk}만큼의 힘이 상승했습니다.");
                Console.WriteLine($"획득 경험치 {expSum}");
                Console.WriteLine($"획득 골드 : {goldSum} G");

                player.ControlLevel();
            }
            else
                Console.WriteLine("You Lose");

            Console.WriteLine();
            Console.WriteLine($"Lv. {player.level} {player.name}");
            Console.WriteLine($"Hp. {EnterHp} => {player.healthPoint}");
            Console.WriteLine();

            Hpbar();
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
                if (monsterList[i].MonsterType != "이벤트")
                {
                    monsters.Add(monster.Spawn());
                    monsters[i].Atk += player.dungeonLevel * 5;
                    monsters[i].Health += player.dungeonLevel * 3;
                }
                else
                    monsters.Add(monster.Spawn());
            }
        }
        public void ScreenText(string tag)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine(tag);
            Console.WriteLine();
        }
        public void MonsterInfo()
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Health > 0)
                    Console.WriteLine($"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} \n     Hp : {monsters[i].Health} Atk : {monsters[i].Atk}");
                else
                {
                    Utility.ColorText(ConsoleColor.DarkGray, $"-[{i + 1}] Lv. {monsters[i].Lv} {monsters[i].Name} [Dead]");
                }
                Console.WriteLine();
            }
        }
        public void Hpbar()
        {
            int viewHp = (int)((float)player.healthPoint / (player.maxhealthPoint/10));
            viewHp = Math.Min(viewHp, 10);
            Console.WriteLine($"Hp. {player.healthPoint} / {player.maxhealthPoint}");
            for (int i = 0; i < viewHp; i++)
            {
                Console.Write("■");
            }
            for (int i = 0; i < 10 - viewHp; i++)
            {
                Console.Write("□");
            }
            
        }
        public void Mpbar()
        {
            Console.WriteLine();
            int viewMp = (int)((float)player.manaPoint / (player.maxManaPoint / 10));
            viewMp = Math.Min(viewMp, 10);
            Console.WriteLine($"Mp. {player.manaPoint} / {player.maxManaPoint}");
            for (int i = 0; i < viewMp; i++)
            {
                Console.Write("■");
            }
            for (int i = 0; i < 10 - viewMp; i++)
            {
                Console.Write("□");
            }
            Console.WriteLine();
        }
    }
}
