using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{
    public class DialogManager : MonoBehaviour
    {
        public float maxDialogDistance ;
        public GameObject dialogUI;
        public Text dialogHeaderText;
        public Text dialogAnswerText;
        public GameObject dialogOptionList;
        public Button dialogOptionsPrefab;

       
        private PlayerInput m_Player;
        private QuestGiver m_Npc;
        private Dialog m_ActiveDialog;
        private float m_OptionTopPossition;
        const float c_DistanceBetweenOption = 32.0f;
        public bool HasActiveDialog { get { return m_ActiveDialog != null; } }
        private float DialogDistance { get { return Vector3.Distance(
                                       m_Player.transform.position,
                                       m_Npc.transform.position);
            }  }

        private void Start()
        {
            m_Player= PlayerInput.Instance;
        }
        private void Awake()
        {

            dialogUI.SetActive(false);
           
        }
        private void Update()
        {
            if(!HasActiveDialog &&
                m_Player != null &&
              m_Player.OptionClickTarget != null     
              )
            {
                if (m_Player.OptionClickTarget.CompareTag("QuestGiver"))
                {
                    m_Npc = m_Player.OptionClickTarget.GetComponent<QuestGiver>();
                   
                    if (DialogDistance < maxDialogDistance)
                    {
                       StartCoroutine(StartDialog());


                    }

                }

            }

            if (HasActiveDialog && DialogDistance > maxDialogDistance+1.0f)
            {
                StopDialog();
            }
        }

        private IEnumerator StartDialog()
        {
            m_ActiveDialog = m_Npc.dialog;
            dialogHeaderText.text = m_Npc.name;
            dialogUI.SetActive(true);
            ClearDialogOption();
            DisplayAnswerText(m_ActiveDialog.welcomeText);
           

            yield return new WaitForSeconds(2.0f);

            if (HasActiveDialog)
            {

                DisplayDialogOption();
            }
         
        }

        private void DisplayDialogOption()
        {
            HideAnswerText();
            CreateDiaLogMenu();
        }
        private void DisplayAnswerText(string answerText)
        {
            dialogAnswerText.gameObject.SetActive(true);
            dialogAnswerText.text = answerText;
        }
        private void HideAnswerText()
        {
            dialogAnswerText.gameObject.SetActive(false);
        }
        private void CreateDiaLogMenu()
        {
            m_OptionTopPossition = 0;
            var queries = Array.FindAll(m_ActiveDialog.queries, query => !query.isAsked);
            foreach(var query in queries)
            {
                m_OptionTopPossition += c_DistanceBetweenOption;
               var dialogOption =  CreateDialogOption(query.text);

            }

        }
        private Button CreateDialogOption(string optionText)
        {

            Button buttonInstance = Instantiate(dialogOptionsPrefab, dialogOptionList.transform);
           buttonInstance.GetComponentInChildren<Text>().text= optionText;
            
            RectTransform rt = buttonInstance.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(
                RectTransform.Edge.Top,m_OptionTopPossition,
                rt.rect.height);
            return buttonInstance;
        }

        private void StopDialog()
        {
            m_Npc = null;
            m_ActiveDialog = null;

            dialogUI.SetActive(false);
        }

        private void ClearDialogOption()
        {
            foreach(Transform child in dialogOptionList.transform)
            {
                Destroy(child.gameObject);
            }
        }




    }



}
