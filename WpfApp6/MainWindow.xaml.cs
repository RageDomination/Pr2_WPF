using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;

        class Snowflake
        {
            public ModelVisual3D model;
            public double x, y, z;
        }

        List<Snowflake> snow = new List<Snowflake>();

        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 30; i++)
                snow.Add(CreateSnowflake());

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += Timer_Tick;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private Snowflake CreateSnowflake()
        {
            double x = rnd.NextDouble() * 6 - 3;
            double y = rnd.NextDouble() * 5;
            double z = rnd.NextDouble() * 4 - 2;

            double size = rnd.NextDouble() * 0.05 + 0.03;

            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(new Point3D(-size, -size, 0));
            mesh.Positions.Add(new Point3D(size, -size, 0));
            mesh.Positions.Add(new Point3D(size, size, 0));
            mesh.Positions.Add(new Point3D(-size, size, 0));

            mesh.TriangleIndices = new Int32Collection() { 0, 1, 2, 2, 3, 0 };

            MaterialGroup material = new MaterialGroup();

            material.Children.Add(
                new DiffuseMaterial(
                    new RadialGradientBrush(
                        Color.FromRgb(255, 255, 255),
                        Color.FromRgb(180, 180, 180))
                )
            );

            material.Children.Add(
                new SpecularMaterial(
                    Brushes.White,
                    30)
            );

            GeometryModel3D model = new GeometryModel3D(mesh, material);

            ModelVisual3D visual = new ModelVisual3D();
            visual.Content = model;

            TranslateTransform3D move = new TranslateTransform3D(x, y, z);
            visual.Transform = move;

            SnowContainer.Children.Add(visual);

            return new Snowflake
            {
                model = visual,
                x = x,
                y = y,
                z = z
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var s in snow)
            {
                s.y -= 0.05;

                if (s.y < -2)
                    s.y = 5;

                ((TranslateTransform3D)s.model.Transform).OffsetY = s.y;
            }
        }
    }
}