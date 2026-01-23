using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QueuemationPackage
{
    public class QueuemationInspector : EditorWindow
    {
        private TreeView queuemationTree;
        private bool shouldRebuild;
        private List<TreeViewItemData<IQueuemationData>> treeItems;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Window/Queuemation Inspector")]
        public static void ShowWindow()
        {
            QueuemationInspector wnd = GetWindow<QueuemationInspector>();
            wnd.titleContent = new GUIContent("Queuemation Inspector");
        }

        public void CreateGUI()
        {
            shouldRebuild = true;
            treeItems = new List<TreeViewItemData<IQueuemationData>>();

            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);

            queuemationTree = new TreeView
            {
                style = { flexGrow = 1 },
                selectionType = SelectionType.None
            };

            rootVisualElement.Add(queuemationTree);
        }

        public void Rebuild()
        {
            treeItems.Clear();

            if (Queuemation.MonitoredQueuemation != null)
            {    
                TreeViewItemData<IQueuemationData> root = new TreeViewItemData<IQueuemationData>
                (
                    Queuemation.MonitoredQueuemation.ID,
                    Queuemation.MonitoredQueuemation,
                    GetChildItems(Queuemation.MonitoredQueuemation)
                );
                treeItems.Add(root);
            }

            queuemationTree.SetRootItems(treeItems);
            queuemationTree.makeItem = () => new Label();
            queuemationTree.bindItem = BindItems;
            queuemationTree.RefreshItems();
            queuemationTree.ExpandAll();
        }

        private List<TreeViewItemData<IQueuemationData>> GetChildItems(IQueuemationData queuemation)
        {
            IEnumerable<IQueuemationData> children = queuemation.Children;
            if (children == null) return null;

            List<TreeViewItemData<IQueuemationData>> items = new List<TreeViewItemData<IQueuemationData>>();
            foreach (IQueuemationData child in children)
            {
                items.Add(new TreeViewItemData<IQueuemationData>(child.ID, child, GetChildItems(child)));
            }

            return items;
        }

        private void BindItems(VisualElement visualElement, int index)
        {
            Label label = (Label)visualElement;
            IQueuemationData queuemation = queuemationTree.GetItemDataForIndex<IQueuemationData>(index);
            label.text = $"({queuemation.ID}) {queuemation.Name} - {GetStatusEmoji(queuemation.Status)}";
        }

        private string GetStatusEmoji(QueuemationStatus status)
        {
            switch (status)
            {
                case QueuemationStatus.NOT_STARTED:
                    return "⬜";
                case QueuemationStatus.IN_PROGRESS:
                    return "🟨";
                case QueuemationStatus.COMPLETED:
                    return "✅";
                case QueuemationStatus.PAUSED:
                    return "⏸️";
                case QueuemationStatus.CANCELED:
                    return "❌";
                default:
                    return "";
            }
        }

        private void SetRebuild(object sender, EventArgs e)
        {
            shouldRebuild = true;
        }

        public void Update()
        {
            if (shouldRebuild)
            {
                Rebuild();
                shouldRebuild = false;
            }
        }

        public void OnEnable()
        {
            Queuemation.QueueUpdates += SetRebuild;
        }

        public void OnDisable()
        {
            Queuemation.QueueUpdates -= SetRebuild;
        }
    }
}
