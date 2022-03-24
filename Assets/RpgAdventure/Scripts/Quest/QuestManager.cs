using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RpgAdventure
{

    public class JsonHelper
    {

        private class Wrapper<T>
        {
            public T[] array;
        }
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{\"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson <Wrapper<T>>(newJson);
            return wrapper.array;
        }

    }

    public class QuestManager : MonoBehaviour, IMessageReceiver
    {
        public Quest[] quests;

        private void Awake()
        {
            LoadQuestsFromDB();
            AssignQuests();
        }

        private void LoadQuestsFromDB()
        {
            using(StreamReader reader = new StreamReader("Assets/RpgAdventure/DB/QuestDB.json"))
            {
                string json = reader.ReadToEnd();
               var loadedQuests = JsonHelper.GetJsonArray<Quest>(json);
                quests = new Quest[loadedQuests.Length];
                quests = loadedQuests;
            }




        }
        private void AssignQuests()
        {
            var questGivers = FindObjectsOfType<QuestGiver>();

            if (questGivers != null && questGivers.Length > 0)
            {
                foreach (var questGiver in questGivers)
                {
                    AssignQuestTo(questGiver);
                }

            }
            
        }

        private void AssignQuestTo(QuestGiver questGiver)
        {
            foreach(var quest in quests)
            {
                if(quest.questGiver == questGiver.GetComponent<UniqueId>().Uid)
                {
                    questGiver.quest = quest;
                }
            }
        }
        // focus  nhan tin enemy die, ten enemy, vu khi diet enemy
        public void OnReceiveMessage(MessageType type, Damageable sender, Damageable.DamageMessage msg)
        {
            if(type == MessageType.DEAD)
            {
                CheckQuestWhenEnemyDead(sender, msg);
            }
                    
        }

        private void CheckQuestWhenEnemyDead(Damageable sender, Damageable.DamageMessage msg)
        {
            Debug.Log("check Q obj ");
            Debug.Log(sender.name );
            Debug.Log( msg.damager);
        }

    }

}
