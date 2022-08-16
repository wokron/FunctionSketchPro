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
            if (heightLim.Contains(0))
            {
                DrawXLineAt(0);
                DrawXScaleAt(0);
            }
            else if (0 < heightLim.from)
            {
                DrawXLineAt(heightLim.from);
                DrawXScaleAt(heightLim.from);
            }
            else
            {
                DrawXLineAt(heightLim.to);
                DrawXScaleAt(heightLim.to, isReverse: true);
            }
        }

        private void DrawXLineAt(double y)
        {
            DrawLineWithCoordPoints(
                CoordPenSetting,
                new Point(widthLim.from, y),
                new Point(widthLim.to, y));
        }

        private void DrawXScaleAt(double y, bool isReverse = false)
        {
            Vector trans = new Vector(0, ScaleLength / 6d);
            if (isReverse)
                trans = -1d * trans;
            double start;
            if (widthLim.from > 0)
                start = widthLim.from + ((widthLim.from + ScaleLength) % ScaleLength);
            else
                start = widthLim.from - (widthLim.from % ScaleLength); // 烦人的负数取模...
            for (double i = start; i < widthLim.to; i += ScaleLength)
            {
                Point begin = new Point(i, y);
                DrawLineWithCoordPoints(CoordPenSetting, begin, begin + trans);
                DrawTextAt($"{i:N2}", begin);
            }
        }

        private void DrawMainCoordY()
        {
            if (widthLim.Contains(0))
            {
                DrawYLineAt(0);
                DrawYScaleAt(0);
            }
            else if (0 < widthLim.from)
            {
                DrawYLineAt(widthLim.from);
                DrawYScaleAt(widthLim.from);
            }
            else
            {
                DrawYLineAt(widthLim.to);
                DrawYScaleAt(widthLim.to, isReverse: true);
            }
        }

        private void DrawYLineAt(double x)
        {
            DrawLineWithCoordPoints(
                CoordPenSetting,
                new Point(x, heightLim.from),
                new Point(x, heightLim.to));
        }

        private void DrawYScaleAt(double x, bool isReverse = false)
        {
            Vector trans = new Vector(ScaleLength / 6d, 0);
            if (isReverse)
                trans = -1d * trans;
            double start;
            if (heightLim.from > 0)
                start = heightLim.from + ((heightLim.from + ScaleLength) % ScaleLength);
            else
                start = heightLim.from - (heightLim.from % ScaleLength);
            for (double i = start; i < heightLim.to; i += ScaleLength)
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
            if (p.X + numsTextSetting.Width > widthLim.to)
                move += new Vector(widthLim.to - (p.X + numsTextSetting.Width), 0);
            if (p.Y - numsTextSetting.Height < heightLim.from)
                move += new Vector(0, heightLim.from - (p.Y - numsTextSetting.Height));

            DrawTextWithCoordPoint(numsTextSetting, p + move);
        }

        private void DrawTextWithCoordPoint(FormattedText formattedText, Point p)
        {
            Matrix trans = new Matrix(1, 0, 0, -1, 0, 0);
            brush.DrawText(formattedText, p * trans);
        }
    }
}
