#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;

namespace CeuxQuiRestent.Audio
{
    public static class WwiseEventDrawer
    {
        public static AK.Wwise.Event currentlyEditedEvent;
        
        public static AK.Wwise.Event WwiseEventField(AK.Wwise.Event _event, Rect pickerPosition)
        {
            // Initialization
            if (_event == null)
            {
                _event = new AK.Wwise.Event();
                _event.ID = 0;
                _event.valueGuid = new byte[16];
            }
            AkWwiseProjectData.WwiseObjectType objectType = AkWwiseProjectData.WwiseObjectType.EVENT;
            string typeName = "Event";

            // Process
            Guid[] componentGuid = new Guid[1];
            for (int i = 0; i < componentGuid.Length; i++)
            {
                byte[] guidBytes = GetByteArray(_event.valueGuid);
                componentGuid[i] = guidBytes == null ? Guid.Empty : new Guid(guidBytes);
            }
            string componentName = UpdateIds(ref _event.ID, componentGuid);

            // Selector button style
            GUIStyle selectorButtonStyle = new GUIStyle(GUI.skin.button);
            selectorButtonStyle.alignment = TextAnchor.MiddleLeft;
            selectorButtonStyle.fontStyle = FontStyle.Normal;

            // If we didn't find the event
            if (componentName.Equals(String.Empty))
            {
                componentName = "No " + typeName + " is currently selected";
                selectorButtonStyle.normal.textColor = Color.red;
            }

            // Selector button
            if (GUILayout.Button(componentName, selectorButtonStyle, GUILayout.MaxWidth(300)))
            {
                currentlyEditedEvent = _event;
                WwisePicker.Create(objectType, pickerPosition);
                _event = currentlyEditedEvent;
            }

            // End
            return _event;
        }

        public static AK.Wwise.Event WwiseEventField(ref string eventName, AK.Wwise.Event _event, Rect pickerPosition)
        {
            // Initialization
            if (_event == null)
            {
                _event = new AK.Wwise.Event();
                _event.ID = 0;
                _event.valueGuid = new byte[16];
            }
            AkWwiseProjectData.WwiseObjectType objectType = AkWwiseProjectData.WwiseObjectType.EVENT;
            string typeName = "Event";

            // Process
            Guid[] componentGuid = new Guid[1];
            for (int i = 0; i < componentGuid.Length; i++)
            {
                byte[] guidBytes = GetByteArray(_event.valueGuid);
                componentGuid[i] = guidBytes == null ? Guid.Empty : new Guid(guidBytes);
            }
            string componentName = UpdateIds(ref _event.ID, componentGuid);
            eventName = componentName;

            // Selector button style
            GUIStyle selectorButtonStyle = new GUIStyle(GUI.skin.button);
            selectorButtonStyle.alignment = TextAnchor.MiddleLeft;
            selectorButtonStyle.fontStyle = FontStyle.Normal;

            // If we didn't find the event
            if (componentName.Equals(String.Empty))
            {
                componentName = "No " + typeName + " is currently selected";
                selectorButtonStyle.normal.textColor = Color.red;
            }

            // Selector button
            if (GUILayout.Button(componentName, selectorButtonStyle, GUILayout.MaxWidth(300)))
            {
                currentlyEditedEvent = _event;
                WwisePicker.Create(objectType, pickerPosition);
                _event = currentlyEditedEvent;
            }

            // End
            return _event;
        }

        public static byte[] GetByteArray(byte[] eventIDs)
        {
            if (eventIDs.Length == 0)
                return null;

            byte[] byteArray = new byte[eventIDs.Length];

            for (int i = 0; i < byteArray.Length; i++)
                byteArray[i] = (byte)eventIDs[i];

            return byteArray;
        }

        public static void SetByteArray(byte[] byteArray)
        {
            for (int i = 0; i < currentlyEditedEvent.valueGuid.Length; i++)
                currentlyEditedEvent.valueGuid[i] = byteArray[i];
        }

        public static string UpdateIds(ref int _id, Guid[] _guid)
        {
            var list = AkWwiseProjectInfo.GetData().EventWwu;

            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i].List.Find(x => new Guid(x.Guid).Equals(_guid[0]));

                if (element != null)
                {
                    _id = element.ID;
                    return element.Name;
                }
            }

            _id = 0;
            return string.Empty;
        }

        public static string GetEventName(AK.Wwise.Event _event)
        {
            Guid[] componentGuid = new Guid[1];
            for (int i = 0; i < componentGuid.Length; i++)
            {
                byte[] guidBytes = GetByteArray(_event.valueGuid);
                componentGuid[i] = guidBytes == null ? Guid.Empty : new Guid(guidBytes);
            }
            string componentName = UpdateIds(ref _event.ID, componentGuid);
            return componentName;
        }
    }
}
#endif
