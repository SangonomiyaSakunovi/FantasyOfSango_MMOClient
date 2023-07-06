using System.Collections.Generic;
using UnityEngine;

namespace MxM
{
    //============================================================================================
    /**
    *  @brief 
    *         
    *********************************************************************************************/
    [CreateAssetMenu(fileName = "MxMEventDefinition", menuName = "MxM/Utility/MxMEventDefinition", order = 0)]
    public class MxMEventDefinition : ScriptableObject
    {
        public int Id = 0;
        public string EventName = "";
        public EMxMEventType EventType = EMxMEventType.Standard;
        public int Priority = -1;
        public int ContactCountToMatch = 1;
        public int ContactCountToWarp = 1;
        public bool ExitWithMotion = true;
        public bool MatchPose = true;
        public bool MatchTrajectory = true;
        public bool MatchRequireTags = false;
        public EFavourTagMethod FavourTagMethod = EFavourTagMethod.Exclusive;
        public EPostEventTrajectoryMode PostEventTrajectoryMode = EPostEventTrajectoryMode.Maintain;

        public bool MatchTiming;
        public bool ExactTimeMatch;
        public float TimingWeight;
        public EEventWarpType TimingWarpType;

        public bool MatchPosition;
        public float PositionWeight;
        public EEventWarpType MotionWarpType;
        public bool WarpTimeScaling;
        public int ContactCountToTimeScale = 1;
        public float MinWarpTimeScale = 0.9f;
        public float MaxWarpTimeScale = 1.2f;

        public bool MatchRotation;
        public float RotationWeight;
        public EEventWarpType RotationWarpType;

        [SerializeField]
        private MxMAnimData m_targetAnimData = null;

        public List<EventContact> EventContacts { get; private set; }
        public EventContact[] EventContactsArray { get { return EventContacts.ToArray(); } }
        public float DesiredDelay { get; set; }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        private void OnEnable()
        {
            if (EventContacts == null)
                EventContacts = new List<EventContact>();

            ValidateEventId();
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void ClearContacts()
        {
            EventContacts.Clear();
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContact(EventContact a_contact)
        {
            EventContacts.Add(a_contact);
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContact(Vector3 a_position, float a_rotationY)
        {
            EventContact newContact = new EventContact(a_position, a_rotationY);
            EventContacts.Add(newContact);
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContact(Vector3 a_position, Quaternion a_rotation)
        {
            EventContact newContact = new EventContact(a_position, a_rotation.eulerAngles.y);
            EventContacts.Add(newContact);
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContact(Transform a_contactTransform)
        {
            EventContact newContact = new EventContact(a_contactTransform.position, a_contactTransform.rotation.eulerAngles.y);
            EventContacts.Add(newContact);
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContacts(Transform[] a_contactTransforms)
        {
            foreach(Transform _transform in a_contactTransforms)
            {
                EventContact newContact = new EventContact(_transform.position, _transform.rotation.eulerAngles.y);
                EventContacts.Add(newContact);
            }
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddEventContacts(List<Transform> a_contactTransforms)
        {
            foreach (Transform _transform in a_contactTransforms)
            {
                EventContact newContact = new EventContact(_transform.position, _transform.rotation.eulerAngles.y);
                EventContacts.Add(newContact);
            }
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void AddDummyContacts(int a_dummyContactCount)
        {
            for(int i = 0; i < a_dummyContactCount; ++i)
            {
                EventContacts.Add(new EventContact(Vector3.zero, 0f));
            }
        }

        //============================================================================================
        /**
        *  @brief 
        *         
        *********************************************************************************************/
        public void ValidateEventId()
        {
            if (m_targetAnimData == null)
                return;

            if(EventName == null || EventName == "")
            {
                if(Id < 0 || Id >= m_targetAnimData.EventNames.Length)
                {
                    Id = -1;
                    EventName = "";
                }
                else
                {
                    EventName = m_targetAnimData.EventNames[Id];
                }
            }
            else
            {
                bool found = false;
                for (int i = 0; i < m_targetAnimData.EventNames.Length; ++i)
                {
                    if (m_targetAnimData.EventNames[i] == EventName)
                    {
                        found = true;
                        Id = i;
                        break;
                    }
                }

                //If no id of that name is found reset the event to null
                if (!found)
                {
                    Id = -1;
                }
            }
        }

    }//End of class: Event Defenition
}//End of namespace: MxM
