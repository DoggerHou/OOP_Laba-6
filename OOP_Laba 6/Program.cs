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
        protected int RADIX; //Радиус окружности/Вписанной в квадрат окружности/Длина стороны треугольника
        protected Point location; //Точка центра объекта
        protected bool detail;    //Маркер выделенности
        public Color object_color;//Цвет объекта



        public Model(Point location, Color color)
        {
            RADIX = 40;
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
            if ((point_X - RADIX >= 2) & (point_X + RADIX <= 736) &
                (point_Y - RADIX >= 2) & (point_Y + RADIX <= 478))
                return true;
            return false;
        }
    }


    public class CCircle : Model
    {
        //private int RADIX = 40; //Радиус круга


        public CCircle(Point location, Color color) : base(location, color) { }


        public override void OnPaint(PaintEventArgs e)//Отрисовка Эллипса
        {
            if (detail == false)
                e.Graphics.DrawEllipse(new Pen(object_color, 4f), location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
            else
                e.Graphics.DrawEllipse(new Pen(object_color, 7f), location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//попали ли мы в объект
        {
            if (Math.Pow(location.X - e.X, 2) + Math.Pow(location.Y - e.Y, 2) <= Math.Pow(RADIX, 2)
                & controlUp & detail)
            {
                detail = false; //Если попал в круг, а он выделен - снять выделение
                return true;
            }
            if (Math.Pow(location.X - e.X, 2) + Math.Pow(location.Y - e.Y, 2) <= Math.Pow(RADIX, 2)
                & controlUp)
            {
                detail = true;//Если попал в круг, а он не выделен - выделить
                return true;
            }
            return false;//не попал в круг - ниче не делает
        }
    }


    public class CSquare : Model
    {
        //private int RADIX = 40;//Радиус вписанного в квадрат круга(или половина стороны квадрата, кому как нравится)


        public CSquare(Point location, Color color) : base(location, color) { }


        public override void OnPaint(PaintEventArgs e)//Отрисовка Квадрата
        {
            if (detail == false)
                e.Graphics.DrawRectangle(new Pen(object_color, 4f), location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
            else
                e.Graphics.DrawRectangle(new Pen(object_color, 7f), location.X - RADIX, location.Y - RADIX, RADIX * 2, RADIX * 2);
        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//попали ли мы в объект
        {
            if ((e.X <= location.X + RADIX) & (e.X >= location.X - RADIX) &
                (e.Y <= location.Y + RADIX) & (e.Y >= location.Y - RADIX) &
                controlUp & detail)
            {
                detail = false; //Если попал в квадрат, а он выделен - снять выделение
                return true;
            }
            if ((e.X <= location.X + RADIX) & (e.X >= location.X - RADIX) &
                (e.Y <= location.Y + RADIX) & (e.Y >= location.Y - RADIX) &
                controlUp & detail == false)
            {
                detail = true; //Если попал в квадрат, а он не выделен - добавить выделение
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
        private Pen pen1;//карандаши для рисовния(один жирный, другой обычный)
        private Pen pen2;


        public Triangle(Point location, Color color) : base(location, color)
        {
            RADIX *= 2;
            A.X = location.X - RADIX / 2;
            A.Y = (int)(location.Y + Math.Sqrt(3) * RADIX / 6);

            B.X = location.X + RADIX / 2;
            B.Y = (int)(location.Y + Math.Sqrt(3) * RADIX / 6);

            C.X = location.X;
            C.Y = (int)(location.Y - Math.Sqrt(3) * RADIX / 3);
        }


        //Возвращает площадь треугольника
        private double Triangle_Square(double aX, double aY, double bX, double bY, double cX, double cY)
        {
            return Math.Abs(bX * cY - cX * bY - aX * cY + cX * aY + aX * bY - bX * aY);
        }


        //Проверяет, вышел ли треугольник за границы
        private bool check_Borders(int point_aX, int point_cY, int point_aY) //к примеру, point_aX - координата 
        {                                                                    //точки А по оси Х
            if ((point_aX >= 2) & (point_aX + RADIX <= 736) &
                (point_cY >= 2) & (point_aY <= 478))
                return true;
            return false;
        }


        private void refreshTriangle() //обновляем координаты точек треугольника после каких-то изменений длины стороны
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
            pen1 = new Pen(object_color, 4f);
            pen2 = new Pen(object_color, 7f);
            pen1.StartCap = System.Drawing.Drawing2D.LineCap.Round; //Чтобы линии были более закругленными
            pen2.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            if (detail == false)
            {
                e.Graphics.DrawLine(pen1, A.X, A.Y, B.X, B.Y);
                e.Graphics.DrawLine(pen1, A.X, A.Y, C.X, C.Y);
                e.Graphics.DrawLine(pen1, B.X, B.Y, C.X, C.Y);
            }
            else
            {
                e.Graphics.DrawLine(pen2, A.X, A.Y, B.X, B.Y);
                e.Graphics.DrawLine(pen2, A.X, A.Y, C.X, C.Y);
                e.Graphics.DrawLine(pen2, B.X, B.Y, C.X, C.Y);
            }
        }


        public override bool isPicked(MouseEventArgs e, bool controlUp)//по методу сравнения площадей
        {
            if ((Triangle_Square(A.X, A.Y, B.X, B.Y, e.X, e.Y) + Triangle_Square(A.X, A.Y, e.X, e.Y, C.X, C.Y) +
                Triangle_Square(e.X, e.Y, B.X, B.Y, C.X, C.Y) - Triangle_Square(A.X, A.Y, B.X, B.Y, C.X, C.Y) <= 0.01) &
                controlUp & detail)
            {
                detail = false; //Если попал в треугольник, а он выделен - снять выделение
                return true;
            }
            if ((Triangle_Square(A.X, A.Y, B.X, B.Y, e.X, e.Y) + Triangle_Square(A.X, A.Y, e.X, e.Y, C.X, C.Y) +
                Triangle_Square(e.X, e.Y, B.X, B.Y, C.X, C.Y) - Triangle_Square(A.X, A.Y, B.X, B.Y, C.X, C.Y) <= 0.01) &
                controlUp)
            {
                detail = true; //Если попал в треугольник, а он выделен - снять выделение
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
