using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classfilebuild
{
    public partial class Form1 : Form
    {

        const string mcuheaddata = "#ifndef _mcuhead_H_\r\n#define _mcuhead_H_\r\n\r\n#define u8 unsigned char\r\n#define u16 unsigned short\r\n#define u32 unsigned long\r\n\r\n#define s8 signed char\r\n#define s16 signed short\r\n#define s32 signed long\r\n\r\n#define vu8 volatile u8\r\n#define vu16 volatile u16\r\n#define vu32 volatile u32\r\n\r\n\r\n#define BIT0 1\r\n#define BIT1 2\r\n#define BIT2 4\r\n#define BIT3 8\r\n#define BIT4 0x10\r\n#define BIT5 0x20\r\n#define BIT6 0x40\r\n#define BIT7 0x80\r\n#define BIT8 0x100\r\n#define BIT9 0x200\r\n#define BIT10 0x400\r\n#define BIT11 0x800\r\n#define BIT12 0x1000\r\n#define BIT13 0x2000\r\n#define BIT14 0x4000\r\n#define BIT15 0x8000\r\n#define BIT16 0x10000\r\n#define BIT17 0x20000\r\n#define BIT18 0x40000\r\n#define BIT19 0x80000\r\n#define BIT20 0x100000\r\n#define BIT21 0x200000\r\n#define BIT22 0x400000\r\n#define BIT23 0x800000\r\n#define BIT24 0x1000000\r\n#define BIT25 0x2000000\r\n#define BIT26 0x4000000\r\n#define BIT27 0x8000000\r\n#define BIT28 0x10000000\r\n#define BIT29 0x20000000\r\n#define BIT30 0x40000000\r\n#define BIT31 0x80000000\r\n\r\n\r\n#define SETBIT(x,y) (x)|=(y)\r\n#define CLRBIT(x,y) (x)&=~(y)\r\n#define CPLBIT(x,y) (x)^=(y);\r\n\r\nunion u2i{\r\n	vu32 l;\r\n	vu16 li[2];\r\n	vu8 lc[4];\r\n	vu16 i;\r\n	vu8 c[2];\r\n};\r\n\r\n#define _16T8H(x) (((union u2i*)&x)->c[1])//??16??8?\r\n#define _16T8L(x) (((union u2i*)&x)->c[0])//??16??8?\r\n\r\n#define _32T16H(x) (((union u2i*)&x)->li[1])//??32??16?\r\n#define _32T16L(x) (((union u2i*)&x)->li[0])//??32??16?\r\n\r\n#define _32T8HH(x) (((union u2i*)&x)->lc[3])//??32??8?\r\n#define _32T8H(x) (((union u2i*)&x)->lc[2])//??32???8?\r\n#define _32T8L(x) (((union u2i*)&x)->lc[1])//??32???8?\r\n#define _32T8LL(x) (((union u2i*)&x)->lc[0])//??32?8?\r\n\r\n#define BIT(x) (((x>>21)&BIT9)|((x>>24)&BIT8)|((x>>21)&BIT7)|((x>>18)&BIT6)|((x>>15)&BIT5)|((x>>12)&BIT4)|((x>>9)&BIT3)|((x>>6)&BIT2)|((x>>3)&BIT1)|((x)&BIT0))\r\n#define BIN(X) BIT(0x##X)\r\n#define BINBYTE(b) BIM(b)\r\n#define BINWORD(b1,b2) ((BIN(b1)<<8)|BIN(b2))\r\n#define BINDWORD(b1,b2,b3,b4) ((BIN(b1)<<24)|(BIN(b2)<<16)|(BIN(b3)<<8)|BIN(b4))\r\n\r\n#endif\r\n";
        const string headdata = "#ifndef _{0:s}_H_\r\n#define _{0:s}_H_\r\n\r\n{1:s}\r\n{2:s}\r\ntypedef struct {{\r\n\tvoid(*Init)(void);\r\n}}{0:s}Base;\r\n\r\nextern const {0:s}Base {0:s};\r\n\r\n#endif\r\n";
        const string sourdata = "#include \"{0:s}.h\"\r\nstatic void init(){{\r\n}}\r\n\r\nconst {0:s}Base {0:s} = {{\r\n\tinit,\t\r\n}};\r\n";
        const string nomcuhead = "#define u8 unsigned char\r\n#define u16 unsigned short\r\n#define u32 unsigned long\r\n\r\n";
        const string ismcuhead = "#include \"mcuhead.h\"";
        public Form1()
        {
            InitializeComponent();
        }

        void buildfile(string path,string o)
        {
            System.IO.FileStream f = new System.IO.FileStream(path, System.IO.FileMode.Create);
            char[] cl = o.ToCharArray();
            byte[] bl = new byte[cl.Length * 2];
            int v = 0;
            int blen = 0;
            for (int i = 0; i < cl.Length; i++)
            {
                v = cl[i];
                if (v < 128)
                {
                    bl[blen++] = (byte)v;
                }
                else
                {
                    bl[blen++] = (byte)(v >> 8);
                    bl[blen++] = (byte)v;
                }
            }
            f.Write(bl, 0, blen);
            f.Flush();
            f.Close();
        }

        void buildfile(string path)
        {
            string add="";
            if (path.IndexOf('.') != -1)
            {
                path = path.Substring(0, path.IndexOf('.'));
            }

            string name = path.Substring(path.LastIndexOf('\\') + 1);
            path = path.Substring(0, path.LastIndexOf('\\') + 1);
            string headchose = nomcuhead;
            if (mcuhead.Checked)
            {
                if (!System.IO.File.Exists(path + "mcuhead.h"))
                {
                    buildfile(path + "mcuhead.h", mcuheaddata);
                }
                headchose = ismcuhead;
            }
            buildfile(path + name + ".h", string.Format(headdata, name, headchose, add));
            buildfile(path + name + ".c", string.Format(sourdata, name));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "(源文件/头文件 .c/.h)|*.*";
            string add = "";
            if (s.ShowDialog() == DialogResult.OK)
            {
                buildfile(s.FileName);
                return;
                string path = s.FileName;
                if (path.IndexOf('.') != -1)
                {
                    path = path.Substring(0, path.IndexOf('.'));
                }
                
                string name = path.Substring(path.LastIndexOf('\\')+1);
                path = path.Substring(0, path.LastIndexOf('\\')+1);
                string headchose = nomcuhead;
                if (mcuhead.Checked)
                {
                    if (!System.IO.File.Exists(path + "mcuhead.h"))
                    {
                        buildfile(path + "mcuhead.h",mcuheaddata);
                    }
                    headchose = ismcuhead;
                }   
                buildfile(path +name+ ".h", string.Format(headdata, name, name, headchose, add));
                buildfile(path +name+ ".c", string.Format(sourdata, name));
            }
        }

        private void exit(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)Keys.Escape)
                System.Environment.Exit(0);
        }

        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            //MessageBox.Show("拖放完成1");
            //e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            //MessageBox.Show("拖放完成2");
            //e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //listView1.Items.Add(v);
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (string.Empty == FileNameBox.Text) return;
            string path = System.Environment.CurrentDirectory+"\\tmp\\";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + FileNameBox.Text + ".c"))
            {
               buildfile(path+ FileNameBox.Text);
            }
            treeView1.DoDragDrop(new DataObject(DataFormats.FileDrop,new string[]{ path + FileNameBox.Text+".c", path + FileNameBox.Text + ".h" }), DragDropEffects.Copy);
            Directory.Delete(path,true);
            //File.Delete(path);
            //File.Delete(path + FileNameBox.Text + ".h");
        }

        private void FileNameBox_TextChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Text == string.Empty)
            {
                if (treeView1.Nodes.Count != 0)
                {
                    treeView1.Nodes.Clear();
                }
                return;
            }
            if (treeView1.Nodes.Count == 0)
            {
                treeView1.Nodes.Add(new TreeNode());
                treeView1.Nodes[0].ImageIndex = 0;
            }
            treeView1.Nodes[0].Text = ((Control)sender).Text;
        }
    }
}
