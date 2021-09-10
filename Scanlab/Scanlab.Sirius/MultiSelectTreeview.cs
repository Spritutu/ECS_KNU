
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 멀티 선택이 가능한 트리뷰 사용자 컨트롤
    /// original author : https://www.codeproject.com/Articles/20581/Multiselect-Treeview-Implementation
    /// revisied by labspiral@gmail.com
    /// </summary>
    public class MultiSelectTreeview : TreeView
    {
        private List<TreeNode> m_SelectedNodes;
        private TreeNode m_SelectedNode;

        public List<TreeNode> SelectedNodes
        {
            get => this.m_SelectedNodes;
            set
            {
                this.ClearSelectedNodes();
                if (value == null)
                    return;
                foreach (TreeNode node in value)
                    this.ToggleNode(node, true);
            }
        }

        public new TreeNode SelectedNode
        {
            get => this.m_SelectedNode;
            set
            {
                this.ClearSelectedNodes();
                if (value == null)
                    return;
                this.SelectNode(value);
            }
        }

        public MultiSelectTreeview()
        {
            this.m_SelectedNodes = new List<TreeNode>(10000);
            base.SelectedNode = (TreeNode)null;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            try
            {
                if (this.m_SelectedNode == null && this.TopNode != null)
                    this.ToggleNode(this.TopNode, true);
                base.OnGotFocus(e);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            try
            {
                base.SelectedNode = (TreeNode)null;
                TreeNode nodeAt = this.GetNodeAt(e.Location);
                if (nodeAt != null)
                {
                    System.Drawing.Rectangle bounds = nodeAt.Bounds;
                    int x = bounds.X;
                    bounds = nodeAt.Bounds;
                    int num = bounds.Right + 10;
                    if (e.Location.X > x && (e.Location.X < num && (Control.ModifierKeys != Keys.None || !this.m_SelectedNodes.Contains(nodeAt))))
                        this.SelectNode(nodeAt);
                }
                base.OnMouseDown(e);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            try
            {
                TreeNode nodeAt = this.GetNodeAt(e.Location);
                if (nodeAt != null && Control.ModifierKeys == Keys.None && this.m_SelectedNodes.Contains(nodeAt))
                {
                    System.Drawing.Rectangle bounds = nodeAt.Bounds;
                    int x = bounds.X;
                    bounds = nodeAt.Bounds;
                    int num = bounds.Right + 10;
                    if (e.Location.X > x && e.Location.X < num)
                        this.SelectNode(nodeAt);
                }
                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            try
            {
                if (e.Item is TreeNode node2 && !this.m_SelectedNodes.Contains(node2))
                {
                    this.SelectSingleNode(node2);
                    this.ToggleNode(node2, true);
                }
                base.OnItemDrag(e);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            try
            {
                base.SelectedNode = (TreeNode)null;
                e.Cancel = true;
                base.OnBeforeSelect(e);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            try
            {
                base.OnAfterSelect(e);
                base.SelectedNode = (TreeNode)null;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.ShiftKey)
                return;
            bool flag = Control.ModifierKeys == Keys.Shift;
            try
            {
                if (this.m_SelectedNode == null && this.TopNode != null)
                    this.ToggleNode(this.TopNode, true);
                if (this.m_SelectedNode == null)
                    return;
                if (e.KeyCode == Keys.Left)
                {
                    if (this.m_SelectedNode.IsExpanded && this.m_SelectedNode.Nodes.Count > 0)
                    {
                        this.m_SelectedNode.Collapse();
                    }
                    else
                    {
                        if (this.m_SelectedNode.Parent == null)
                            return;
                        this.SelectSingleNode(this.m_SelectedNode.Parent);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (!this.m_SelectedNode.IsExpanded)
                        this.m_SelectedNode.Expand();
                    else
                        this.SelectSingleNode(this.m_SelectedNode.FirstNode);
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (this.m_SelectedNode.PrevVisibleNode == null)
                        return;
                    this.SelectNode(this.m_SelectedNode.PrevVisibleNode);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.m_SelectedNode.NextVisibleNode == null)
                        return;
                    this.SelectNode(this.m_SelectedNode.NextVisibleNode);
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (flag)
                    {
                        if (this.m_SelectedNode.Parent == null)
                        {
                            if (this.Nodes.Count <= 0)
                                return;
                            this.SelectNode(this.Nodes[0]);
                        }
                        else
                            this.SelectNode(this.m_SelectedNode.Parent.FirstNode);
                    }
                    else
                    {
                        if (this.Nodes.Count <= 0)
                            return;
                        this.SelectSingleNode(this.Nodes[0]);
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    if (flag)
                    {
                        if (this.m_SelectedNode.Parent == null)
                        {
                            if (this.Nodes.Count <= 0)
                                return;
                            this.SelectNode(this.Nodes[this.Nodes.Count - 1]);
                        }
                        else
                            this.SelectNode(this.m_SelectedNode.Parent.LastNode);
                    }
                    else
                    {
                        if (this.Nodes.Count <= 0)
                            return;
                        TreeNode lastNode = this.Nodes[0].LastNode;
                        while (lastNode.IsExpanded && lastNode.LastNode != null)
                            lastNode = lastNode.LastNode;
                        this.SelectSingleNode(lastNode);
                    }
                }
                else if (e.KeyCode == Keys.Prior)
                {
                    int visibleCount = this.VisibleCount;
                    TreeNode node;
                    for (node = this.m_SelectedNode; visibleCount > 0 && node.PrevVisibleNode != null; --visibleCount)
                        node = node.PrevVisibleNode;
                    this.SelectSingleNode(node);
                }
                else if (e.KeyCode == Keys.Next)
                {
                    int visibleCount = this.VisibleCount;
                    TreeNode node;
                    for (node = this.m_SelectedNode; visibleCount > 0 && node.NextVisibleNode != null; --visibleCount)
                        node = node.NextVisibleNode;
                    this.SelectSingleNode(node);
                }
                else
                {
                    string str = ((char)e.KeyValue).ToString();
                    TreeNode node = this.m_SelectedNode;
                    while (node.NextVisibleNode != null)
                    {
                        node = node.NextVisibleNode;
                        if (node.Text.StartsWith(str))
                        {
                            this.SelectSingleNode(node);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void SelectNode(TreeNode node)
        {
            try
            {
                this.BeginUpdate();
                if (this.m_SelectedNode != null)
                {
                    switch (Control.ModifierKeys)
                    {
                        case Keys.Shift:
                            TreeNode node1 = this.m_SelectedNode;
                            TreeNode treeNode1 = node;
                            if (node1.Parent == treeNode1.Parent)
                            {
                                if (node1.Index < treeNode1.Index)
                                {
                                    while (node1 != treeNode1)
                                    {
                                        node1 = node1.NextVisibleNode;
                                        if (node1 != null)
                                            this.ToggleNode(node1, true);
                                        else
                                            break;
                                    }
                                    goto label_35;
                                }
                                else if (node1.Index != treeNode1.Index)
                                {
                                    while (node1 != treeNode1)
                                    {
                                        node1 = node1.PrevVisibleNode;
                                        if (node1 != null)
                                            this.ToggleNode(node1, true);
                                        else
                                            break;
                                    }
                                    goto label_35;
                                }
                                else
                                    goto label_35;
                            }
                            else
                            {
                                TreeNode treeNode2 = node1;
                                TreeNode treeNode3 = treeNode1;
                                int num = Math.Min(treeNode2.Level, treeNode3.Level);
                                while (treeNode2.Level > num)
                                    treeNode2 = treeNode2.Parent;
                                while (treeNode3.Level > num)
                                    treeNode3 = treeNode3.Parent;
                                for (; treeNode2.Parent != treeNode3.Parent; treeNode3 = treeNode3.Parent)
                                    treeNode2 = treeNode2.Parent;
                                if (treeNode2.Index < treeNode3.Index)
                                {
                                    while (node1 != treeNode1)
                                    {
                                        node1 = node1.NextVisibleNode;
                                        if (node1 != null)
                                            this.ToggleNode(node1, true);
                                        else
                                            break;
                                    }
                                    goto label_35;
                                }
                                else if (treeNode2.Index == treeNode3.Index)
                                {
                                    if (node1.Level < treeNode1.Level)
                                    {
                                        while (node1 != treeNode1)
                                        {
                                            node1 = node1.NextVisibleNode;
                                            if (node1 != null)
                                                this.ToggleNode(node1, true);
                                            else
                                                break;
                                        }
                                        goto label_35;
                                    }
                                    else
                                    {
                                        while (node1 != treeNode1)
                                        {
                                            node1 = node1.PrevVisibleNode;
                                            if (node1 != null)
                                                this.ToggleNode(node1, true);
                                            else
                                                break;
                                        }
                                        goto label_35;
                                    }
                                }
                                else
                                {
                                    while (node1 != treeNode1)
                                    {
                                        node1 = node1.PrevVisibleNode;
                                        if (node1 != null)
                                            this.ToggleNode(node1, true);
                                        else
                                            break;
                                    }
                                    goto label_35;
                                }
                            }
                        case Keys.Control:
                            break;
                        default:
                            this.SelectSingleNode(node);
                            goto label_35;
                    }
                }
                bool flag = this.m_SelectedNodes.Contains(node);
                this.ToggleNode(node, !flag);
            label_35:
                this.OnAfterSelect(new TreeViewEventArgs(this.m_SelectedNode));
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void ClearSelectedNodes()
        {
            try
            {
                foreach (TreeNode selectedNode in this.m_SelectedNodes)
                {
                    selectedNode.BackColor = this.BackColor;
                    selectedNode.ForeColor = this.ForeColor;
                }
            }
            finally
            {
                this.m_SelectedNodes.Clear();
                this.m_SelectedNode = (TreeNode)null;
            }
        }

        private void SelectSingleNode(TreeNode node)
        {
            if (node == null)
                return;
            this.ClearSelectedNodes();
            this.ToggleNode(node, true);
            node.EnsureVisible();
        }

        private void ToggleNode(TreeNode node, bool bSelectNode)
        {
            if (bSelectNode)
            {
                this.m_SelectedNode = node;
                if (!this.m_SelectedNodes.Contains(node))
                    this.m_SelectedNodes.Add(node);
                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                this.m_SelectedNodes.Remove(node);
                node.BackColor = this.BackColor;
                node.ForeColor = this.ForeColor;
            }
        }

        private void HandleException(Exception ex)
        {
            int num = (int)MessageBox.Show(ex.Message);
        }

        public void MoveUp(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView treeView = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index <= 0)
                    return;
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index - 1, node);
            }
            else
            {
                if (!node.TreeView.Nodes.Contains(node))
                    return;
                int index = treeView.Nodes.IndexOf(node);
                if (index <= 0)
                    return;
                treeView.Nodes.RemoveAt(index);
                treeView.Nodes.Insert(index - 1, node);
            }
        }

        public static void MoveDown(TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView treeView = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index >= parent.Nodes.Count - 1)
                    return;
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index + 1, node);
            }
            else
            {
                if (treeView == null || !treeView.Nodes.Contains(node))
                    return;
                int index = treeView.Nodes.IndexOf(node);
                if (index >= treeView.Nodes.Count - 1)
                    return;
                treeView.Nodes.RemoveAt(index);
                treeView.Nodes.Insert(index + 1, node);
            }
        }
    }
}
