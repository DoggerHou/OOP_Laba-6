using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace OOP_Laba_6
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Model
    {
        public Color object_color;//Цвет объекта
        public PictureBox pb; //Для передачи размера формы для рисования
        protected int RADIX; //Радиус окружности/Вписанной в квадрат окружности/Длина стороны треугольника
        protected Point location; //Точка центра объекта
        protected bool detail;    //Маркер выделенности



        public Model(Point location, Color color, PictureBox pb)
        {
            RADIX = 40;
            this.pb = pb;
            this.location = location;
            detail = true;
            object_color = color;
        }


        public void changeDetail_toFalse() { detail = false; } //Снимает выделение c объекта


        public bool getDetail() { return detail; } //Достает информацию о Выделенности объекта


        public virtual void OnPaint(PaintEventArgs e) { }//Отрисовка


        public virtual bool isPicked(MouseEventArgs e, bool controlUp) { return false; } //Попали ли мы в объект


        public virtual void increase_Size() //Увеличить размер
        {
            if (check_Location(location.X, location.Y, RADIX))
                RADIX += 2;
        }


        public virtual void decrease_Size() //Уменьшить размер
        {
            if (RADIX > 0)
                RADIX -= 2;
        }


        public virtual void move_Object(int _X, int _Y) //Изменить местоположение объекта
        {
            if (check_Location(location.X + _X, location.Y + _Y, RADIX))
            {
                location.X += _X;
                location.Y += _Y;
            }
        }


        public bool check_Location(int point_X, int point_Y, int RADIX) //проверка на выход за границу для круга/квадрата
        {
            if ((point_X - RADIX >= 0) & (point_X + RADIX <= pb.Width) &
                (point_Y - RADIX >= 0) & (point_Y + RADIX <= pb.Height))
                return true;
            return false;
        }
    }


    public class CCircle : Model
    {
        //private int RADIX = 40; //Радиус круга


        public CCircle(Point location, Color color, PictureBox pb) : base(location, color, pb) { }


        public override void OnPaint(PaintEventArgs e)//Отрисовка Эллипса
        {
            Pen pen = new Pen(object_color);
            if (detail)
                pen.Width = 7;
            else
                pen.Width = 4;

            e.Graphics.DrawEllipse(pen, location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//попали ли мы в объект
        {
            if (Math.Pow(location.X - e.X, 2) + Math.Pow(location.Y - e.Y, 2) <= Math.Pow(RADIX, 2)
                & controlUp)
            {
                detail = !detail; //Инвертируем выделенность
                return true;
            }
            return false;//не попал в круг - ниче не делает
        }
    }


    public class CSquare : Model
    {
        //private int RADIX = 40;//Радиус вписанного в квадрат круга(или половина стороны квадрата, кому как нравится)


        public CSquare(Point location, Color color, PictureBox pb) : base(location, color, pb) { }


        public override void OnPaint(PaintEventArgs e)//Отрисовка Квадрата
        {
            Pen pen = new Pen(object_color);
            if (detail)
                pen.Width = 7;
            else
                pen.Width = 4;

            e.Graphics.DrawRectangle(pen, location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//попали ли мы в объект
        {
            if ((e.X <= location.X + RADIX) & (e.X >= location.X - RADIX) &
                (e.Y <= location.Y + RADIX) & (e.Y >= location.Y - RADIX) &
                controlUp)
            {
                detail = !detail; //Инвертируем выделенность
                return true;
            }
            return false;
        }
    }


    public class Triangle : Model
    {
        //private int RADIX = 80; //Длина стороны треугольника
        private Point A; //Координаты Трех точек треугольника
        private Point B;
        private Point C;


        public Triangle(Point location, Color color, PictureBox pb) : base(location, color,pb)
        {
            RADIX *= 2;
            refreshTriangle();
        }


        //Возвращает площадь треугольника
        private double Triangle_Square(double aX, double aY, double bX, double bY, double cX, double cY)
        {
            return Math.Abs(bX * cY - cX * bY - aX * cY + cX * aY + aX * bY - bX * aY);
        }


        //Проверяет, вышел ли треугольник за границы
        private bool check_Borders(int point_aX, int point_cY, int point_aY) //к примеру, point_aX - координата 
        {                                                                    //точки А по оси Х
            if ((point_aX >= 0) & (point_aX + RADIX <= pb.Width) &
                (point_cY >= 0) & (point_aY <= pb.Height))
                return true;
            return false;
        }

        //обновляем координаты точек треугольника после каких-то изменений длины стороны
        private void refreshTriangle() 
        {
            A.X = location.X - RADIX / 2;
            A.Y = (int)(location.Y + Math.Sqrt(3) * RADIX / 6);

            B.X = location.X + RADIX / 2;
            B.Y = (int)(location.Y + Math.Sqrt(3) * RADIX / 6);

            C.X = location.X;
            C.Y = (int)(location.Y - Math.Sqrt(3) * RADIX / 3);
        }


        public override void OnPaint(PaintEventArgs e)//состоит из отрисовки трех линий
        {
            Pen pen = new Pen(object_color);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round; //Чтобы линии были более закругленными

            if (detail)
                pen.Width = 7;
            else
                pen.Width = 4;

            e.Graphics.DrawLine(pen, A.X, A.Y, B.X, B.Y);
            e.Graphics.DrawLine(pen, A.X, A.Y, C.X, C.Y);
            e.Graphics.DrawLine(pen, B.X, B.Y, C.X, C.Y);

        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//по методу сравнения площадей
        {
            if ((Triangle_Square(A.X, A.Y, B.X, B.Y, e.X, e.Y) + Triangle_Square(A.X, A.Y, e.X, e.Y, C.X, C.Y) +
                Triangle_Square(e.X, e.Y, B.X, B.Y, C.X, C.Y) - Triangle_Square(A.X, A.Y, B.X, B.Y, C.X, C.Y) <= 0.01) &
                controlUp)
            {
                detail = !detail; //Инвертируем выделенность
                return true;
            }
            return false;
        }


        public override void increase_Size()
        {
            if (check_Borders(A.X, C.Y, A.Y))
            {
                RADIX += 4;
                refreshTriangle();
            }
        }


        public override void decrease_Size()
        {
            if (RADIX > 0)
            {
                RADIX -= 4;
                refreshTriangle();
            }
        }


        public override void move_Object(int _X, int _Y)
        {
            if (check_Borders(A.X + _X, C.Y + _Y, A.Y + _Y))
            {
                location.X += _X;
                location.Y += _Y;
                refreshTriangle();
            }
        }

    }


    public class Storage
    {
        private List<Model> objects;



        public Storage()
        {
            objects = new List<Model>();
        }


        public int getSize() { return objects.Count(); } //возвращает размер Списка

        public Model getObject(int index) { return objects[index]; } //возвращает объект Model


        public void AddObject(Model temp_object, MouseEventArgs e, bool controlUp)  //добавляет объект
        {
            for (int i = 0; i < objects.Count(); i++) //Если попали в объект, то выходим
                if (objects[i].isPicked(e, controlUp))      //(чтобы не рисовать новый)
                {
                    return;
                }
            objects.Add(temp_object); //если не попали, добавляем новый объект
            for (int i = 0; i < objects.Count() - 1; i++)//снимаем выделение со всех предыдущих
                objects[i].changeDetail_toFalse();
        }


        public void delete_DetailedObjects()    //удаляет все "помеченные" объекты
        {
            for (int i = objects.Count() - 1; i >= 0; i--)
                if (objects[i].getDetail() == true)
                    objects.RemoveAt(i);
        }
    }
}
