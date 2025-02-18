﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Quest
    {

        public int QuestId {  get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GoalCount { get; }
        public int CurrentProgress { get; set;}
        public bool IsCompleted {  get; set; }
        public bool IsAccepted { get; set; }
        public int RewardGold { get; }
        public int RewardExp { get; }
        public string Target { get; set; }

        public QuestStatus Status { get; set; }

        public Quest(int id, string title, string description, string target, int goalCount, int rewardGold, int rewardExp )

        {
            QuestId = id;
            Title = title;
            Description = description;
            IsCompleted = false;
            Status = QuestStatus.Availble;
            GoalCount = goalCount;
            CurrentProgress = 0;
            Target = target;

            RewardGold = rewardGold;
            RewardExp = rewardExp;
        }



        public void UpdateProgress(int amount)
        {
            
            if (IsCompleted) return;

            //Console.WriteLine($"[기존 진행도] {CurrentProgress}, 추가량: {amount}");
            CurrentProgress += amount;
            Console.WriteLine($"[진행 중] {Title}: {CurrentProgress} / {GoalCount} 처치");

            if (CurrentProgress >= GoalCount)
            {
                CurrentProgress = GoalCount;
                Status = QuestStatus.Completed;
                IsCompleted=true;
                Console.WriteLine($"[퀘스트 완료] {Title} 퀘스트의 목표를 달성했습니다");
            }
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
