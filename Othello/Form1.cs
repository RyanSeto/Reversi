using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Othello
{
    public partial class Form1 : Form
    {
        Random rand;
        Rectangle[,] board;
        Pen blaPen, rPen, grPen, purpPen;
        Bitmap redPiece, bluePiece, redTrans, blueTrans;
        Brush grBrush, blaBrush, bluBrush, rBrush, purpBrush;
        Boolean[,] filled, red, possible, flipping;
        Boolean redTurn, start, nowBreak, end, AITrue, AIRed, AIGoing, AIWent, AIOnce, contained, shouldPass;
        int size, xMouse, yMouse, xHover, yHover, x, y, posX, posY, redScore, blueScore, aix, aiy, time;
        Font font;

        public Form1()
        {
            InitializeComponent();

            rand = new Random();
            board = new Rectangle[8, 8];
            filled = new Boolean[8, 8];
            red = new Boolean[8, 8];
            possible = new Boolean[8, 8];
            flipping = new Boolean[8, 8];

            blaPen = new Pen(Color.Black, 1);
            rPen = new Pen(Color.Red, 2);
            grPen = new Pen(Color.Green, 3);
            purpPen = new Pen(Color.Purple, 3);
            grBrush = new SolidBrush(Color.Bisque);
            blaBrush = new SolidBrush(Color.Black);
            bluBrush = new SolidBrush(Color.Blue);
            rBrush = new SolidBrush(Color.Red);
            purpBrush = new SolidBrush(Color.Purple);

            redPiece = new Bitmap("redCircle.png");
            bluePiece = new Bitmap("blueCircle.png");
            redTrans = new Bitmap("redCircleTrans.png");
            blueTrans = new Bitmap("blueCircleTrans.png");

            font = new Font("Comic Sans MS", 12);

            size = 50;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new Rectangle((i * size) + 80, (j * size) + 120, size, size);
                }
            }

            filled[3, 3] = true;
            filled[3, 4] = true;
            filled[4, 3] = true;
            filled[4, 4] = true;

            red[3, 3] = true;
            red[4, 4] = true;
            red[3, 4] = false;
            red[4, 3] = false;

            if (rand.Next(0, 2) == 1)
            {
                redTurn = true;
            }

            else
            {
                redTurn = false;
            }

            end = false;

            timer1.Stop();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    g.FillRectangle(grBrush, board[i, j]);
                    g.DrawRectangle(blaPen, board[i, j]);

                    if (start == true)
                    {
                        if (filled[i, j] == true)
                        {
                            if (red[i, j] == true)
                            {
                                g.DrawImage(redPiece, board[i, j]);
                            }

                            else
                            {
                                g.DrawImage(bluePiece, board[i, j]);
                            }
                        }

                        else if (possible[i, j] == true)
                        {
                            if (redTurn == true)
                            {
                                g.DrawImage(redTrans, board[i, j]);
                            }

                            else
                            {
                                g.DrawImage(blueTrans, board[i, j]);
                            }
                        }

                        if (board[i, j].Contains(xMouse, yMouse))
                        {
                            xHover = i;
                            yHover = j;
                        }
                    }
                }
            }

            if (start == true)
            {
                if (redTurn == true && filled[xHover, yHover] == false)
                {
                    if (AITrue == false || AIRed == false)
                    {
                        if (possible[xHover, yHover] == true)
                        {
                            g.DrawRectangle(grPen, board[xHover, yHover]);
                        }

                        else
                        {
                            g.DrawRectangle(rPen, board[xHover, yHover]);
                        }

                        g.DrawImage(redPiece, board[xHover, yHover].X, board[xHover, yHover].Y, size, size);
                    }

                //    g.DrawImage(redPiece, board[xHover, yHover].X, board[xHover, yHover].Y, size, size);
                }

                else if (redTurn == false && filled[xHover, yHover] == false)
                {
                    if (AITrue == false || AIRed == true)
                    {
                        if (possible[xHover, yHover] == true)
                        {
                            g.DrawRectangle(grPen, board[xHover, yHover]);
                        }

                        else
                        {
                            g.DrawRectangle(rPen, board[xHover, yHover]);
                        }

                        g.DrawImage(bluePiece, board[xHover, yHover].X, board[xHover, yHover].Y, size, size);
                    }

            //        g.DrawImage(bluePiece, board[xHover, yHover].X, board[xHover, yHover].Y, size, size);
                }  

                if (redTurn == true && end == false)
                {
                    g.DrawString("Red, it is your turn.", font, rBrush, 195, 60);
                }

                else if(redTurn == false && end == false)
                {
                    g.DrawString("Blue, it is your turn.", font, bluBrush, 195, 60);
                }

                g.DrawString("Red Score: " + redScore.ToString(), font, rBrush, 20, 60);
                g.DrawString("Blue Score: " + blueScore.ToString(), font, bluBrush, 420, 60);

                if (AIGoing == true && AIRed == true && end == false)
                {
                    g.DrawString("Red AI is going in",  font, rBrush, 500, 120);
                    g.DrawString(time.ToString(), font, rBrush, 550, 150);
                }

                else if (AIGoing == true && AIRed == false && end == false)
                {
                    g.DrawString("Blue AI is going in", font, bluBrush, 500, 120);
                    g.DrawString(time.ToString(), font, bluBrush, 550, 150);
                }

                if (AITrue == true && AIWent == true)
                {
                    if (AIRed == true && redTurn == false || AIRed == false && redTurn == true)
                    {
                        g.DrawRectangle(purpPen, board[aix, aiy]);
                    }
                }
            }

            if (end == true)
            {
                if (redScore > blueScore)
                {
                    g.DrawString("Red, you win!", font, rBrush, 195, 60);
                }

                else if (blueScore > redScore)
                {
                    g.DrawString("Blue, you win!", font, bluBrush, 195, 60);
                }

                else
                {
                    g.DrawString("It's a tie!", font, purpBrush, 195, 60);
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (AITrue == true && AIRed == true && redTurn == true)
            {

            }

            else if (AITrue == true && AIRed == false && redTurn == false)
            {

            }

            else
            {
                xMouse = e.X;
                yMouse = e.Y;

                Invalidate();
            }  
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            AIWent = false;
            end = false;
            buttonPass.Enabled = true;

            if (start == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        board[i, j] = new Rectangle((i * size) + 80, (j * size) + 120, size, size);
                        filled[i, j] = false;
                        possible[i, j] = false;
                    }
                }

                filled[3, 3] = true;
                filled[3, 4] = true;
                filled[4, 3] = true;
                filled[4, 4] = true;

                red[3, 3] = true;
                red[4, 4] = true;
                red[3, 4] = false;
                red[4, 3] = false;

                if (rand.Next(0, 2) == 1)
                {
                    redTurn = true;
                }

                else
                {
                    redTurn = false;
                }

            }

            start = true;
            buttonStart.Text = "Restart";

            if (redTurn == true)
            {
                redPossibilities(sender, e);
            }

            else if (redTurn == false)
            {
                bluePossibilities(sender, e);
            }

            redScore = 2;
            blueScore = 2;

            timer1.Stop();
            AIGoing = false;
            Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            nowBreak = false;
            contained = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j].Contains(e.X, e.Y))
                    {
                        if (possible[i, j] == true && filled[i, j] == false)
                        {
                            contained = true;
                            posX = i;
                            posY = j;

                            if (redTurn == true)
                            {
                                if (AITrue == false || AITrue == true && AIRed == false)
                                {
                                    red[i, j] = true;
                                    filled[i, j] = true;
                                    redTurn = false;

                                    //         posX = i;
                                    //          posY = j;
                                    flippingForRed(sender, e);
                                }
                            }

                            else if(redTurn == false)
                            {
                                if (AITrue == false || AITrue == true && AIRed == true)
                                {
                                    red[i, j] = false;
                                    filled[i, j] = true;
                                    redTurn = true;

                                    //       posX = i;
                                    //       posY = j;
                                    flippingForBlue(sender, e);
                                }
                            }
                        }

                        nowBreak = true;
                        break;
                    }
                }

                if (nowBreak == true)
                {
                    break;
                }
            }

            if (contained == true)
            {
                redScore = 0;
                blueScore = 0;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        possible[i, j] = false;

                        if (filled[i, j] == true)
                        {
                            if (red[i, j] == true)
                            {
                                redScore++;
                            }

                            else
                            {
                                blueScore++;
                            }
                        }
                    }
                }
            }

            if (redTurn == true)// && AIRed == true)
            {
                 redPossibilities(sender, e);

                if (AITrue == true && AIRed == true && AIOnce == false)
                {
                    AIGoing = true;
                    time = 3;
                    AIOnce = true;

                    timer1.Start();
                   // redAIAction(sender, e);
                }
            }

            else if (redTurn == false)// && AIRed == false)
            {
                bluePossibilities(sender, e);

                if (AITrue == true && AIRed == false && AIOnce == false)
                {
                    AIGoing = true;
                    time = 3;
                    AIOnce = true;

                    timer1.Start();
                    // redAIAction(sender, e);
                }
            }

            if (redScore + blueScore == 64)
            {
                end = true;
                buttonPass.Enabled = false;
            }

            else if (redScore == 0 || blueScore == 0)
            {
                end = true;
                buttonPass.Enabled = false;
            }

            Invalidate();
        }

        private void redPossibilities(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    while (possible[i, j] == false)
                    {
                        /////////////Top Left
                        x = i - 1;
                        y = j - 1;

                        if (x >= 0 && y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x--;
                                y--;

                                while (x >= 0 && y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                    y--;
                                }
                            }
                        }

                        ///////////Top
                        x = i;
                        y = j - 1;

                        if (y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                y--;

                                while (y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    y--;
                                }
                            }
                        }

                        //////////Top Right
                        x = i + 1;
                        y = j - 1;

                        if (x <= 7 && y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x++;
                                y--;

                                while (x <= 7 && y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                    y--;
                                }
                            }
                        }

                        ///Right
                        x = i + 1;
                        y = j;

                        if (x <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x++;

                                while (x <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                }
                            }
                        }

                        /////////Bottom Right
                        x = i + 1;
                        y = j + 1;

                        if (x <= 7 && y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x++;
                                y++;

                                while (x <= 7 && y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                    y++;
                                }
                            }
                        }

                        /////////Bottom
                        x = i;
                        y = j + 1;

                        if (y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                y++;

                                while (y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    y++;
                                }
                            }
                        }

                        ///////Bottom Left
                        x = i - 1;
                        y = j + 1;

                        if (x >= 0 && y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x--;
                                y++;

                                while (x >= 0 && y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                    y++;
                                }
                            }
                        }
                        /////////Left
                        x = i - 1;
                        y = j;

                        if (x >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == false)
                            {
                                x--;

                                while (x >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == true)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                }
                            }
                        }

                        if (possible[i, j] == false)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void bluePossibilities(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    while (possible[i, j] == false)
                    {
                        /////////////Top Left
                        x = i - 1;
                        y = j - 1;

                        if (x >= 0 && y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x--;
                                y--;

                                while (x >= 0 && y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                    y--;
                                }
                            }
                        }

                        ///////////Top
                        x = i;
                        y = j - 1;

                        if (y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                y--;

                                while (y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    y--;
                                }
                            }
                        }

                        //////////Top Right
                        x = i + 1;
                        y = j - 1;

                        if (x <= 7 && y >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x++;
                                y--;

                                while (x <= 7 && y >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                    y--;
                                }
                            }
                        }

                        ///Right
                        x = i + 1;
                        y = j;

                        if (x <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x++;

                                while (x <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                }
                            }
                        }

                        /////////Bottom Right
                        x = i + 1;
                        y = j + 1;

                        if (x <= 7 && y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x++;
                                y++;

                                while (x <= 7 && y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x++;
                                    y++;
                                }
                            }
                        }

                        /////////Bottom

                        x = i;
                        y = j + 1;

                        if (y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                y++;

                                while (y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    y++;
                                }
                            }
                        }

                        ///////Bottom Left
                        x = i - 1;
                        y = j + 1;

                        if (x >= 0 && y <= 7)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x--;
                                y++;

                                while (x >= 0 && y <= 7)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                    y++;
                                }
                            }
                        }
                        /////////Left
                        x = i - 1;
                        y = j;

                        if (x >= 0)
                        {
                            if (filled[x, y] == true && red[x, y] == true)
                            {
                                x--;

                                while (x >= 0)
                                {
                                    if (filled[x, y] == true && red[x, y] == false)
                                    {
                                        possible[i, j] = true;
                                        break;
                                    }

                                    if (filled[x, y] == false)
                                    {
                                        break;
                                    }

                                    x--;
                                }
                            }
                        }

                        if (possible[i, j] == false)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void flippingForRed(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    flipping[i, j] = false;
                }
            }

            /////////////Top Left
            x = posX - 1;
            y = posY - 1;

            while (x >= 0 && y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x++;
                    y++;

                    while (x < posX && y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                        y++;
                    }

                    break;
                }

                x--;
                y--;
            }

            /////////////Top
            x = posX;
            y = posY - 1;

            while (y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    y++;

                    while (y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        y++;
                    }

                    break;
                }

                y--;
            }

            /////////////Top Right
            x = posX + 1;
            y = posY - 1;

            while (x <= 7 && y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x--;
                    y++;

                    while (x > posX && y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                        y++;
                    }

                    break;
                }

                x++;
                y--;
            }

            /////////////Right
            x = posX + 1;
            y = posY;

            while (x <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x--;

                    while (x > posX)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                    }

                    break;
                }

                x++;
            }

            /////////////Bottom Right
            x = posX + 1;
            y = posY + 1;

            while (x <= 7 && y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x--;
                    y--;

                    while (x > posX && y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                        y--;
                    }

                    break;
                }

                x++;
                y++;
            }

            /////////////Bottom
            x = posX;
            y = posY + 1;

            while (y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    y--;

                    while (y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        y--;
                    }

                    break;
                }

                y++;
            }

            /////////////Bottom Left
            x = posX - 1;
            y = posY + 1;

            while (x >= 0 && y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x++;
                    y--;

                    while (x < posX && y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                        y--;
                    }

                    break;
                }

                x--;
                y++;
            }

            /////////////Left
            x = posX - 1;
            y = posY;

            while (x >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == true)
                {
                    x++;

                    while (x < posX)
                    {
                        if (filled[x, y] == true && red[x, y] == false)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                    }

                    break;
                }

                x--;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (flipping[i, j] == true)
                    {
                        red[i, j] = true;
                    }
                }
            }

            Invalidate();
        }

        private void flippingForBlue(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    flipping[i, j] = false;
                }
            }

            /////////////Top Left
            x = posX - 1;
            y = posY - 1;

            while (x >= 0 && y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x++;
                    y++;

                    while (x < posX && y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                        y++;
                    }

                    break;
                }

                x--;
                y--;
            }

            /////////////Top
            x = posX;
            y = posY - 1;

            while (y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    y++;

                    while (y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        y++;
                    }

                    break;
                }

                y--;
            }

            /////////////Top Right
            x = posX + 1;
            y = posY - 1;

            while (x <= 7 && y >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x--;
                    y++;

                    while (x > posX && y < posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                        y++;
                    }

                    break;
                }

                x++;
                y--;
            }

            /////////////Right
            x = posX + 1;
            y = posY;

            while (x <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x--;

                    while (x > posX)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                    }

                    break;
                }

                x++;
            }

            /////////////Bottom Right
            x = posX + 1;
            y = posY + 1;

            while (x <= 7 && y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x--;
                    y--;

                    while (x > posX && y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x--;
                        y--;
                    }

                    break;
                }

                x++;
                y++;
            }

            /////////////Bottom
            x = posX;
            y = posY + 1;

            while (y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    y--;

                    while (y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        y--;
                    }

                    break;
                }

                y++;
            }

            /////////////Bottom Left
            x = posX - 1;
            y = posY + 1;

            while (x >= 0 && y <= 7 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x++;
                    y--;

                    while (x < posX && y > posY)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                        y--;
                    }

                    break;
                }

                x--;
                y++;
            }

            /////////////Left
            x = posX - 1;
            y = posY;

            while (x >= 0 && filled[x, y] == true)
            {
                if (filled[x, y] == true && red[x, y] == false)
                {
                    x++;

                    while (x < posX)
                    {
                        if (filled[x, y] == true && red[x, y] == true)
                        {
                            flipping[x, y] = true;
                        }

                        x++;
                    }

                    break;
                }

                x--;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (flipping[i, j] == true)
                    {
                        red[i, j] = false;
                    }
                }
            }

            Invalidate();
        }

        private void buttonPass_Click(object sender, EventArgs e)
        {
            if (AITrue == false || AIRed == true && redTurn == false || AIRed == false && redTurn == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (possible[i, j] == true)
                        {
                            possible[i, j] = false;
                        }
                    }
                }

                if (redTurn == true)
                {
                    redTurn = false;
                    bluePossibilities(sender, e);
                }

                else
                {
                    redTurn = true;
                    redPossibilities(sender, e);
                }

                if (redTurn == true && AIRed == true && AITrue == true || redTurn == false && AIRed == false && AITrue == true)
                {
                    AIGoing = true;
                    time = 3;
                    timer1.Start();
                }

                Invalidate();
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStart_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void restartAndMakeRedAnAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStart_Click(sender, e);
            groupBox1.Visible = true;

            AITrue = true;
            AIRed = true;

            if (redTurn == true)
            {
                AIGoing = true;
                time = 3;
                timer1.Start();
            }

            AIOnce = false;
            buttonStart.Enabled = false;
           // redAIAction(sender, e);
        }

        private void redAIAction(object sender, EventArgs e)
        {
            AIWent = true;

         //   redPossibilities(sender, e);

     /*       for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possible[i, j] = false;
                }
            }

            redPossibilities(sender, e); */

            shouldPass = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (possible[i, j] == true)
                    {
                        shouldPass = false;
                    }
                }
            }

            if (shouldPass == false)
            {
                aix = rand.Next(0, 8);
                aiy = rand.Next(0, 8);

                while (possible[aix, aiy] == false || filled[aix, aiy] == true)
                {
                    aix = rand.Next(0, 8);
                    aiy = rand.Next(0, 8);
                }

                posX = aix;
                posY = aiy;

                red[aix, aiy] = true;
                filled[aix, aiy] = true;
                redTurn = false;

                flippingForRed(sender, e);

                //    bluePossibilities(sender, e);

                redScore = 0;
                blueScore = 0;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        possible[i, j] = false;

                        if (filled[i, j] == true)
                        {
                            if (red[i, j] == true)
                            {
                                redScore++;
                            }

                            else
                            {
                                blueScore++;
                            }
                        }
                    }
                }

                bluePossibilities(sender, e);

                if (redScore + blueScore == 64)
                {
                    end = true;
                    buttonPass.Enabled = false;
                }

                else if (redScore == 0 || blueScore == 0)
                {
                    end = true;
                    buttonPass.Enabled = false;
                }

                AIOnce = false;
                Invalidate();
            }

            else
            {
                buttonPass_Click(sender, e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (end == false)
            {
                time--;

                if (time == 0 && redTurn == true)
                {
                    redAIAction(sender, e);
                    timer1.Stop();
                    AIGoing = false;
                }

                else if (time == 0 && redTurn == false)
                {
                    blueAIAction(sender, e);
                    timer1.Stop();
                    AIGoing = false;
                }

                Invalidate();
            }
        }

        private void blueAIAction(object sender, EventArgs e)
        {
            AIWent = true;

            //   redPossibilities(sender, e);

     /*       for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possible[i, j] = false;
                }
            }

            bluePossibilities(sender, e); */

            shouldPass = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (possible[i, j] == true)
                    {
                        shouldPass = false;
                    }
                }
            }

            if (shouldPass == false)
            {
                aix = rand.Next(0, 8);
                aiy = rand.Next(0, 8);

                while (possible[aix, aiy] == false || filled[aix, aiy] == true)
                {
                    aix = rand.Next(0, 8);
                    aiy = rand.Next(0, 8);
                }

                posX = aix;
                posY = aiy;

                red[aix, aiy] = false;
                filled[aix, aiy] = true;
                redTurn = true;

                flippingForBlue(sender, e);

                //    bluePossibilities(sender, e);

                redScore = 0;
                blueScore = 0;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        possible[i, j] = false;

                        if (filled[i, j] == true)
                        {
                            if (red[i, j] == true)
                            {
                                redScore++;
                            }

                            else
                            {
                                blueScore++;
                            }
                        }
                    }
                }

                redPossibilities(sender, e);

                if (redScore + blueScore == 64)
                {
                    end = true;
                    buttonPass.Enabled = false;
                }

                else if (redScore == 0 || blueScore == 0)
                {
                    end = true;
                    buttonPass.Enabled = false;
                }

                AIOnce = false;
                Invalidate();
            }

            else
            {
                buttonPass_Click(sender, e);
            }
        }

        private void restartAndMakeBlueAnAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStart_Click(sender, e);
            groupBox1.Visible = true;

            AITrue = true;
            AIRed = false;

            if (redTurn == false)
            {
                AIGoing = true;
                time = 3;
                timer1.Start();
            }

            AIOnce = false;
            buttonStart.Enabled = false;
        }

        private void restartWithNoAIsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AITrue = false;
            AIGoing = false;
            AIWent = false;
            buttonStart_Click(sender, e);
            buttonStart.Enabled = true;
            groupBox1.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 2000;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 500;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1;
        }
    }
}
