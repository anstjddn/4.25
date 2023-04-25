using System.Numerics;

namespace _4._25
{
    internal class BinarySearchTree<T> where T : IComparable<T>                     //이진트리에서 비교가능한값을 넣어야 하므로 IComparable 넣는다.
    {
        private Node root;                                  // root 노드 설정
        public BinarySearchTree()
        {
            this.root = null;                           // 초기노드에는 아무것도 없어야하므로 null을 넣는다.
        }

        public class Node                                   //이진트리 안에 노드를 이용해서 비교할꺼므로   
        {
            internal T item;                                // 안에 넣을값과 왼쪽,오른쪽,부모를 가르키는 참조값만들기
            internal Node left;
            internal Node right;
            internal Node parent;
            public Node(T item, Node left, Node right, Node parent)
            {
                this.item = item;
                this.left = left;
                this.right = right;
                this.parent = parent;

            }
            public bool IsRootNode { get { return parent == null; } }
            public bool IsRightNode { get { return parent != null && parent.right == this; } }
            public bool IsLeftNode { get { return parent != null && parent.left == this; } }
            public bool HasNoChildNode { get { return left == null && right == null; } }
            public bool HasLeftChildNode { get { return left != null && right == null; } }
            public bool HasRightChildNode { get { return left == null && right != null; } }
            public bool HasBothChildNode { get { return left != null && right != null; } }

        }
        public void Add(T Value)                                // 더하는거 설정
        {
            Node newnode = new Node(Value, null, null, null);           // 새로운 노드를 만들어 값만설정
            //일단 아무것도 없을떄
            if (root == null)                                        // 만약 맨위에 노드에 아무것도 없으면
            {
                root = newnode;                                     // 기존의 null이었던 노드에 새로만든 노드를 넣는다
                return;
            }
            Node current = root;
            //기존의 노드가있을떄
            while (current != null)
            {
                if (Value.CompareTo(current.item) < 0)          // 새로넣을 newnode의 값이 기존의root의 item값과 비교하여
                                                                // 작으면 왼쪽으로 들어간다.
                {

                    if (current.left != null)                  // 그런데 currnetnode의 왼쪽이 null이 아니라면
                        current = current.left;                  // current자식의 왼쪽값을 currnet값으로 변경하고
                                                                 // 다시 newnode의 value값과 비교하고
                    else                                        //currnet의 노드의 왼쪽에 값이 없으면
                    {
                        current.left = newnode;             // currnet왼쪽을 newnode로 설정하고
                        newnode.parent = current;           // newnode의 부모를 currnet로 설정한다.
                        return;
                    }
                }
                else if (Value.CompareTo(current.item) > 0)          // 새로넣을 newnode의 값이 기존의root의 item값과 비교하여
                                                                     // 그러면 오른쪽으로 넣는다.
                {
                    if (current.right != null)                  // 그런데 currnetnode의 오른쪽이 null이 아니라면
                        current = current.right;                  // current자식의 오른값을 currnet값으로 변경하고
                                                                  // 다시 newnode의 value값과 비교하고
                    else
                    {
                        current.right = newnode;             // currnet오른쪽을 newnode로 설정하고
                        newnode.parent = current;           // newnode의 부모를 currnet로 설정한다.
                        return;
                    }
                }
                else return;            // 중복은 그냥 리턴
            }
        }
        private void EraseNode(Node node)               //노드삭제할때
        {
            //일단 자식의 노드가 아무것도 없을때
            if (node.HasNoChildNode)
            {
                if (node.IsLeftNode)                //만약 노드가 아무것도 없는데 지울려고하는노드가 부모의 왼쪽에 있는거라면
                    node.parent.left = null;         // null을 부모왼쪽노드에 넣고
                else if (node.IsRightNode)          //만약 노드가 아무것도 없는데 지울려고하는노드가 부모의 오른쪽에 있는거라면
                    node.parent.right = null;        // null을 부모오른쪽 노드에 넣는다.
                else if (node.IsRootNode)            // 만약 부모노드가 null이라면(node가 root일때)
                    root = null;                    // null을 root에 얺는다.
                return;
            }
            // 자식의 노드가 1개있을때            자식과 부모노드연결필요
            else if (node.HasLeftChildNode || node.HasRightChildNode)
            {
                Node parent = node.parent;                  //일단 부모노드 설정하고
                //child노드를 설정하는데 아까 자식있는지 없는지 설정한 bool을 이용하여 트루면 자식이 왼쪽에 붙어있는거고 거짓이면 오른쪽에 붙어있는거므로
                Node Child = node.HasLeftChildNode ? node.left : node.right;      //위치확인


                if (node.IsLeftNode)            //삭제할려는 노드가 부모의 왼쪽노드라면
                {
                    Child.parent = parent;              // 부모를 자식부모노드로 연결하고
                    parent.left = Child;                // 자식을 부모 왼쪽노드로 연결한다.
                }

                else if (node.IsRightNode)             ///삭제할려는 노드가 부모의 오른쪽노드라면
                {
                    Child.parent = parent;              // 부모를 자식부모노드로 연결하고
                    parent.right = Child;               // 자식을 부모 오른쪽노드로 연결한다.
                }

                else                                   // 지금삭제할려고하는 노드가 root인경우
                {
                    Child = root;                               // root를 자식으로 설정하고
                    Child.parent = null;                        // null을 자식부모로 설정하낟.
                }
            }
            //자식의 노드가 2개이면 2개의 노드를 비교후 더 작은 값을 삭제한 node위치에 둔다
            else
            {           // 일단 삭제할 노드의 왼쪽을 새로운 노드로 설정하고
                Node replaceNode = node.left;               // 더작은값이 왼쪽이므로
                while (replaceNode.right != null)            // 삭제할 노드의 왼쪽값이 right값이 있으면
                {
                    replaceNode = replaceNode.right;        // 삭제할 노드의 왼쪽값중 오른쪽같을 삭제할 노드의 자식의 왼쪽값으로 이동
                }
                node.item = replaceNode.item; // 삭제할 노드의 자식노드의 값을 삭제할 노드의 item을 덮어씌운다.
                EraseNode(replaceNode); //              기존에 아래에있던 노드 삭제
            }


        }

