using Microsoft.Maui.Dispatching;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace MyResto.Pages;

public class ParticleView : SKCanvasView
{
    private readonly List<Particle> particles = new();
    private readonly Random random = new();
    private IDispatcherTimer timer;
    private float width;
    private float height;

    // Add default constructor
    public ParticleView() : base()
    {
        Initialize();
    }

    // Keep the parameterized constructor
    public ParticleView(IDispatcher dispatcher) : base()
    {
        Initialize();
    }

    private void Initialize()
    {
        timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(16); // 60fps
        timer.Tick += Timer_Tick;

        SizeChanged += OnSizeChanged;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object sender, EventArgs e)
    {
        timer?.Stop();
        SizeChanged -= OnSizeChanged;
        Unloaded -= OnUnloaded;
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        width = (float)Width;
        height = (float)Height;

        // Initialize or reset particles when size changes
        InitializeParticles();
        timer.Start();
    }

    private void InitializeParticles()
    {
        particles.Clear();
        for (int i = 0; i < 100; i++)
        {
            particles.Add(new Particle
            {
                X = random.Next(0, (int)width),
                Y = random.Next(0, (int)height),
                Speed = random.Next(1, 3),
                Size = random.Next(2, 5)
            });
        }
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        using var paint = new SKPaint
        {
            Color = new SKColor(139, 0, 0, 100), // #8B0000 with some transparency
            IsAntialias = true
        };

        foreach (var particle in particles)
        {
            canvas.DrawCircle(particle.X, particle.Y, particle.Size, paint);
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Update particle positions
        foreach (var particle in particles)
        {
            particle.Y += particle.Speed;
            if (particle.Y > height)
                particle.Y = 0;
        }
        
        InvalidateSurface();
    }
}

public class Particle
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Speed { get; set; }
    public float Size { get; set; }
}