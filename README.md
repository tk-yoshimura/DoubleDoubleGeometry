# DoubleDoubleGeometry
 Double-Double Geometry 2D/3D Implements 

## Requirement
 .NET 8.0
 
 ## Install
[Download DLL](https://github.com/tk-yoshimura/DoubleDoubleGeometry/releases)  
[Download nuget](https://www.nuget.org/packages/TYoshimura.DoubleDouble.Geometry/)

## Usage

```csharp
Line2D l = Line2D.FromDirection((2, 3), (1, 1));
Sphere3D s = Sphere3D.FromIntersection((3, 2, -1), (1, 3, -2), (3, -1, -4), (0, 0, -2));
Polyhedron3D p = Polyhedron3D.Icosahedron;

Polyhedron3D.IsConvex(p);
ddouble volume = p.Volume;
ddouble area = p.Area;
bool inside = p.Inside((0, 0, 0));

Matrix3D matrix = Matrix3D.RotateAxis((1, 2, 3), 4);
Vector3D v0 = (3, 4, 0), v1 = (0, 3, -4);
Line3D line = matrix * Line3D.FromIntersection(v0, v1);
Sphere3D sphere = new(matrix * Vector3D.Zero, 5);
(Vector3D v, ddouble t)[] cross = Intersect3D.LineSphere(line, sphere);
```

## Licence
[MIT](https://github.com/tk-yoshimura/DoubleDoubleGeometry/blob/master/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
