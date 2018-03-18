using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iso.Occlusion
{
    public class SortNode
    {
        //保存所有比自己小的元素。children之间是相等关系。
        protected List<SortNode> m_Children=new List<SortNode>();

        //父结点
        protected SortNode m_Parent=null;
        
        //范围
        protected Rect m_Rect;

        //保存可能会作为子元素的元素。元素所在的路径比移过来要小,暂时不要移过来，但保留记录。
        //如果结点的deep发生变化，则要再做检查。
        protected List<SortNode> m_VirtualChildren = new List<SortNode>();

        protected int m_CheckDeep;

        public void AddChild(SortNode child)
        {
            m_Children.Add(child);
            child.parent = this;
        }

        public void RemoeChild(SortNode child)
        {
            m_Children.Remove(child);
        }

        public int GetDeep()
        {
            int deep = 0;
            SortNode parent = m_Parent;
            while (parent != null)
            {
                ++deep;
                parent = parent.parent;
            }
            return deep;
        }

        public List<SortNode> children
        {
            get
            {
                return m_Children;
            }

            set
            {
                m_Children = value;
            }
        }

        public SortNode parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                if (m_Parent != null)
                {
                    m_Parent.RemoeChild(this);
                }
                m_Parent = value;
            }
        }

        public Rect rect
        {
            get
            {
                return m_Rect;
            }
            set
            {
                m_Rect = value;
            }
        }

        public List<SortNode> virtualChildren
        {
            get
            {
                return m_VirtualChildren;
            }

            set
            {
                m_VirtualChildren = value;
            }
        }

        public int checkDeep
        {
            get
            {
                return m_CheckDeep;
            }

            set
            {
                m_CheckDeep = value;
            }
        }

    }
}

