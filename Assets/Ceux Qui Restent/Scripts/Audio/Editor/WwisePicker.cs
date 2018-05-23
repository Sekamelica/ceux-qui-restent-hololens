#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using AK.Wwise.TreeView;

namespace CeuxQuiRestent.Audio
{
    public class WwisePicker : EditorWindow
    {
        static WwisePicker picker = null;

        AkWwiseTreeView m_treeView = new AkWwiseTreeView();
        AkWwiseProjectData.WwiseObjectType wwiseType;
        bool m_close = false;

        static public void Create(AkWwiseProjectData.WwiseObjectType _wwiseType, Rect in_pos)
        {
            if (picker == null)
            {
                picker = ScriptableObject.CreateInstance<WwisePicker>();

                //position the window below the button
                Rect pos = new Rect(in_pos.x, in_pos.yMax, 0, 0);

                //If the window gets out of the screen, we place it on top of the button instead
                if (in_pos.yMax > (Screen.currentResolution.height / 2))
                {
                    pos.y = in_pos.y - (Screen.currentResolution.height / 2);
                }

                //We show a drop down window which is automatically destroyed when focus is lost
                picker.ShowAsDropDown(pos, new Vector2(in_pos.width >= 250 ? in_pos.width : 250, Screen.currentResolution.height / 2));
                
                picker.wwiseType = _wwiseType;

                //Make a backup of the tree's expansion status and replace it with an empty list to make sure nothing will get expanded
                //when we populate the tree 
                List<string> expandedItemsBackUp = AkWwiseProjectInfo.GetData().ExpandedItems;
                AkWwiseProjectInfo.GetData().ExpandedItems = new List<string>();

                picker.m_treeView.AssignDefaults();
                picker.m_treeView.SetRootItem(System.IO.Path.GetFileNameWithoutExtension(WwiseSetupWizard.Settings.WwiseProjectPath), AkWwiseProjectData.WwiseObjectType.PROJECT);

                //Populate the tree with the correct type 
                if (_wwiseType == AkWwiseProjectData.WwiseObjectType.EVENT)
                {
                picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Events", AkWwiseProjectInfo.GetData().EventWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.SWITCH)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Switches", AkWwiseProjectInfo.GetData().SwitchWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.STATE)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "States", AkWwiseProjectInfo.GetData().StateWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.SOUNDBANK)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Banks", AkWwiseProjectInfo.GetData().BankWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.AUXBUS)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Auxiliary Busses", AkWwiseProjectInfo.GetData().AuxBusWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.GAMEPARAMETER)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Game Parameters", AkWwiseProjectInfo.GetData().RtpcWwu);
                }
                else if (_wwiseType == AkWwiseProjectData.WwiseObjectType.TRIGGER)
                {
                    picker.m_treeView.PopulateItem(picker.m_treeView.RootItem, "Triggers", AkWwiseProjectInfo.GetData().TriggerWwu);
                }

                TreeViewItem item = null;

                byte[] byteArray = WwiseEventDrawer.GetByteArray(WwiseEventDrawer.currentlyEditedEvent.valueGuid);
                if (byteArray != null)
                    item = picker.m_treeView.GetItemByGuid(new Guid(byteArray));

                if (item != null)
                {
                    item.ParentControl.SelectedItem = item;

                    int itemIndexFromRoot = 0;

                    //Expand all the parents of the selected item.
                    //Count the number of items that are displayed before the selected item
                    while (true)
                    {
                        item.IsExpanded = true;

                        if (item.Parent != null)
                        {
                            itemIndexFromRoot += item.Parent.Items.IndexOf(item) + 1;
                            item = item.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }

                    //Scroll down the window to make sure that the selected item is always visible when the window opens
                    float itemHeight = item.ParentControl.m_skinSelected.button.CalcSize(new GUIContent(item.Header)).y + 2.0f; //there seems to be 1 pixel between each item so we add 2 pixels(top and bottom) 
                    picker.m_treeView.SetScrollViewPosition(new Vector2(0.0f, (itemHeight * itemIndexFromRoot) - (Screen.currentResolution.height / 4)));
                }

                //Restore the tree's expansion status
                AkWwiseProjectInfo.GetData().ExpandedItems = expandedItemsBackUp;
            }
        }

        public void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                m_treeView.DisplayTreeView(TreeViewControl.DisplayTypes.USE_SCROLL_VIEW);

                EditorGUILayout.BeginHorizontal("Box");
                {
                    if (GUILayout.Button("Ok"))
                    {
                        //Get the selected item
                        TreeViewItem selectedItem = m_treeView.GetSelectedItem();

                        //Check if the selected item has the correct type
                        if (selectedItem != null && wwiseType == (selectedItem.DataContext as AkWwiseTreeView.AkTreeInfo).ObjectType)
                        {
                            SetGuid(selectedItem);
                        }

                        //The window can now be closed
                        m_close = true;
                    }
                    else if (GUILayout.Button("Cancel"))
                    {
                        m_close = true;
                    }
                    else if (GUILayout.Button("Reset"))
                    {
                        ResetGuid();
                        m_close = true;
                    }
                    //We must be in 'used' mode in order for this to work
                    else if (Event.current.type == EventType.used && m_treeView.LastDoubleClickedItem != null && wwiseType == (m_treeView.LastDoubleClickedItem.DataContext as AkWwiseTreeView.AkTreeInfo).ObjectType)
                    {
                        SetGuid(m_treeView.LastDoubleClickedItem);
                        m_close = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        void SetGuid(TreeViewItem in_item)
        {
            //we set the items guid
            WwiseEventDrawer.SetByteArray((in_item.DataContext as AkWwiseTreeView.AkTreeInfo).Guid);
        }

        void ResetGuid()
        {
            byte[] emptyArray = new byte[16];

            //we set the items guid
            WwiseEventDrawer.SetByteArray(emptyArray);
        }

        public void Update()
        {
            //Unity sometimes generates an error when the window is closed from the OnGUI function.
            //So We close it here
            if (m_close)
                Close();
        }
    }
#endif
}