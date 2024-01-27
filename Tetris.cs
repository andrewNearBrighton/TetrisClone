using Godot;
using System;
using System.Numerics;

public partial class Tetris : Godot.TileMap

{
int currentTetronimo;
bool endRound;
Vector2I currentLocation0, tetronimoPiece0;
Vector2I currentLocation1, tetronimoPiece1;
Vector2I currentLocation2, tetronimoPiece2;
Vector2I currentLocation3, tetronimoPiece3;
Vector2I tileSource = new Vector2I (0,0);
double moveTimer;
bool movementAllowed = true;
int score = 0;

const int placedTileId = 7;




	public override void _Ready()
	{
		var ScoreLabel = GetNode<Label>("Label");
		ScoreLabel.Text = score.ToString();
		NewTetronimo();
	}

		private void _on_timer_timeout()
	{		
			endRound = false;
			int below0 = GetCellSourceId (0,(currentLocation0+Vector2I.Down));
			int below1 = GetCellSourceId (0,(currentLocation1+Vector2I.Down));
			int below2 = GetCellSourceId (0,(currentLocation2+Vector2I.Down));
			int below3 = GetCellSourceId (0,(currentLocation3+Vector2I.Down));
			
			if (below0 >=placedTileId  || below1 >=placedTileId || below2>=placedTileId || below3 >=placedTileId)
			{
				endRound = true;
			}
			
			if (endRound == false)
			{
			SetCell(0, currentLocation0, -1, tileSource,0);
			SetCell(0, currentLocation1, -1, tileSource,0);
			SetCell(0, currentLocation2, -1, tileSource,0);
			SetCell(0, currentLocation3, -1, tileSource,0);
			currentLocation0 = currentLocation0 + Vector2I.Down;
			currentLocation1 = currentLocation0 + tetronimoPiece1;
			currentLocation2 = currentLocation0 + tetronimoPiece2;
			currentLocation3 = currentLocation0 + tetronimoPiece3;
			SetCell(0, currentLocation0, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation1, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation2, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation3, currentTetronimo, tileSource,0);
			}
			else
			{
				EndRound();
			}
		}

	public void EndRound()
		{
		int clearedRows = 0;
		SetCell(0, currentLocation0, placedTileId, tileSource,0);
		SetCell(0, currentLocation1, placedTileId, tileSource,0);
		SetCell(0, currentLocation2, placedTileId, tileSource,0);
		SetCell(0, currentLocation3, placedTileId, tileSource,0);

		for (int row=21; row > 2 ;row--)
			{
			int rowSum = 0;
			for (int cols=2; cols<12 ; cols++)
				{
				rowSum +=GetCellSourceId (0,(new Vector2I(cols,row)));
				}
			if (rowSum >=70)
				{
				ClearRow(row);
				clearedRows += 1;
				row+=1;
				}
			}
		Score (clearedRows);
		NewTetronimo();
	}


	public void Score(int clearedRows)
		{
		score += (clearedRows * 100);
		GD.Print (score.ToString());
		var ScoreLabel = GetNode<Label>("Label");
		ScoreLabel.Text = score.ToString();
		}

	public void ClearRow(int row)
		{
		for (int testRow = row; testRow>3; testRow--)
		{
			for (int cols = 2; cols < 12; cols ++)
			{
				Vector2I cellBeingCleared = new Vector2I (cols, testRow);
				int cellAboveId = GetCellSourceId (0,(cellBeingCleared + Vector2I.Up));
				SetCell(0,cellBeingCleared,cellAboveId,tileSource,0);
			}
		}
	}

