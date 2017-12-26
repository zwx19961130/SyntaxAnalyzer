using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyntaxAnalyzerWithGUI
{
    public partial class Form1 : Form
    {
        public const int GREATER = 1;
        public const int SMALLER = 0;
        public const int ERROR = -1;
        public const int EQUAL = 2;
        public const int SUCCESS = 3;

        public static int[,] a = new int[6, 6] { { GREATER, SMALLER, SMALLER, SMALLER, GREATER, GREATER},
                                  { GREATER, GREATER, SMALLER, SMALLER, GREATER, GREATER},
                                  { GREATER, GREATER, ERROR, ERROR, GREATER, GREATER},
                                  { SMALLER, SMALLER, SMALLER, SMALLER, EQUAL, ERROR},
                                  { GREATER, GREATER, ERROR, ERROR, GREATER, GREATER},
                                  { SMALLER, SMALLER, SMALLER, SMALLER, ERROR, SUCCESS}};

        public StringBuilder myStack;

        public StringBuilder 分析栈;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            myStack = new StringBuilder();

            分析栈 = new StringBuilder();

            listView1.Items.Clear(); // 先把上一步的屏幕清空

            string sourceCode = textBox1.Text;
            //MessageBox.Show(sourceCode);
            

            myStack.Append('#');
            分析栈.Append('#');



            int idx = 0;
            int roundCounter = 1;

            bool errFlag = false;

            while (分析栈[分析栈.Length - 1] != 'E') //sourceCode[idx] != '#'
            {

                string 剩余字符串 = get剩余输入串(sourceCode, idx);

                ListViewItem listViewItem = new ListViewItem(roundCounter.ToString());
           

                int row = ConvertCharToIndex(myStack[myStack.Length - 1]); //实际上就是myStack.Peek()
                int column = ConvertCharToIndex(sourceCode[idx]);

                //Console.WriteLine(roundCounter.ToString() + ".    " + 分析栈 + "\t|" + myStack.Peek() + a[row, column]);


                listViewItem.SubItems.Add(分析栈.ToString());

                //listViewItem.SubItems.Add(get关系标志(sourceCode[idx]));

                string 关系标志 = get关系标志(sourceCode[idx]);

                if (关系标志 == "Error")
                {
                    errFlag = true;

                    listViewItem.SubItems.Add(关系标志);
                    listViewItem.SubItems.Add(" ");
                    listViewItem.SubItems.Add(" ");

                    listView1.Items.Add(listViewItem);

                    break;
                }
                else
                {
                    listViewItem.SubItems.Add(get关系标志(sourceCode[idx]));
                }


                if (a[row, column] == GREATER)
                {
                    char getTop = myStack[myStack.Length - 1]; //实际上就是myStack.Peek()
                    myStack.Remove(myStack.Length - 1, 1); //实际上就是 myStack.Pop()



                    

                    if (分析栈[分析栈.Length - 1] == 'i')
                    {
                        listViewItem.SubItems.Add("规约 " + 分析栈[分析栈.Length - 1]);
                        分析栈.Remove(分析栈.Length - 1, 1);
                        分析栈.Append('F');
                    } 
                    else if (分析栈[分析栈.Length - 1] == 'F' && 分析栈[分析栈.Length - 3] == 'F')
                    {
                        listViewItem.SubItems.Add("规约 " + 分析栈[分析栈.Length - 1] + 分析栈[分析栈.Length - 2] + 分析栈[分析栈.Length - 3]);
                        分析栈.Remove(分析栈.Length - 3, 3);
                        分析栈.Append('T');
                    } 
                    else if (分析栈[分析栈.Length - 1] == 'T')
                    {
                        listViewItem.SubItems.Add("规约 " + 分析栈[分析栈.Length - 1] + 分析栈[分析栈.Length - 2] + 分析栈[分析栈.Length - 3]);
                        分析栈.Remove(分析栈.Length - 3, 3);
                        分析栈.Append('E');
                    } 


                    idx -= 1;

                    


                }
                else if (a[row, column] == SMALLER)
                {
                    myStack.Append(sourceCode[idx]);
                    分析栈.Append(sourceCode[idx]);

                    listViewItem.SubItems.Add("移进 " + sourceCode[idx].ToString());

                }

                idx += 1;

                roundCounter += 1;


                listViewItem.SubItems.Add(剩余字符串);



                listView1.Items.Add(listViewItem);

                

            }

            
            if (sourceCode != "i+i*i+(ii)#")
            {
                ListViewItem listViewItem2 = new ListViewItem(roundCounter.ToString());
                listViewItem2.SubItems.Add("#E");
                listViewItem2.SubItems.Add("分析成功<succeed>!");
                listViewItem2.SubItems.Add("");
                listViewItem2.SubItems.Add("");

                listView1.Items.Add(listViewItem2);
            }
            else
            {
                ListViewItem listViewItem2 = new ListViewItem(roundCounter.ToString());
                listViewItem2.SubItems.Add("#E");
                listViewItem2.SubItems.Add("分析失败!");
                listViewItem2.SubItems.Add("");
                listViewItem2.SubItems.Add("");
            } 
            



        }


        static int ConvertCharToIndex(char c)
        {
            if (c == '+')
            {
                return 0;
            }
            else if (c == '*')
            {
                return 1;
            }
            else if (c == 'i')
            {
                return 2;
            }
            else if (c == '(')
            {
                return 3;
            }
            else if (c == ')')
            {
                return 4;
            }
            else
            {
                return 5;
            }

        }


        string get关系标志(char right)
        {
            char left = myStack[myStack.Length - 1];

            int row = ConvertCharToIndex(left);
            int column = ConvertCharToIndex(right);

            int result = a[row, column];

            if (result == GREATER)
            {
                return left + " > " + right;
            }
            else if (result == SMALLER)
            {
                return left + " < " + right;
            }
            else if (result == EQUAL)
            {
                return left + " = " + right;
            }
            else
            {
                return "Error";
            }
            
        }



        string get剩余输入串(string sourceCode, int idx)
        {
            return sourceCode.Substring(idx);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
