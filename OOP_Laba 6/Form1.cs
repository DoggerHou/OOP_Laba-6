using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Laba_6
{
    public partial class Form1 : Form
    {
        Storage myStorage = new Storage();
        bool controlUp = false;
        Color btn_color = Color.Black;
        bool ellipse = true;
        bool square = false;
        bool triangle = false;



        public Form1()
        {
            InitializeComponent();
        }


        private void pictureBox_Paint(object sender, PaintEventArgs e)//отрисовка объектов
        {
            for (int i = 0; i < myStorage.getSize(); i++)
            {
                myStorage.getObject(i).OnPaint(e);
            }

        }


        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
            if (ellipse)//Добавление эллипса
                myStorage.AddObject(new CCircle(e.Location, btn_color), e, controlUp);

            if (square)//добавление квадрата
                myStorage.AddObject(new CSquare(e.Location, btn_color), e, controlUp);
            pictureBox.Invalidate();

            if (triangle)//добавление треугольника
                myStorage.AddObject(new Triangle(e.Location, btn_color), e, controlUp);
            pictureBox.Invalidate();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Add) //обрабатывает нажатие: Увеличение объектов
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail() == true)
                        myStorage.getObject(i).increase_Size();
                }

            if (e.KeyData == Keys.Subtract)//обрабатывает нажатие: Уменьшение объектов
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail() == true)
                        myStorage.getObject(i).decrease_Size();
                }

            if (e.Control) //обрабатывает нажатие: Выбор объектов
                controlUp = true;

            if (e.KeyData == Keys.Delete)//обрабатывает нажатие: Удаление объектов
                myStorage.delete_DetailedObjects();


            //Далее идет обработка 4 кнопок движения, ничего интересного и сложного

            if (e.KeyData == Keys.Right)//Движение вправо
            {
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail())
                        myStorage.getObject(i).move_Object(1, 0);
                }
            }
            if (e.KeyData == Keys.Left)//Движение влево
            {
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail())
                        myStorage.getObject(i).move_Object(-1, 0);
                }
            }
            if (e.KeyData == Keys.Down)//тут +1, ибо ось Y направлена вниз(а не как в православии, вверх)
            {
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail())
                        myStorage.getObject(i).move_Object(0, 1);
                }
            }
            if (e.KeyData == Keys.Up)//тут -1, ибо ось Y направлена вниз(а не как в православии, вверх)
            {
                for (int i = 0; i < myStorage.getSize(); i++)
                {
                    if (myStorage.getObject(i).getDetail())
                        myStorage.getObject(i).move_Object(0, -1);
                }
            }
            pictureBox.Invalidate();
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)//Если кнопка кливиатуры отпущена, значит 
        {                                                      //отпущен и ctrl
            controlUp = false;
        }


        private void button_RED_Click(object sender, EventArgs e)   //Кнопки выбора цвета
        {
            btn_color = ((Button)sender).BackColor;
            for (int i = 0; i < myStorage.getSize(); i++)
            {
                if (myStorage.getObject(i).getDetail())
                    myStorage.getObject(i).object_color = btn_color;
            }
            pictureBox.Invalidate();
            this.ActiveControl = null;
        }


        private void button_ELLIPSE_Click(object sender, EventArgs e)//Кнопки выбора объекта для отрисовки
        {
            if (sender == button_ELLIPSE)
            {
                ellipse = true;
                square = false;
                triangle = false;
                button_ELLIPSE.BackColor = Color.Black;
                button_SQUARE.BackColor = Color.DimGray;
                button_TRIANGLE.BackColor = Color.DimGray;
            }

            else if (sender == button_SQUARE)
            {
                ellipse = false;
                square = true;
                triangle = false;
                button_ELLIPSE.BackColor = Color.DimGray;
                button_SQUARE.BackColor = Color.Black;
                button_TRIANGLE.BackColor = Color.DimGray;
            }
            else if (sender == button_TRIANGLE)
            {
                ellipse = false;
                square = false;
                triangle = true;
                button_ELLIPSE.BackColor = Color.DimGray;
                button_SQUARE.BackColor = Color.DimGray;
                button_TRIANGLE.BackColor = Color.Black;
            }

        }


        private void button_ColorDialog_Click(object sender, EventArgs e)//кнопка для ColorDialog
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_color = colorDialog1.Color;
                ((Button)sender).BackColor = colorDialog1.Color;

                for (int i = 0; i < myStorage.getSize(); i++)    //изменяем цвет у всех выбранных объектов
                {
                    if (myStorage.getObject(i).getDetail())
                        myStorage.getObject(i).object_color = btn_color;
                }
                pictureBox.Invalidate();
            }
            this.ActiveControl = null;
        }
    }
}
