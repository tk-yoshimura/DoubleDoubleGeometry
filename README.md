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
```

## Licence
[MIT](https://github.com/tk-yoshimura/DoubleDoubleGeometry/blob/master/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
