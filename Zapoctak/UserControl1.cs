using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zapoctak.gui;

namespace Zapoctak
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        //apply holy water to eyes after looking at this method
        public void bind(CharacterSelection sel)
        {
            sel.addRemove[0] = button1;
            sel.addRemove[1] = button2;
            sel.addRemove[2] = button3;
            sel.addRemove[3] = button4;

            sel.prev[0] = button5;
            sel.prev[1] = button6;
            sel.prev[2] = button7;
            sel.prev[3] = button8;

            sel.next[0] = button9;
            sel.next[1] = button10;
            sel.next[2] = button11;
            sel.next[3] = button12;

            sel.pictures[0] = pictureBox1;
            sel.pictures[1] = pictureBox2;
            sel.pictures[2] = pictureBox3;
            sel.pictures[3] = pictureBox4;

            sel.classLabel[0] = label2;
            sel.classLabel[1] = label3;
            sel.classLabel[2] = label4;
            sel.classLabel[3] = label5;
        }
    }
}
