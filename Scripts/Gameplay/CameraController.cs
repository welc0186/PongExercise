using Godot;
using System;

public partial class CameraController : Camera2D
{
	
	public const float BASE_NOISE_STRENGTH = 20;
	public const float NOISE_DECAY = 10;
	
	FastNoiseLite _noise;
	int _noisePosition;
	float _noiseStrength;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ProcessMode = ProcessModeEnum.Always;
		_noise = new();
		_noisePosition = 0;
		_noiseStrength = 0;
		RandomNumberGenerator rng = new();
		rng.Randomize();
		_noise.Seed = (int) rng.Randi();
		_noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		_noise.Frequency = 0.5f;
		Events.onGoalEnter.Subscribe(Shake);
	}

	public override void _ExitTree()
	{
		Events.onGoalEnter.Unsubscribe(Shake);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		_noiseStrength = Lerp(_noiseStrength, 0, NOISE_DECAY * (float) delta);
		this.Offset = GetNoiseOffset() * _noiseStrength;
	}

	float Lerp(float start, float end, float by)
	{
		return start * (1 - by) + end * by;
	}

	void Shake(Node node, object data)
	{
		_noiseStrength = BASE_NOISE_STRENGTH;
	}

	Vector2 GetNoiseOffset()
	{
		_noisePosition++;
		return new Vector2(
			_noise.GetNoise2D(1, _noisePosition),
			_noise.GetNoise2D(100, _noisePosition)
		);
	}


}
