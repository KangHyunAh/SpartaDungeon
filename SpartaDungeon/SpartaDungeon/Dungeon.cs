using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Dungeon
    {
        private int EnterHp { get; set; }
        private int EnterMp {  get; set; }
        public int ItemLimits { get; set; }
        GameManager gm { get; set; }
        Random random = new Random();


        private QuestManager questManager;
        private Player player;

        public Dungeon(QuestManager questManager, Player player)
        {
            this.questManager = questManager;
            this.player = player;
        }



        internal void Battle(GameManager gm)
        {
            this.gm = gm;
            EnterHp = gm.player.healthPoint;
            EnterMp = gm.player.manaPoint;
            ItemLimits = 2;
            if(gm.monsters.Count == 0)
            {
                if (!(gm.player.dungeonLevel % 3 == 0))
                {
                    MonsterSpawn(gm.monsterList, random.Next(1, 5));
                }
                else
                {
                    MonsterSpawn(gm.bossmonsterList, 1);
                    MonsterSpawn(gm.monsterList, random.Next(1, 4));
                }
            }
            else
                foreach (Monster monster in gm.monsters)
                {
                    monster.Health = monster.MaxHealth;
                }
            ReadyBattle();
        }
        public void ReadyBattle()
        {

            ScreenText($"Battle!! - {gm.player.dungeonLevel}층");
            Console.WriteLine("[몬스터 정보]");
            Console.WriteLine();

            MonsterInfo();

            Console.WriteLine();
            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar();
            Console.WriteLine();
            Console.WriteLine("0. 도망가기");
            Console.WriteLine("1. 스킬");
            Console.WriteLine("2. 기본 공격");
            Console.WriteLine($"3. 소모아이템(사용가능 횟수 : {ItemLimits})");
            Console.WriteLine();
            while (true)
            {
                int input = Utility.GetInput(0, 3);
                if (input == 0)
                    return;
                else if (input == 1)
                {
                    SkillChoiceBattle();
                    break;
                }
                else if (input == 2)
                {
                    TargetBattle();
                    break;
                }
                else if (input == 3 && ItemLimits > 0)
                {
                    gm.inventoryAndShop.ConsumableItemInventoryScreen(gm,true);
                    break;
                }
                else
                    Console.WriteLine("최대치만큼 사용하였습니다.");

            }

        }

        public void SkillChoiceBattle()
        {
            ScreenText("스킬 선택");
            for (int i = 0; i < gm.player.skills.Count; i++)
            {
                Console.Write($"{i + 1}.{gm.player.skills[i].Name}");
                if (gm.player.skills[i].UseHp > 0)
                    Console.Write($" [Hp소모 : {gm.player.skills[i].UseHp}]");
                if (gm.player.skills[i].UseMp > 0)
                    Console.Write($" [Mp소모 : {gm.player.skills[i].UseMp}]");
                Console.WriteLine();
                Console.WriteLine($"  {gm.player.skills[i].Description}");
                Console.WriteLine();
            }

            Console.WriteLine("0. 뒤로");
            while (true)
            {
                int input = Utility.GetInput(0, 2);
                if (input == 0)
                {
                    ReadyBattle();
                    break;
                }
                else if (gm.player.skills[input - 1].Type == SkillType.Attack && gm.player.skills[input - 1].UseMp <= gm.player.manaPoint && gm.player.skills[input - 1].UseHp <= gm.player.healthPoint)
                {
                    TargetBattle(true, input);
                    break;
                }
                else if (gm.player.skills[input - 1].Type == SkillType.Heal && gm.player.skills[input - 1].UseMp <= gm.player.manaPoint && gm.player.skills[input - 1].UseHp <= gm.player.healthPoint)
                {
                    PlayerSkillNonAttack(input - 1);
                    break;
                }
                else
                    Console.WriteLine("필요한 코스트가 부족합니다.");
            }
        }

        public void TargetBattle(bool useskill = false, int skillnum = 0)
        {
            ScreenText("Battle!!");

            MonsterInfo();

            Console.WriteLine();
            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar();
            Console.WriteLine();
            Console.WriteLine();
            if (useskill && gm.player.skills[skillnum - 1].Count == 5)
            {
                Console.WriteLine("0. 뒤로");
                Console.WriteLine("1. 전체기");

                int input = Utility.GetInput(0, 1);
                if (input == 0)
                    SkillChoiceBattle();
                else
                    PlayerSkillAttack(skillnum - 1, 0);
            }
            else if (useskill && gm.player.skills[skillnum - 1].Count > 1)
            {
                Console.WriteLine("0. 뒤로");
                Console.WriteLine("1. 무작위 공격");
                int input = Utility.GetInput(0, 1);
                if (input == 0)
                    SkillChoiceBattle();
                else
                    PlayerSkillAttack(skillnum - 1, 0);
            }
            else
            {
                Console.WriteLine("0. 뒤로");
                if (gm.monsters.Count == 1)
                    Console.WriteLine($"1. 대상을 선택해주세요.");
                else
                    Console.WriteLine($"1~{gm.monsters.Count}. 대상을 선택해주세요.");
                while (true)
                {
                    int input = Utility.GetInput(0, gm.monsters.Count);
                    if (input == 0 && useskill == true)
                    {
                        SkillChoiceBattle();
                        break;
                    }
                    else if (input == 0 && useskill == false)
                    {
                        ReadyBattle();
                        break;
                    }
                    else if (gm.monsters[input - 1].Health < 0)
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

        public void PlayerSkillNonAttack(int skillnum)
        {
            ScreenText("Battle!! - Player의 턴");
            Console.WriteLine($"{gm.player.name}가 {gm.player.skills[skillnum].Name}을(를) 사용!");
            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar();
            gm.player.skills[skillnum].HealSkill(gm.player);
            Console.WriteLine();
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine();
            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar();
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            int input = Utility.GetInput(0, 0);
            MonsterAttack();
        }

        public void PlayerSkillAttack(int skillnum, int target)
        {
            ScreenText("Battle!! - Player의 턴");

            Console.Write($"{gm.player.name}의 {gm.player.skills[skillnum].Name} 발동!  ");
            if (gm.player.skills[skillnum].UseHp != 0)
                Console.Write($"[Hp 소모 : {gm.player.skills[skillnum].UseHp}] ");
            if (gm.player.skills[skillnum].UseMp != 0)
                Console.Write($"[Mp 소모 : {gm.player.skills[skillnum].UseMp}]");

            Console.WriteLine();
            Console.WriteLine();

            List<Monster> livemonster = new List<Monster>();

            for (int i = 0; i < gm.monsters.Count; i++)
            {
                if (gm.monsters[i].Health > 0)
                {
                    livemonster.Add(gm.monsters[i]);
                    Console.WriteLine($"-[{i + 1}] Lv. {gm.monsters[i].Lv} {gm.monsters[i].Name} Hp : {gm.monsters[i].Health}");
                }
            }

            gm.player.skills[skillnum].AttackSkill(gm.monsters, gm.player, target);
            Console.WriteLine();
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine("           v");
            Console.WriteLine();

            for (int i = 0; i < livemonster.Count; i++)
            {
                if (livemonster[i].Health > 0)
                    Console.WriteLine($"-[{i + 1}] Lv. {livemonster[i].Lv} {livemonster[i].Name} Hp : {livemonster[i].Health}");
                else
                {
                    Utility.ColorText(ConsoleColor.DarkGray, $"-[{i + 1}] Lv. {livemonster[i].Lv} {livemonster[i].Name} [Dead]");
                    DefeatMonster(livemonster[i]);
                }
            }
            livemonster.Clear();

            Console.WriteLine();
            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar(); ;
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
            {
                if (gm.monsters.Count(m => m.Health > 0) != 0)
                    MonsterAttack();
                else
                    BattleResult();
            }
        }

        public void PlayerBasicAttack(int target)
        {
            ScreenText("Battle!! - Player의 턴");

            int[] monsternum = new int[gm.monsters.Count];
            for (int i = 0; i < gm.monsters.Count; i++)
            {
                monsternum[i] = gm.monsters[i].Health;
            }
            Console.WriteLine($"{gm.player.name} 의 공격!");

            int damage = (int)Math.Ceiling(random.NextDouble() * ((float)(gm.player.strikePower + gm.player.equipStrikePower) * 0.2) + (float)(gm.player.strikePower + gm.player.equipStrikePower) * 0.9);
            int critical = random.Next(15, 100);
            if (critical < 15)
            {
                damage = (int)((float)damage * 1.6);
            }
            int evasion = random.Next(0, 10);

            if (evasion > 0)
                gm.monsters[target].Health -= damage;

            if (evasion == 0)
                Console.WriteLine("몬스터가 공격을 회피했다!");
            else if (critical < 15)
                Console.WriteLine($"{gm.monsters[target].Name} 을(를) 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!");
            else
                Console.WriteLine($"{gm.monsters[target].Name} 을(를) 맞췄습니다. [데미지 : {damage}]");

            Console.WriteLine();

            for (int i = 0; i < gm.monsters.Count; i++)
            {
                if (gm.monsters[i].Health <= 0 && monsternum[i] > 0)
                {
                    Console.WriteLine($"Lv. {gm.monsters[i].Lv} {gm.monsters[i].Name}");
                    Console.WriteLine($"HP. {monsternum[i]} => Dead");
                    DefeatMonster(gm.monsters[target]);
                }
            }



            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            if (Utility.GetInput(0, 0) == 0)
            {
                int livemonsters = gm.monsters.Count(m => m.Health > 0);
                if (!(gm.player.healthPoint > 0 && livemonsters > 0))
                    BattleResult();
                else
                    MonsterAttack();
            }
        }

        public void MonsterAttack()
        {
            ScreenText("Battle!! - Monster의 턴");


            foreach (Monster monster in gm.monsters)
            {
                int playerHp = gm.player.healthPoint;
                if (monster.Health > 0)
                {
                    if (gm.player.healthPoint > 0)
                    {
                        Console.WriteLine($"{monster.Name} 의 공격!");
                        int damage = monster.Atk - gm.player.defensivePower;

                        int Critical = random.Next(0, 100);
                        if (Critical < 15)
                        {
                            damage = (int)((float)damage * 1.6);
                        }
                        damage = Math.Max(0, damage);

                        int evasion = random.Next(0, 10);
                        if (evasion > 0)
                            gm.player.healthPoint -= damage;

                        if (evasion == 0)
                        {
                            Console.WriteLine($"{monster.Name}의 공격을 {gm.player.name}은(는) 회피하였습니다.");
                            gm.player.DisplayHpBar();
                            Console.WriteLine();
                            Thread.Sleep(300);
                        }
                        else if (damage == 0)
                        {
                            Console.WriteLine($"{monster.Name}의 공격을 {gm.player.name}은(는) 높은 방어력으로 공격을 막아냈습니다.");
                            gm.player.DisplayHpBar();
                            Console.WriteLine();
                            Thread.Sleep(300);
                        }
                        else
                        {
                            if (gm.player.healthPoint > 0)
                            {
                                if (Critical < 15)
                                {
                                    Console.WriteLine($"{monster.Name}은 {gm.player.name}에게 공격을 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!");
                                    gm.player.DisplayHpBar();
                                    Console.WriteLine();
                                    Thread.Sleep(300);
                                }
                                else
                                {
                                    Console.WriteLine($"{monster.Name}은 {gm.player.name}에게 공격을 맞췄습니다. [데미지 : {damage}]");
                                    gm.player.DisplayHpBar();
                                    Console.WriteLine();
                                    Thread.Sleep(300);
                                }
                            }
                            else
                            {
                                gm.player.healthPoint = 0;
                                if (Critical < 15)
                                {
                                    Console.WriteLine($"{monster.Name}은 {gm.player.name}에게 공격을 맞췄습니다. [데미지 : {damage}] - 치명타 공격!!\n[Hp {playerHp} => [Dead]");
                                    gm.player.DisplayHpBar();
                                    Console.WriteLine();
                                    Thread.Sleep(300);
                                }
                                else
                                {
                                    Console.WriteLine($"{monster.Name}은 {gm.player.name}에게 공격을 맞췄습니다. [데미지 : {damage}][Hp {playerHp} => [Dead]");
                                    gm.player.DisplayHpBar();
                                    Console.WriteLine();
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
                int livemonsters = gm.monsters.Count(m => m.Health > 0);
                if (!(gm.player.healthPoint > 0 && livemonsters > 0))
                    BattleResult();
                else
                    ReadyBattle();
            }


        }

        public void DefeatMonster(Monster monster)
        {

            Console.WriteLine($"'{monster.Name}'을 처치했습니다!");
      


            //Console.WriteLine($"현재 진행 중인 퀘스트 개수: {questManager.acceptedQuests.Count}");

            foreach (int questId in questManager.acceptedQuests)
            {
                questManager.UpdateQuestProgress(questId, 1, player);
                Console.WriteLine($"퀘스트 진행도 업데이트됨: {questId}");
            }
        }

        public void BattleResult()
        {
            ScreenText("Battle - Result");

            if (gm.player.healthPoint > 0)
            {
                int goldSum = 0;
                int expSum = 0;
                float getAtk = 0;
                for (int i = 0; i < gm.monsters.Count; i++)
                {
                    if (gm.monsters[i].MonsterType == "보스")
                        getAtk += (float)gm.monsters[i].Atk / 5;
                    else if (gm.monsters[i].MonsterType == gm.player.chad)
                        getAtk += (float)gm.monsters[i].Atk / 10;
                    else
                        getAtk += (float)gm.monsters[i].Atk / 20;

                    gm.player.gold += gm.monsters[i].Rewards;
                    goldSum += gm.monsters[i].Rewards;
                    gm.player.exp += (gm.monsters[i].Exp + gm.player.dungeonLevel * 3);
                    expSum += (gm.monsters[i].Exp + gm.player.dungeonLevel * 3);
                }


                Utility.ColorText(ConsoleColor.Yellow, "Victory!");
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터 {gm.monsters.Count}마리를 잡았습니다.");

                if ((int)getAtk >= 1)
                {
                    gm.player.strikePower += (int)getAtk;
                    Console.WriteLine($"몬스터들의 흔적을 읽어 {(int)getAtk}만큼의 힘이 상승했습니다.");
                }

                Console.WriteLine($"획득 경험치 {expSum}");
                Console.WriteLine($"획득 골드 : {goldSum} G");
                if (gm.player.dungeonLevel % 3 == 0)
                {
                    List<EquipItem> bossdrop = gm.equipItemList.Where(x => x.IsBossItem == true).ToList();
                    int dropnum = random.Next(0, bossdrop.Count);
                    bossdrop[dropnum].ItemCount += 1;
                    Console.WriteLine($"{gm.monsters[0].Name}에게서 [{(EquipType)bossdrop[dropnum].Type}]{bossdrop[dropnum].Name}을 획득하였습니다.");
                    bossdrop.Clear();
                }
                foreach (Monster monster in gm.monsters)
                {
                    if (monster.Getitem == 0)
                    {
                        int dropnum = random.Next(0, gm.equipItemList.Count);
                        if(!gm.equipItemList[dropnum].IsBossItem)
                        {
                            gm.equipItemList[dropnum].ItemCount += 1;
                            Console.WriteLine($"Lv.{monster.Lv} {monster.Name}에게서 [{(EquipType)gm.equipItemList[dropnum].Type}]{gm.equipItemList[dropnum].Name}을 획득하였습니다.");
                        }
                    }
                }
                gm.player.ControlLevel();
                gm.player.dungeonLevel += 1;
            }
            else
                Utility.ColorText(ConsoleColor.DarkRed, "You Lose.");

            Console.WriteLine();
            Console.WriteLine($"Lv. {gm.player.level} {gm.player.name}");
            Console.WriteLine($"Hp. {EnterHp} => {gm.player.healthPoint}");
            Console.WriteLine($"Mp. {EnterMp} => {gm.player.manaPoint}");
            Console.WriteLine();

            gm.player.DisplayHpBar();
            Console.WriteLine();
            gm.player.DisplayMpBar();
            gm.monsters.Clear();

            Console.WriteLine();
            Console.WriteLine("0. 메뉴로");
            if (gm.player.healthPoint > 0)
            {
                Console.WriteLine("1. 다음층으로");
                Console.WriteLine();
                int input = Utility.GetInput(0, 1);
                if (input == 1)
                    Battle(gm);
            }
            else
            {
                Console.WriteLine();
                int input = Utility.GetInput(0, 0);
                return;
            }

        }



        public void MonsterSpawn(List<Monster> monsterList, int spawnNum)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                Monster monster = monsterList[random.Next(0, monsterList.Count)];
                Monster Addmonster = monster.Spawn();
                if (Addmonster.MonsterType != "이벤트")
                {
                    Addmonster.Atk += (int)(gm.player.dungeonLevel * (gm.player.dungeonLevel / 3));
                    Addmonster.Health += (int)(gm.player.dungeonLevel * (gm.player.dungeonLevel / 3) * 3);
                    Addmonster.MaxHealth += (int)(gm.player.dungeonLevel * (gm.player.dungeonLevel / 3) * 3);
                }
                else
                {
                    Addmonster.Atk += gm.player.dungeonLevel;
                    Addmonster.Health += gm.player.dungeonLevel;
                    Addmonster.MaxHealth += gm.player.dungeonLevel;
                }
                Addmonster.Lv = Addmonster.Atk / 10+1;
                gm.monsters.Add(Addmonster);
            }
        }
        public void ScreenText(string tag)
        {
            Console.Clear();

            Console.WriteLine();
            Utility.ColorText(ConsoleColor.Yellow, tag);
            Console.WriteLine();
        }
        public void MonsterInfo()
        {
            for (int i = 0; i < gm.monsters.Count; i++)
            {
                if (gm.monsters[i].Health > 0)
                {
                    if (gm.monsters[i].MonsterType == "보스")
                        Utility.ColorText(ConsoleColor.White, $"-[{i + 1}] Lv. {gm.monsters[i].Lv} {gm.monsters[i].Name} \n     Hp : {gm.monsters[i].Health} Atk : {gm.monsters[i].Atk}");
                    else
                        Console.WriteLine($"-[{i + 1}] Lv. {gm.monsters[i].Lv} {gm.monsters[i].Name} \n     Hp : {gm.monsters[i].Health} Atk : {gm.monsters[i].Atk}");
                }
                else
                {
                    Utility.ColorText(ConsoleColor.DarkGray, $"-[{i + 1}] Lv. {gm.monsters[i].Lv} {gm.monsters[i].Name} [Dead]");
                }
            }
        }
        //public void Hpbar()
        //{
        //    int viewHp = (int)((float)gm.player.healthPoint / ((gm.player.maxhealthPoint + gm.player.equipMaxhealthPoint) / 10));
        //    viewHp = Math.Min(viewHp, 10);
        //    Console.WriteLine($"Hp. {gm.player.healthPoint} / {gm.player.maxhealthPoint + gm.player.equipMaxhealthPoint}");
        //    for (int i = 0; i < viewHp; i++)
        //    {
        //        Utility.ColorText(ConsoleColor.Red, "■", Text.Write);
        //    }
        //    for (int i = 0; i < 10 - viewHp; i++)
        //    {
        //        Console.Write("□");
        //    }

        //}
        //public void Mpbar()
        //{
        //    Console.WriteLine();
        //    int viewMp = (int)((float)gm.player.manaPoint / (gm.player.maxManaPoint / 10));
        //    viewMp = Math.Min(viewMp, 10);
        //    Console.WriteLine($"Mp. {gm.player.manaPoint} / {gm.player.maxManaPoint}");
        //    for (int i = 0; i < viewMp; i++)
        //    {
        //        Utility.ColorText(ConsoleColor.Blue, "■", Text.Write);
        //    }
        //    for (int i = 0; i < 10 - viewMp; i++)
        //    {
        //        Console.Write("□");
        //    }
        //    Console.WriteLine();
        //}
    }
}
