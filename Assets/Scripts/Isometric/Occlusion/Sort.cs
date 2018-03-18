using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iso.Occlusion
{
    public class Sort
    {
        protected SortNode m_RootNode;

        public int m_CountCaculate = 0;

        public void Insert(SortNode node)
        {
            if (m_RootNode == null)
            {
                m_RootNode = node;
            }
            else
            {
                Compare2(m_RootNode, node, 0);
            }
        }

        void Compare2(SortNode current,SortNode target,int deep)
        {
            int side = CaculateSide(current.rect, target.rect);
            if (side > 0)
            {
                //current遮挡target
                //继续比较和子结点的关系
                CompareChildren(current, target, deep);
                if (target.parent == null)
                {
                    current.AddChild(target);
                }
            }
            else if (side < 0)
            {
                //target遮挡current，把current做为target的潜在子对象
                target.virtualChildren.Add(current);
            }
            else
            {
                //暂时没有遮挡关系,继续比较子结点。
                CompareChildren(current, target, deep);
            }
        }

        void CompareChildren(SortNode current,SortNode target,int deep)
        {
            for(int i = 0, l = current.children.Count; i < l; ++i)
            {
                Compare2(current.children[i], target, deep + 1);
            }
        }

        void Compare(SortNode current,SortNode target)
        {
            Stack<SortNode> checks = new Stack<SortNode>();
            int side;

            current.checkDeep = 0;
            checks.Push(current);

            while (checks.Count > 0)
            {
                current = checks.Pop();

                side = CaculateSide(current.rect, target.rect);
                if (side > 0)
                {
                    //current遮挡target
                    //继续比较和子结点的关系
                    for (int i =  current.children.Count-1; i>=0l; --i)
                    { 
                        current.children[i].checkDeep = current.checkDeep + 1;
                        checks.Push(current.children[i]);
                    }

                    if (target.parent == null)
                    {
                        current.AddChild(target);
                        target.checkDeep = current.checkDeep + 1;
                    }
                    else if(current.checkDeep>=target.checkDeep)
                    {
                        current.AddChild(target);
                        target.checkDeep = current.checkDeep + 1;
                    }
                }
                else if (side < 0)
                {
                    //target遮挡current，把current做为target的潜在子对象
                    target.virtualChildren.Add(current);
                }
                else
                {
                    //暂时没有遮挡关系,继续比较子结点。
                    for (int i =current.children.Count-1; i>=0; --i)
                    {
                        current.children[i].checkDeep = current.checkDeep + 1;
                        checks.Push(current.children[i]);
                    }
                }
            }

            //处理比target小的没有移过来的结点
            ParseVirtualChildren(target);
        }

        void ParseVirtualChildren(SortNode node)
        {
            SortNode virtualChild = null;
            Stack<SortNode> checks = new Stack<SortNode>();

            checks.Push(node);
            while (checks.Count > 0)
            {
                node = checks.Pop();

                //首先检查需子类，看看有没有要改变深度的。
                //大多数时候在被检查结点插入的地方的子结点会被新加到这个结点下。
                for (int i = node.virtualChildren.Count - 1; i >= 0; --i)
                {
                    virtualChild = node.virtualChildren[i];
                    if (node.checkDeep >= virtualChild.checkDeep)
                    {
                        node.AddChild(virtualChild);
                        node.virtualChildren.RemoveAt(i);
                        virtualChild.checkDeep = node.checkDeep + 1;
                        checks.Push(virtualChild);
                    }
                }

                for (int i = node.children.Count - 1; i >= 0; --i)
                {
                    checks.Push(node.children[i]);
                }
            }
        }

        /**
         * 计算二个矩形的位置关系
         * 这里使用的屏幕坐标。
         
            先看X方向
                |       |
                |       |
                |       |
             1  |   0   |   -1
                |       |
                |       |
                |       |
                
            再看y方向
                        
                   1
            -----------------
                   0       
            -----------------
                  -1
                  
            合起来就是
                |      |
             2  |  1   | 0
            ----|------|----
             1  |  0   | -1
            ----|------|----
             0  | -1   | -2
                |      |
                
 
         * 中间是A,其它8个位置表示B可能出现的位置。
         * 反回值表示A-B的值
         * 大于0,表示A大于B(A遮挡B);
         * 小于0,表示A小于B(B遮挡A);
         * 等于0,表示A等于B(相互不遮挡).
         * 注意屏幕坐标和opengl的坐标
 
         转换成opengl的坐标系，斜视角矩形的关系
            
 
                  \  2 /
                   \  /
                 1  \/  1
              \     /\    /
               \   /  \  /
                \ /  0 \/
             0   /\    /\   0
                /  \  /  \
               / -1 \/ -1 \
                    /\
                   /  \
                  / -2 \
         */
        int CaculateSide(Rect a,Rect b)
        {
            ++m_CountCaculate;

            int x, y = 0;

            //大于等于
            if (b.xMin >= a.xMax)
            {
                //右
                x = 1;
            }
            //小于等于
            else if (b.xMax <= a.xMin)
            {
                //左
                x = -1;
            }
            else//b.xMax<=a.xMax && b.xMin>=a.xMin(内中),b.xMax>=a.xMax && b.xMin<=a.xMin(外中) 都是中
            {
                //中
                x = 0;
            }

            //大于等于
            if (b.yMin >= a.yMax) 
            {
                //上
                y = 1;
            }
            // 小于等于
            else if (b.yMax<=a.yMin)
            {
                //下
                y = -1;
            }
            else//b.yMax<=a.yMax && b.yMin>=a.yMin(内中),b.yMax>=a.yMax && b.yMin<=a.yMin(外中) 都是中
            {
                //中
                y = 0;
            }

            return x+y;
        }

        public SortNode rootNode
        {
            get
            {
                return m_RootNode;
            }

            set
            {
                m_RootNode = value;
            }
        }
    }
}

