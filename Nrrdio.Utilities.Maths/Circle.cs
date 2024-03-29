﻿using System.Diagnostics;

namespace Nrrdio.Utilities.Maths;

public class Circle {
	public Point Center { get; private set; }

	public double Radius {
		get => _Radius;
		private set {
			if (_Radius != value) {
				_Radius = value;
				RadiusSquared = Math.Pow(value, 2);
			}
		}
	}
	double _Radius;

	public double RadiusSquared { get; private set; }

	public Circle(Point center, double radius) {
		Radius = radius;
		Center = center;
	}

	public Circle(Point point1, Point point2) {
		Center = point1.Lerp(point2, .5);
		Radius = Math.Max(Center.Distance(point1), Center.Distance(point2));
	}

	public Circle(IEnumerable<Point> points, Point centroid) {
		if (points.Count() < 2 || centroid is null) {
			throw new Exception("A circle requires at least three points and a centroid.");
		}
		else {
			// Copy the list so we don't mess up the order.
			var circlePoints = new List<Point>(points.Distinct());

			// Take the 3 furthest points from a centroid and ignore all the internal points. This helps with generating circumcircles.
			circlePoints = circlePoints.OrderByDescending(p => centroid.Distance(p)).ToList();

            var colinear = true;

            // Degenerate case: All 3 furthest points are colinear
            while (colinear) {
                // The first two points are naturally the furthest, so check the 3rd point for colinearity
                var segment = new Segment(circlePoints[0], circlePoints[1]);
                var nearLine = circlePoints[2].NearLine(segment);

                if (circlePoints[2].OnLine(segment)) {
                    circlePoints.RemoveAt(2);
                }
                else {
                    colinear = false;
                }

                if (circlePoints.Count < 3) {
                    throw new Exception("Too many points were colinear");
                }
            }

            FromCircumcircle(circlePoints[0], circlePoints[1], circlePoints[2]);
		}
	}

	public Polygon SuperTriangle() {
		// relative vectors
		var mp1x = Radius * Math.Cos(Formula.DegreesToRadians(60d));
		var mp1y = Radius * Math.Sin(Formula.DegreesToRadians(60d));
		var mp2x = Radius * Math.Cos(Formula.DegreesToRadians(180d));
		var mp2y = Radius * Math.Sin(Formula.DegreesToRadians(180d));
		var mp3x = Radius * Math.Cos(Formula.DegreesToRadians(300d));
		var mp3y = Radius * Math.Sin(Formula.DegreesToRadians(300d));

		// midpoints
		var mp1 = Center + new Point(mp1x, mp1y);
		var mp2 = Center + new Point(mp2x, mp2y);
		var mp3 = Center + new Point(mp3x, mp3y);

		// outer triangle vertices
		var p1x = mp1.X + mp3.X - mp2.X;
		var p1y = mp1.Y + mp3.Y - mp2.Y;
		var p2x = mp1.X + mp2.X - mp3.X;
		var p2y = mp1.Y + mp2.Y - mp3.Y;
		var p3x = mp2.X + mp3.X - mp1.X;
		var p3y = mp2.Y + mp3.Y - mp1.Y;

		var point1 = new Point(p1x, p1y);
		var point2 = new Point(p2x, p2y);
		var point3 = new Point(p3x, p3y);

		return new Polygon(point1, point2, point3);
	}

    public bool Contains(Point point) => Center.Distance(point) <= Radius;

    public Polygon ToPolygon(int sides = 3, int segmentsPerSide = 1) {
		if (sides < 3) {
			throw new ArgumentException("Requires at least 3 sides");
		}

        if (segmentsPerSide < 1) {
            throw new ArgumentException("Requires at least 1 segment per side");
        }

		var spacing = Math.PI * 2d / sides;
        var lerpBy = 1d / segmentsPerSide;

        double x;
		double y;
		Point point;
		Point offset;
        Point previousPoint = default;

		var points = new List<Point>();

		var theta = 0d;

        for (var i = 0; i <= sides; i++) {
			x = Radius * Math.Cos(theta);
			y = Radius * Math.Sin(theta);
			theta += spacing;

			offset = new Point(x, y);
			point = Center + offset;

            if (previousPoint is not null) {
                addSegmentedSide(previousPoint, point);
            }

            previousPoint = point;
		}

		return new Polygon(points);

        void addSegmentedSide(Point sideVertex1, Point sideVertex2) {
            for (var i = 0; i < segmentsPerSide; i++) {
                points.Add(sideVertex1.Lerp(sideVertex2, lerpBy * i));
            }
        }
    }

	void FromCircumcircle(Point p0, Point p1, Point p2) {
		// https://codefound.wordpress.com/2013/02/21/how-to-compute-a-circumcircle/#more-58
		// https://en.wikipedia.org/wiki/Circumscribed_circle

		var dA = p0.Dot(p0);
		var dB = p1.Dot(p1);
		var dC = p2.Dot(p2);

		var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
		var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));

		var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

        Debug.Assert(div != 0, "All 3 points are colinear");

        var centerX = aux1 / div;
        var centerY = aux2 / div;

        Center = new Point(centerX, centerY);
		Radius = Math.Max(Math.Max(Center.Distance(p0), Center.Distance(p1)), Center.Distance(p2));
	}
}