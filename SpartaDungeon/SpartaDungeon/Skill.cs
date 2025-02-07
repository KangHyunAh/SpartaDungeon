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
        public string Name { get; }
        public string Description { get; }
        public SkillType Type { get; }
        public float SkillPower { get; }
        public int Count { get; }
        public int UseHp { get; }
        public int UseMp { get; }


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

        public void AttackSkill(List<Monster> monsters, Player player, int index = 0)
        {
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
            else if (Count != 5 && Count != 1)
            {
                List<Monster> target = monsters.Where(i => i.Health > 0).OfType<Monster>().ToList();
                if (target.Count == 1)
                {
                    target[0].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                }
                else if (target.Count >= 2)
                {
                    int rndtargetIndex1 = new Random().Next(0, target.Count);

                    target[rndtargetIndex1].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);

                    int rndtargetIndex2 = new Random().Next(0, target.Count);

                    while (rndtargetIndex1 != rndtargetIndex2)
                    {
                        rndtargetIndex2 = new Random().Next(0, target.Count);
                    }

                    target[rndtargetIndex2].Health -= (int)((player.strikePower + player.equipStrikePower) * SkillPower);
                }
            }
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
                    player.skills.Add(new Skill("더블 스트라이크", "랜덤한 적 두명에게 공격력 * 1.3의 데미지를 준다.", SkillType.Attack, 1.3f, 1, 0, 10));
                    break;
                case "광전사":
                    player.skills.Add(new Skill("분노 분출", "적 전체에 공격력 * 1.0의 데미지를 준다.", SkillType.Attack, 2.0f, 1, 20, 0));
                    player.skills.Add(new Skill("자가 치유", "자신의 최대 HP(장비 스탯 제외)의 30% 를 회복한다.", SkillType.Heal, 0.3f, 1, 0, 20));
                    break;
                default:
                    break;
            }
        }
    }
}