        private Node FindNode(T Value)
        {
            Node current = root;                        //맨 위에 root부터 비교하면서 할예정이므로

            if (root == null)                            // root가 null이면 null 출력
                return null;
            while (current != null)                 // root가 null이 아니면
            {
                if (Value.CompareTo(current.item) < 0)            // 내가 찾을값 Value랑 Current랑 비교하여 Value 작으면 왼쪽으로가서후 
                    current = current.left;                     // 왼쪽 자식값을 root로 재설정
                else if (Value.CompareTo(current.item) > 0)       // 내가 찾을값 Value랑 Current랑 비교하여 Value가크면 오른쪽
                    current = current.right;
                else return current;                  //찾는값과 currnet값이 같은경우 current 출력
            }
            return null;            // root가 null이 아닌데 없으면 출력 즉 내가 찾는값이 없으면 출력

        }

        /*2. 이진탐색트리의 한계점: 힙상태와 달리 순서대로 채우는 방법이 아니라 링크드리스트처럼 노드형식으로 되어있기때문에
         * 불균형하게 들어갈수있다.불균형하게 들어가면 탐색트리의 시간복잡도가 O(logn)에서 O(n)까지 떨어질수있으므로 불균형하게 들어가는법을 방지해야한다.
         * 따라서 불균형하게 들어가는걸 막기위해 AVL트리랑 레드블랙트리가 사용된다.레드블랙트리는 각각의 노드를 레드나 블랙인 색상속성을 가지게해서 빨간색상의
         * 노드가 연속적으로 나오면 좌회전이나 우회전을 통해 자가균형을 통해 불균형하게 들어가는걸 막는다. AVL트리는 balance Factor(BF) 를 도입하여 모든 노드가 BF를 가지고
         * BF가 -1,0,1중 값을 하나 갖는다. 만약 BF가 셋중 하나가 아니라 다른값을 가지면 자가균형을 통해 좌회전이나 우회전을 한다.
         * 
         * 3. 이진탐색트리의 순회방법은 중위순회방법이다.중위순회방법은 왼쪽, 노드, 오른쪽 이렇게 되어있을때
         * 왼쪽노드가 null이 아닌경우 왼쪽으로 들어간후 출력하고 그다음에 왼쪽오른쪽 중앙에 있는 노드를 출력하고
         * 그리고 오른쪽노드가 null이 아닌경우 오른쪽값을 출력하게끔하면 자동적으로 오름차순으로 출력이된다.
         * 출력순서는 왼쪽노드의 왼쪽자식노드-왼쪽노드-왼쪽자식노드의 오른쪽노드-가운데노드-오른쪽자식노드의 왼쪽노드-오른쪽노드-오른쪽자식노드의 오른쪽노드 순으로한다.
         *
           */
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}