	public void NewTetronimo()
		{
		Random rnd = new Random();
		currentTetronimo = rnd.Next(0,5);
		GenerateTetronimo();
		currentLocation0 = new Vector2I(6,4);
		currentLocation1 = currentLocation0 + tetronimoPiece1;
		currentLocation2 = currentLocation0 + tetronimoPiece2;
		currentLocation3 = currentLocation0 + tetronimoPiece3;

		SetCell(0, currentLocation0, currentTetronimo, tileSource,0);
		SetCell(0, currentLocation1, currentTetronimo, tileSource,0);
		SetCell(0, currentLocation2, currentTetronimo, tileSource,0);
		SetCell(0, currentLocation3, currentTetronimo, tileSource,0);
			
		int below0 = GetCellSourceId (0,(currentLocation0+Vector2I.Down));
		int below1 = GetCellSourceId (0,(currentLocation1+Vector2I.Down));
		int below2 = GetCellSourceId (0,(currentLocation2+Vector2I.Down));
		int below3 = GetCellSourceId (0,(currentLocation3+Vector2I.Down));

		if (below0 >=placedTileId || below1 >=placedTileId || below2 >=placedTileId || below3 >=placedTileId)
			{
			var FailLabel =GetNode<Label>("Fail");
			FailLabel.Text = "Fail";
			var Timer = GetNode<Timer>("Timer");
			Timer.Stop();
			}
	

		}

	public void GenerateTetronimo()
		{
			switch (currentTetronimo)
			{
			case 0:
			tetronimoPiece1 = new Vector2I (0,-2);
			tetronimoPiece2 = new Vector2I (0,-1);
			tetronimoPiece3 = new Vector2I (0,1);
			break;

			case 1:
			tetronimoPiece1 = new Vector2I (0,-2);
			tetronimoPiece2 = new Vector2I (0,-1);
			tetronimoPiece3 = new Vector2I (1,0);
			break;

			case 2:
			tetronimoPiece1 = new Vector2I (1,-1);
			tetronimoPiece2 = new Vector2I (0,-1);
			tetronimoPiece3 = new Vector2I (1,0);
			break;

			case 3:
			tetronimoPiece1 = new Vector2I (-1,0);
			tetronimoPiece2 = new Vector2I (0,1);
			tetronimoPiece3 = new Vector2I (1,1);
			break;

			default:
			tetronimoPiece1 = new Vector2I (0,-1);
			tetronimoPiece2 = new Vector2I (-1,0);
			tetronimoPiece3 = new Vector2I (1,0);
			break;

			}
		}

	public override void _Process(double delta)
		{
		var movementTimer = GetNode<Timer>("Timer");

		if (Input.IsActionPressed("ui_right") == true)
			{
			MoveRight();	
			}
	
		if (Input.IsActionPressed("ui_left") == true)
			{
			MoveLeft();	
			}
		if (Input.IsActionPressed("ui_up") == true)
			{
			RotateTetronimo();
			}
		if (Input.IsActionPressed("ui_down")==true)
			{
			movementTimer.WaitTime = 0.1;
			}
		else
			{
			movementTimer.WaitTime = 0.4;
			}
		}

	public void MoveRight()
		{
		int right0 = GetCellSourceId (0,(currentLocation0+Vector2I.Right));
		int right1 = GetCellSourceId (0,(currentLocation1+Vector2I.Right));
		int right2 = GetCellSourceId (0,(currentLocation2+Vector2I.Right));
		int right3 = GetCellSourceId (0,(currentLocation3+Vector2I.Right));

		if (movementAllowed == true && right0 <placedTileId && right1 <placedTileId && right2 <placedTileId && right3 <placedTileId)
			{
			SetCell(0, currentLocation0, -1, tileSource,0);
			SetCell(0, currentLocation1, -1, tileSource,0);
			SetCell(0, currentLocation2, -1, tileSource,0);
			SetCell(0, currentLocation3, -1, tileSource,0);
			currentLocation0 += Vector2I.Right;
			currentLocation1 += Vector2I.Right;
			currentLocation2 += Vector2I.Right;
			currentLocation3 += Vector2I.Right;
			SetCell(0, currentLocation0, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation1, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation2, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation3, currentTetronimo, tileSource,0);
			movementAllowed = false;
			}
		}


