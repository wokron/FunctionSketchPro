using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private double scaleNumForWidth = 20;

        private double ScaleLength { get => UnitNumForWidth / scaleNumForWidth; }
        private void DrawCoord()
        {
            DrawMainCoordX();
            DrawMainCoordY();
        }

        private void DrawMainCoordX()
        {
            if (HeightLim.Contains(0))
            {
                DrawXLineAt(0);
                DrawXScaleAt(0);
            }
            else if (0 < HeightLim.from)
            {
                DrawXLineAt(HeightLim.from);
                DrawXScaleAt(HeightLim.from);
            }
            else
            {
                DrawXLineAt(HeightLim.to);
                DrawXScaleAt(HeightLim.to, isReverse: true);
            }
        }

        private void DrawXLineAt(double y)
        {
            DrawLineWithCoordPoints(
                CoordPenSetting,
                new Point(WidthLim.from, y),
                new Point(WidthLim.to, y));
        }

        private void DrawXScaleAt(double y, bool isReverse = false)
        {
            Vector trans = new Vector(0, ScaleLength / 6d);
            if (isReverse)
                trans = -1d * trans;
            double start;
            if (WidthLim.from > 0)
                start = WidthLim.from + (ScaleLength - WidthLim.from % ScaleLength);
            else
                start = WidthLim.from - (WidthLim.from % ScaleLength); // 烦人的负数取模...
            for (double i = start; i < WidthLim.to; i += ScaleLength)
            {
                Point begin = new Point(i, y);
                DrawLineWithCoordPoints(CoordPenSetting, begin, begin + trans);
                DrawTextAt($"{i:N2}", begin);
            }
        }

        private void DrawMainCoordY()
        {
            if (WidthLim.Contains(0))
            {
                DrawYLineAt(0);
                DrawYScaleAt(0);
            }
            else if (0 < WidthLim.from)
            {
                DrawYLineAt(WidthLim.from);
                DrawYScaleAt(WidthLim.from);
            }
            else
            {
                DrawYLineAt(WidthLim.to);
                DrawYScaleAt(WidthLim.to, isReverse: true);
            }
        }

        private void DrawYLineAt(double x)
        {
            DrawLineWithCoordPoints(
                CoordPenSetting,
                new Point(x, HeightLim.from),
                new Point(x, HeightLim.to));
        }

        private void DrawYScaleAt(double x, bool isReverse = false)
        {
            Vector trans = new Vector(ScaleLength / 6d, 0);
            if (isReverse)
                trans = -1d * trans;
            double start;
            if (HeightLim.from > 0)
                start = HeightLim.from + (ScaleLength - HeightLim.from % ScaleLength);
            else
                start = HeightLim.from - (HeightLim.from % ScaleLength);
            for (double i = start; i < HeightLim.to; i += ScaleLength)
            {
                Point begin = new Point(x, i);
                DrawLineWithCoordPoints(CoordPenSetting, begin, begin + trans);
                if (i < -ScaleLength/2d || i > ScaleLength/2d)
                    DrawTextAt($"{i:N2}", begin);
            }
        }

        private void DrawTextAt(string text ,Point p)
        {
            var numsTextSetting = new FormattedText(
                text, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Times New Roman"),
                ScaleLength / 3d, Brushes.Gray,
                1.25);

            Vector move = new Vector(0, 0);
            if (p.X + numsTextSetting.Width > WidthLim.to)
                move += new Vector(WidthLim.to - (p.X + numsTextSetting.Width), 0);
            if (p.Y - numsTextSetting.Height < HeightLim.from)
                move += new Vector(0, HeightLim.from - (p.Y - numsTextSetting.Height));

            DrawTextWithCoordPoint(numsTextSetting, p + move);
        }

        private void DrawTextWithCoordPoint(FormattedText formattedText, Point p)
        {
            Matrix trans = new Matrix(1, 0, 0, -1, 0, 0);
            brush.DrawText(formattedText, p * trans);
        }
    }
}
