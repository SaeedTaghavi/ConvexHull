﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OuelletConvexHullLinear
{
	public class QuadrantSpecific3 : Quadrant
	{
		// ************************************************************************
		public QuadrantSpecific3(IReadOnlyList<Point> listOfPoint, int initialResultGuessSize) :
			base(listOfPoint, initialResultGuessSize)
		{
		}

		// ******************************************************************
		protected override void SetQuadrantLimits()
		{
			Point firstPoint = this._listOfPoint.First();

			double leftX = firstPoint.X;
			double leftY = firstPoint.Y;

			double bottomX = leftX;
			double bottomY = leftY;

			foreach (var point in _listOfPoint)
			{
				if (point.X <= leftX)
				{
					if (point.X == leftX)
					{
						if (point.Y < leftY)
						{
							leftY = point.Y;
						}
					}
					else
					{
						leftX = point.X;
						leftY = point.Y;
					}
				}

				if (point.Y <= bottomY)
				{
					if (point.Y == bottomY)
					{
						if (point.X < bottomX)
						{
							bottomX = point.X;
						}
					}
					else
					{
						bottomX = point.X;
						bottomY = point.Y;
					}
				}
			}

			FirstPoint = new Point(leftX, leftY);
			LastPoint = new Point(bottomX, bottomY);
			RootPoint = new Point(bottomX, leftY);
		}

		// ******************************************************************
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override bool IsGoodQuadrantForPoint(Point pt)
		{
			if (pt.X < this.RootPoint.X && pt.Y < this.RootPoint.Y)
			{
				return true;
			}

			return false;
		}

		// ******************************************************************
		protected override int TryAdd(Point pt)
		{
			int indexLow = 0;
			int indexHi = HullPointCount - 1;

			while (indexLow != indexHi - 1)
			{
				int index = ((indexHi - indexLow) >> 1) + indexLow;

				if (pt.X >= HullPoints[index].X && pt.Y >= HullPoints[index].Y)
				{
					return -1; // No calc needed
				}

				if (pt.X > HullPoints[index].X)
				{
					indexLow = index;
					continue;
				}

				if (pt.X < HullPoints[index].X)
				{
					indexHi = index;
					continue;
				}

				if (pt.Y < HullPoints[index].Y)
				{
					indexLow = index;
				}
				else
				{
					return -1;
				}
				break;
			}

			if (pt.Y >= HullPoints[indexLow].Y)
			{
				return -1; // Eliminated without slope calc
			}

			return indexLow;
		}

		// ************************************************************************
		protected override bool IsQuickInterpolationCandidate(Point pt)
		{
			Point[] hullPoints = HullPoints; // Need a stable copy

			int index = (int)(((pt.X - hullPoints[0].X) / (hullPoints[hullPoints.Length - 1].X - hullPoints[0].X)) * hullPoints.Length);

			if (pt.X >= hullPoints[index].X && pt.Y >= hullPoints[index].Y)
			{
				return false; // No calc needed
			}

			return true;
		}

		// ******************************************************************
	}
}
