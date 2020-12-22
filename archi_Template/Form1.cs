using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace archi_Template
{
    public partial class Form1 : Form
    {
        string instructions;
        Dictionary<string, int> MipsRegister = new Dictionary<string, int>();
        Dictionary<uint, string> MipsMemory = new Dictionary<uint, string>();
        Dictionary<int, string> insDict = new Dictionary<int, string>();
        int PC, SizeOfIns;
        string[] arr;
        string a,b,c,d,WE,EX,M;
        int resALU,stageOneCount, stageTwoCount, stageThreeCount, stageFourCount, stageFiveCount;

        private void PiplineGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void MipsRegisterGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void runCycleBtn_Click(object sender, EventArgs e)
        {
          
            stageOneCount++; stageTwoCount++; stageThreeCount++; stageFourCount++; stageFiveCount++;
            if (stageFiveCount >= 5 && stageFiveCount <= SizeOfIns + 4)
            {
                Get_StageFive(d);
            }
            if (stageFourCount >= 4 && stageFourCount <= SizeOfIns + 3)
            {
                d = Get_StageFour(c);
            }
            if (stageThreeCount >= 3 && stageThreeCount <= SizeOfIns + 2)
            {
                c = Get_StageThree(b);
                string[] x = c.Split('|');
                string[] y = x[0].Split('&');
                string[] z = x[1].Split('&');
                PiplineGrid.Rows[16].Cells[0].Value = "EX/MEM RegWrite";
                PiplineGrid.Rows[16].Cells[1].Value = y[0];
                PiplineGrid.Rows[17].Cells[0].Value = "EX/MEM memToReg";
                PiplineGrid.Rows[17].Cells[1].Value = y[1];
                PiplineGrid.Rows[18].Cells[0].Value = "EX/MEM branch";
                PiplineGrid.Rows[18].Cells[1].Value = z[0];
                PiplineGrid.Rows[19].Cells[0].Value = "EX/MEM memRead";
                PiplineGrid.Rows[19].Cells[1].Value = z[1];
                PiplineGrid.Rows[20].Cells[0].Value = "EX/MEM memWrite";
                PiplineGrid.Rows[20].Cells[1].Value = z[2];
                PiplineGrid.Rows[21].Cells[0].Value = "EX/MEM SumAdder";
                PiplineGrid.Rows[21].Cells[1].Value = x[2];
                PiplineGrid.Rows[22].Cells[0].Value = "EX/MEM resALU";
                PiplineGrid.Rows[22].Cells[1].Value = x[3];
                PiplineGrid.Rows[23].Cells[0].Value = "EX/MEM readData2";
                PiplineGrid.Rows[23].Cells[1].Value = x[4];
                PiplineGrid.Rows[24].Cells[0].Value = "EX/MEM RegDst";
                PiplineGrid.Rows[24].Cells[1].Value = x[5];

            }
            if (stageTwoCount >= 2 && stageTwoCount <= SizeOfIns + 1)
            {
                b = Get_StageTwo(a);
                string[] x = b.Split('|');
                string[] y = x[0].Split('&');
                string[] z = x[1].Split('&');
                string[] w = x[2].Split('&');
                PiplineGrid.Rows[2].Cells[0].Value = "ID/EX RegWrite";
                PiplineGrid.Rows[2].Cells[1].Value = y[0];
                PiplineGrid.Rows[3].Cells[0].Value = "ID/EX memToReg";
                PiplineGrid.Rows[3].Cells[1].Value = y[1];
                PiplineGrid.Rows[4].Cells[0].Value = "ID/EX branch";
                PiplineGrid.Rows[4].Cells[1].Value = z[0];
                PiplineGrid.Rows[5].Cells[0].Value = "ID/EX memRead";
                PiplineGrid.Rows[5].Cells[1].Value = z[1];
                PiplineGrid.Rows[6].Cells[0].Value = "ID/EX memWrite";
                PiplineGrid.Rows[6].Cells[1].Value = z[2];
                PiplineGrid.Rows[7].Cells[0].Value = "ID/EX RegDst";
                PiplineGrid.Rows[7].Cells[1].Value = w[0];
                PiplineGrid.Rows[8].Cells[0].Value = "ID/EX AluOP";
                PiplineGrid.Rows[8].Cells[1].Value = w[1];
                PiplineGrid.Rows[9].Cells[0].Value = "ID/EX AluSrc";
                PiplineGrid.Rows[9].Cells[1].Value = w[2];
                PiplineGrid.Rows[10].Cells[0].Value = "ID/EX PC";
                PiplineGrid.Rows[10].Cells[1].Value = x[3];
                PiplineGrid.Rows[11].Cells[0].Value = "ID/EX ReadData1";
                PiplineGrid.Rows[11].Cells[1].Value = x[4];
                PiplineGrid.Rows[12].Cells[0].Value = "ID/EX ReadData2";
                PiplineGrid.Rows[12].Cells[1].Value = x[5];
                PiplineGrid.Rows[13].Cells[0].Value = "ID/EX instr[15-0]";
                PiplineGrid.Rows[13].Cells[1].Value = x[6];
                PiplineGrid.Rows[14].Cells[0].Value = "ID/EX instr[20-16]";
                PiplineGrid.Rows[14].Cells[1].Value = x[7];
                PiplineGrid.Rows[15].Cells[0].Value = "ID/EX instr[15-11]";
                PiplineGrid.Rows[15].Cells[1].Value = x[8];

            }

            if (stageOneCount >= 1 && stageOneCount <= SizeOfIns)
            {
                string x = PC + insDict[PC];
                a = Get_StageOne(x);
                PiplineGrid.Rows[0].Cells[0].Value = "IF/ID PC";
                PiplineGrid.Rows[0].Cells[1].Value = a.Substring(0,4);
                PiplineGrid.Rows[1].Cells[0].Value = "IF/ID Instruction";
                PiplineGrid.Rows[1].Cells[1].Value = a.Substring(4, 32);
                PC += 4;
                StartPCTxt.Text = PC + "";
            }

        }
        private string Get_StageOne(string regPipline)
        {
            string IF_ID;
            IF_ID =(int.Parse( regPipline.Substring(0, 4))+4) + regPipline.Substring(4, 32);
            return IF_ID;
        }
        private string Get_StageTwo(string regPipline1)
        {
            string  ID_EX;
            string regdst, aluop, alusrc, branch, memRead, memWrite, RegWrite, memToReg;
            if (regPipline1.Substring(4, 6) == "000000")
            {
                regdst = "1";aluop = "10";alusrc = "0";branch = "0";memRead = "0";memWrite = "0";memToReg = "0";RegWrite = "1";
                WE=RegWrite+"&" + memToReg;
                M = branch + "&" + memRead + "&" + memWrite;
                EX = regdst + "&" + aluop + "&" + alusrc;
            }
            if (regPipline1.Substring(4, 6) != "000000")
            {
                regdst = "X"; aluop = "00"; alusrc = "1"; branch = "0"; memRead = "0"; memWrite = "1"; memToReg = "X"; RegWrite = "0";
                WE = RegWrite + "&" + memToReg;
                M = branch + "&" + memRead + "&" + memWrite;
                EX = regdst + "&" + aluop + "&" + alusrc;
            }
            string pc = regPipline1.Substring(0, 4);
            string rs = regPipline1.Substring(10, 5);
            string rt = regPipline1.Substring(15, 5);
            string rd = regPipline1.Substring(20, 5);
            string signExtend =regPipline1.Substring(20, 16);
            int rsVal =Convert.ToInt32(rs,2);
            int rtVal=Convert.ToInt32(rt, 2);
            ID_EX =WE+"|"+M+"|"+EX+"|"+ pc+"|"+ MipsRegister[("$" + rsVal)]+"|"+ MipsRegister[("$" + rtVal)]+"|"+ signExtend+ "|"+ Convert.ToInt32(rt, 2) + "|"+ Convert.ToInt32(rd, 2);
            return ID_EX;
        }

        private string Get_StageThree(string regPipline2)
        {
            string EX_MEM;
            string[] Arr_regPipline3 = regPipline2.Split('|');
            WE = Arr_regPipline3[0];
            M = Arr_regPipline3[1];
            string []arr_ex = Arr_regPipline3[2].Split('&');
            int sum_adder = adder(Arr_regPipline3[3], Arr_regPipline3[6]);
            string type = Arr_regPipline3[6].Substring(10, 6);
            string ALU_SRC_MUx = ALUSRC(Arr_regPipline3[5], Arr_regPipline3[6], arr_ex[2]);
            resALU = ALU(Arr_regPipline3[4], ALU_SRC_MUx, type);
            string read_data2 = Arr_regPipline3[5];
            string regDst_Mux = RegDst(Arr_regPipline3[7], Arr_regPipline3[8], arr_ex[0]);
            EX_MEM =WE+"|"+M+"|"+ sum_adder + "|" + resALU + "|" + read_data2 + "|" + regDst_Mux;
            return EX_MEM;
        }
        private string Get_StageFour(string regPipline3)
        {
            string MEM_WB;
            string[] Arr_regPipline4 = regPipline3.Split('|');
            string[] arr_M = Arr_regPipline4[1].Split('&');
            WE = Arr_regPipline4[0];
            string[] x = WE.Split('&');
            if (arr_M[2] != "1")
            {
                PiplineGrid.Rows[25].Cells[0].Value = "MEM/WB RegWrite";
                PiplineGrid.Rows[25].Cells[1].Value = x[0];
                PiplineGrid.Rows[26].Cells[0].Value = "MEM/WB memToReg";
                PiplineGrid.Rows[26].Cells[1].Value = x[1];
                PiplineGrid.Rows[27].Cells[0].Value = "MEM/WB ReadData";
                PiplineGrid.Rows[27].Cells[1].Value = "Not VAl";
                PiplineGrid.Rows[28].Cells[0].Value = "MEM/WB resALU";
                PiplineGrid.Rows[28].Cells[1].Value = Arr_regPipline4[3];
                PiplineGrid.Rows[29].Cells[0].Value = "MEM/WB RegDstRet";
                PiplineGrid.Rows[29].Cells[1].Value = Arr_regPipline4[5];
                MEM_WB =WE+"|"+"Not VAl"+"|"+ Arr_regPipline4[3] + "|" + Arr_regPipline4[5];

            }
            else
            {
                PiplineGrid.Rows[25].Cells[0].Value = "MEM/WB RegWrite";
                PiplineGrid.Rows[25].Cells[1].Value = x[0];
                PiplineGrid.Rows[26].Cells[0].Value = "MEM/WB memToReg";
                PiplineGrid.Rows[26].Cells[1].Value = x[1];
                PiplineGrid.Rows[27].Cells[0].Value = "MEM/WB ReadData";
                PiplineGrid.Rows[27].Cells[1].Value = "Not VAl";
                PiplineGrid.Rows[28].Cells[0].Value = "MEM/WB resALU";
                PiplineGrid.Rows[28].Cells[1].Value = Arr_regPipline4[3];
                PiplineGrid.Rows[29].Cells[0].Value = "MEM/WB RegDstRet";
                PiplineGrid.Rows[29].Cells[1].Value = "NOT dest this SW";
                MipsMemory[uint.Parse(Arr_regPipline4[3])] = Arr_regPipline4[4];
                MemoryGrid.Rows[int.Parse(Arr_regPipline4[3])].Cells[1].Value = Arr_regPipline4[4];
                MEM_WB = "x";
            }

            return MEM_WB;
        }
        private void Get_StageFive(string regPipline4)
        {
            

            if (regPipline4 != "x")
            {
                string[] Arr_regPipline5 = regPipline4.Split('|');
                int dest = int.Parse(Arr_regPipline5[3]);
                MipsRegister[("$" + dest)] = int.Parse(Arr_regPipline5[2]);
                MipsRegisterGrid.Rows[dest].Cells[1].Value = MipsRegister[("$" + dest)];
            }
        }
        private string ALUSRC(string a,string b, string selector)
        {
            if (selector=="0")
                return a;
            else
                return b;
        }
        private string RegDst(string a,string b,string selector)
        {
            if (selector=="1")
                return b;
            else
                return "x";
        }
        private int ALU(string reg1,string reg2,string type)
        {
            if (type == "100010")
                return int.Parse(reg1) - int.Parse(reg2);
            if(type=="100101")
                return int.Parse(reg1) | int.Parse(reg2);
            if(type=="100100")
                return int.Parse(reg1) & int.Parse(reg2);
            if (type == "100000")
                return int.Parse(reg1) + int.Parse(reg2);
            else
                return int.Parse(reg1) + Convert.ToInt32(reg2, 2);
        }
        private int adder(string a1,string a2)
        {
            return (int.Parse(a1)<<2) + Convert.ToInt32(a2, 2);
        }
       
       

        private void inializeBtn_Click(object sender, EventArgs e)
        {
            MipsRegisterGrid.Rows.Add(32);
            PiplineGrid.Rows.Add(29);
            MemoryGrid.Rows.Add(1000);
            for (int i = 0; i < 1000; i++)
            {
                MemoryGrid.Rows[i].Cells[0].Value = i ;
            }
            
            MipsRegister["$0"] = 0;
            MipsRegisterGrid.Rows[0].Cells[0].Value ="$0";
            MipsRegisterGrid.Rows[0].Cells[1].Value = MipsRegister["$0"];
            for (int i = 1; i < 32; i++)
            {
                MipsRegister[("$" + i)] =(int)(i+100);
                MipsRegisterGrid.Rows[i].Cells[0].Value = ("$" + i);
                MipsRegisterGrid.Rows[i].Cells[1].Value = MipsRegister[("$" + i)];
            }
            instructions = UserCodetxt.Text.ToString();
            PC = int.Parse(StartPCTxt.Text);
            arr = instructions.Split('\n');
            SizeOfIns= arr.Count();
            string temp;
            for (int i = 0; i < SizeOfIns; i++)
            {
                temp = arr[i].Substring(6, 6) + arr[i].Substring(13, 5) + arr[i].Substring(19, 5) + arr[i].Substring(25, 5) + arr[i].Substring(31, 5) + arr[i].Substring(37, 6);
                insDict[int.Parse(arr[i].Substring(0, 4))]=temp;
            }
            stageOneCount = 0; stageTwoCount = 0; stageThreeCount = 0; stageFourCount = 0;stageFiveCount = 0;

        }
    }
}