	public void MoveLeft()
		{
		int left0 = GetCellSourceId (0,(currentLocation0+Vector2I.Left));
		int left1 = GetCellSourceId (0,(currentLocation1+Vector2I.Left));
		int left2 = GetCellSourceId (0,(currentLocation2+Vector2I.Left));
		int left3 = GetCellSourceId (0,(currentLocation3+Vector2I.Left));

		if (movementAllowed == true && left0 <placedTileId && left1 <placedTileId && left2 <placedTileId && left3 <placedTileId)
			{
			SetCell(0, currentLocation0, -1, tileSource,0);
			SetCell(0, currentLocation1, -1, tileSource,0);
			SetCell(0, currentLocation2, -1, tileSource,0);
			SetCell(0, currentLocation3, -1, tileSource,0);
			currentLocation0 += Vector2I.Left;
			currentLocation1 += Vector2I.Left;
			currentLocation2 += Vector2I.Left;
			currentLocation3 += Vector2I.Left;
			SetCell(0, currentLocation0, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation1, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation2, currentTetronimo, tileSource,0);
			SetCell(0, currentLocation3, currentTetronimo, tileSource,0);
			movementAllowed = false;
			}
		}

	public void RotateTetronimo()
		{
		if (movementAllowed == true)
			{
/*			SetCell(0, currentLocation0, -1, tileSource,0);
			SetCell(0, currentLocation1, -1, tileSource,0);
			SetCell(0, currentLocation2, -1, tileSource,0);
			SetCell(0, currentLocation3, -1, tileSource,0);
*/
			Godot.Vector2 tetronimoPiece1Rotation = new Godot.Vector2 (tetronimoPiece1.X, tetronimoPiece1.Y);
			Godot.Vector2 tetronimoPiece2Rotation = new Godot.Vector2 (tetronimoPiece2.X, tetronimoPiece2.Y);
			Godot.Vector2 tetronimoPiece3Rotation = new Godot.Vector2 (tetronimoPiece3.X, tetronimoPiece3.Y);

			tetronimoPiece1Rotation = tetronimoPiece1Rotation.Rotated((3.142f/2));
			tetronimoPiece2Rotation = tetronimoPiece2Rotation.Rotated((3.142f/2));
			tetronimoPiece3Rotation = tetronimoPiece3Rotation.Rotated((3.142f/2));
			
			int tetronimoPiece1X = (int) Math.Round(tetronimoPiece1Rotation.X);
			int tetronimoPiece1Y = (int) Math.Round(tetronimoPiece1Rotation.Y);
			int tetronimoPiece2X = (int) Math.Round(tetronimoPiece2Rotation.X);
			int tetronimoPiece2Y = (int) Math.Round(tetronimoPiece2Rotation.Y);
			int tetronimoPiece3X = (int) Math.Round(tetronimoPiece3Rotation.X);
			int tetronimoPiece3Y = (int) Math.Round(tetronimoPiece3Rotation.Y);
				
			var testTetronimoPiece1 = new Vector2I (tetronimoPiece1X,tetronimoPiece1Y);
			var testTetronimoPiece2 = new Vector2I (tetronimoPiece2X,tetronimoPiece2Y);
			var testTetronimoPiece3 = new Vector2I (tetronimoPiece3X,tetronimoPiece3Y);

			int newCellID1 = GetCellSourceId (0, (currentLocation0 + testTetronimoPiece1));
			int newCellID2 = GetCellSourceId (0, (currentLocation0 + testTetronimoPiece2));	
			int newCellID3 = GetCellSourceId (0, (currentLocation0 + testTetronimoPiece3));

			if (newCellID1 < placedTileId && newCellID2 < placedTileId && newCellID3 < placedTileId)
				{
				
				tetronimoPiece1 = testTetronimoPiece1;
				tetronimoPiece2 = testTetronimoPiece2;
				tetronimoPiece3 = testTetronimoPiece3;

				SetCell(0, currentLocation0, -1, tileSource,0);
				SetCell(0, currentLocation1, -1, tileSource,0);
				SetCell(0, currentLocation2, -1, tileSource,0);
				SetCell(0, currentLocation3, -1, tileSource,0);
			
				currentLocation1 = currentLocation0 + tetronimoPiece1;
				currentLocation2 = currentLocation0 + tetronimoPiece2;
				currentLocation3 = currentLocation0 + tetronimoPiece3;
				
				SetCell(0, currentLocation0, currentTetronimo, tileSource,0);
				SetCell(0, currentLocation1, currentTetronimo, tileSource,0);
				SetCell(0, currentLocation2, currentTetronimo, tileSource,0);
				SetCell(0, currentLocation3, currentTetronimo, tileSource,0);
				
				movementAllowed =false;
				}
/*			

			if (currentLocation0.X > 11 || currentLocation1.X >11 || currentLocation2.X > 11 || currentLocation3.X >11)
				{
				currentLocation0 = currentLocation0 + Vector2I.Left;
				currentLocation1 = currentLocation1 + Vector2I.Left;
				currentLocation2 = currentLocation2 + Vector2I.Left;
				currentLocation3 = currentLocation3 + Vector2I.Left;
				}
			if (currentLocation0.X > 11 || currentLocation1.X >11 || currentLocation2.X > 11 || currentLocation3.X >11)
				{
				currentLocation0 = currentLocation0 + Vector2I.Left;
				currentLocation1 = currentLocation1 + Vector2I.Left;
				currentLocation2 = currentLocation2 + Vector2I.Left;
				currentLocation3 = currentLocation3 + Vector2I.Left;
				}
			if (currentLocation0.X < 2 || currentLocation1.X < 2 || currentLocation2.X < 2 || currentLocation3.X < 2)
				{
				currentLocation0 = currentLocation0 + Vector2I.Right;
				currentLocation1 = currentLocation1 + Vector2I.Right;
				currentLocation2 = currentLocation2 + Vector2I.Right;
				currentLocation3 = currentLocation3 + Vector2I.Right;
				}
			if (currentLocation0.X < 2 || currentLocation1.X < 2 || currentLocation2.X < 2 || currentLocation3.X < 2)
				{
				currentLocation0 = currentLocation0 + Vector2I.Right;
				currentLocation1 = currentLocation1 + Vector2I.Right;
				currentLocation2 = currentLocation2 + Vector2I.Right;
				currentLocation3 = currentLocation3 + Vector2I.Right;
				}
			if (currentLocation0.X < 2 || currentLocation1.X < 2 || currentLocation2.X < 2 || currentLocation3.X < 2)
				{
				currentLocation0 = currentLocation0 + Vector2I.Right;
				currentLocation1 = currentLocation1 + Vector2I.Right;
				currentLocation2 = currentLocation2 + Vector2I.Right;
				currentLocation3 = currentLocation3 + Vector2I.Right;
				}
			if (currentLocation0.Y >20 || currentLocation1.Y >20 || currentLocation2.Y >20 || currentLocation3.Y >20)
				{
				currentLocation0 = currentLocation0 + Vector2I.Up;
				currentLocation1 = currentLocation1 + Vector2I.Up;
				currentLocation2 = currentLocation2 + Vector2I.Up;
				currentLocation3 = currentLocation3 + Vector2I.Up;
				}
			if (currentLocation0.Y >20 || currentLocation1.Y >20 || currentLocation2.Y >20 || currentLocation3.Y >20)
				{
				currentLocation0 = currentLocation0 + Vector2I.Up;
				currentLocation1 = currentLocation1 + Vector2I.Up;
				currentLocation2 = currentLocation2 + Vector2I.Up;
				currentLocation3 = currentLocation3 + Vector2I.Up;
				}
*/
		}
			
	}


	public void _on_timer_2_timeout()
		{
		movementAllowed = true;
		}
	
}
