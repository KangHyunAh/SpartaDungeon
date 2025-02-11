using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Quest
    {

        public int Id {  get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GoalCount { get; }
        public int CurrentCount { get; set;}
        public bool IsCompleted {  get; set; }
        public bool IsAccepted { get; set; }
        public int RewardGold { get; }
        public int RewardExp { get; }


        public Quest(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
            IsCompleted = false;
        }


        public void CompleteQuest()
        {
            IsCompleted = true;
            Console.WriteLine($"퀘스트'{Title}' 완료!");
        }


        public void Reward(Player player)
        {
            if (IsCompleted)
            {
                player.gold += RewardGold;
                player.exp += RewardExp;
                Console.WriteLine($"보상: {RewardGold}골드,  {RewardExp} 경험치 지급!");
            }
            else
            {
                Console.WriteLine("퀘스트가 완료되지 않았습니다. 보상을 받을 수 없습니다");
            }
        }
    }
}
