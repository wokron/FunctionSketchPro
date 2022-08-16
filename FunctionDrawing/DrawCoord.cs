using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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

        private void DrawXLineAt(double height)
        {
            DrawLineWithCoordPoints(
                    CoordPenSetting,
                    new Point(widthLim.from, height),
                    new Point(widthLim.to, height));
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
                start = widthLim.from - ((widthLim.from) % ScaleLength); // 烦人的负数取模...
            for (double i = start; i < widthLim.to; i += ScaleLength)
            {
                Point begin = new Point(i, y);
                DrawLineWithCoordPoints(CoordPenSetting, begin, begin + trans);
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

        private void DrawYLineAt(double width)
        {
            DrawLineWithCoordPoints(
                    CoordPenSetting,
                    new Point(width, heightLim.from),
                    new Point(width, heightLim.to));
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
                start = heightLim.from - ((heightLim.from) % ScaleLength);
            for (double i = start; i < heightLim.to; i += ScaleLength)
            {
                Point begin = new Point(x, i);
                DrawLineWithCoordPoints(CoordPenSetting, begin, begin + trans);
            }
        }
    }
}
