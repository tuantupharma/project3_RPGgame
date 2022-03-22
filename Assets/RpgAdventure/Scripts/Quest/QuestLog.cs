using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RpgAdventure
{
    public enum QuestStatus
    {
        ACTIVE,
        FAILED,
        COMPLETED
    }


    public class AcceptedQuest : Quest
    {
        public QuestStatus questStatus;
       
    }

    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedQuest> quests;



    }


}

