using SpartaDungeon.PotionNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public enum SkillType
{
    Attack,
    Heal,
    Deffence
}

namespace SpartaDungeon
{
    public class Skill
    {
        public string Name { get; } // 스킬 이름
        public string Description { get; } // 스킬 설명
        public SkillType Type { get; } // 스킬 타입
        public float SkillPower { get; } // 스킬 효과 (공격 수치 || 회복 수치)
        public int Count { get; } // 적중시킬 적 수
        public int UseHp { get; } // 사용할 HP
        public int UseMp { get; } // 사용할 MP

        private Random random = new Random();

        public Skill(string n, string d, SkillType t, float p, int c, int h, int m)
        {
            Name = n;
            Description = d;
            Type = t;
            SkillPower = p;
            Count = c;
            UseHp = h;
            UseMp = m;
        }
        public void HealSkill(Player player)
        {
            player.healthPoint += (int)(player.maxhealthPoint * SkillPower);
            player.healthPoint = Math.Min(player.healthPoint, player.maxhealthPoint);
            player.manaPoint -= UseMp;
        }
        // 단일공격일시 매개변수에 index 넣어주기
        public void AttackSkill(List<Monster> monsters, Player player, int index = 0)
        {
            // 적 전체 공격
            if (Count == 5)
            {
                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i].Health > 0)
                    {
                        monsters[i].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                    }
                }
            }

            // 일정 수 랜덤 공격
            else if (Count != 5 && Count != 1)
            {
                // HP가 0이 아닌 적 찾기
                List<Monster> target = monsters.Where(i => i.Health > 0).OfType<Monster>().ToList();

                // 남은 적이 한마리일 때
                if (target.Count == 1)
                {
                    target[0].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                }

                // 남은 적이 두마리 이상일 때
                else if( target.Count >= 2)
                {
                    
                    int rndtargetIndex1 = random.Next(0, target.Count);
                    int rndtargetIndex2;
                    // index2에 index1과 다른값이 나올때 까지 반복
                    do
                    {
                        rndtargetIndex2 = random.Next(0, target.Count);
                    } while (rndtargetIndex1 == rndtargetIndex2);
                    target[rndtargetIndex1].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                    target[rndtargetIndex2].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                }

                target.Clear();
            }

            // 단일 공격
            else
            {
                monsters[index].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
            }

            player.healthPoint -= UseHp;
            player.manaPoint -= UseMp;
        }
        
    }

    public class SkillManager()
    {
        public static void SkillInit(Player player)
        {
            switch (player.chad)
            {
                case "나이트":
                    player.skills.Add(new Skill("휩쓸기", "적 전체에 공격력 * 0.3의 데미지를 준다.", SkillType.Attack, 0.3f, 5, 0, 10));
                    player.skills.Add(new Skill("방패 치기", "적 한명에게 공격력 * 1.5의 데미지를 준다.", SkillType.Attack, 1.5f, 1, 0, 10));
                    break;
                case "검사":
                    player.skills.Add(new Skill("알파 스트라이크", "적 한명에게 공격력 * 2.0의 데미지를 준다.", SkillType.Attack, 2.0f, 1, 0, 10));
                    player.skills.Add(new Skill("더블 스트라이크", "랜덤한 적 두명에게 공격력 * 1.3의 데미지를 준다.", SkillType.Attack, 1.3f, 2, 0, 10));
                    break;
                case "광전사":
                    player.skills.Add(new Skill("분노 분출", "적 전체에 공격력 * 1.0의 데미지를 준다.", SkillType.Attack, 2.0f, 5, 20, 0));
                    player.skills.Add(new Skill("자가 치유", "자신의 최대 HP(장비 스탯 제외)의 30% 를 회복한다.", SkillType.Heal, 0.3f, 1, 0, 20));
                    break;
                default:
                    break;
            }
        }
    }
}
