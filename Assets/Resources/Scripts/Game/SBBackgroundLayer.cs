using UnityEngine;
using System.Collections;

public class SBBackgroundLayer : FContainer {
	public SBBackgroundLayer() {
		MakeFloor();
	}
	
	private void MakeFloor() {
		float boardWidth = 200f;
		float boardHeight = 20f;
		float padding = 2f;
		int boardRowCount = (int)((Futile.screen.height - SBConfig.TOP_UI_HEIGHT) / boardHeight);
		
		for (int i = 0; i < boardRowCount; i++) {
			float rowOffset = Random.Range(-boardWidth, 0);
			int boardColumnCount = (int)((Futile.screen.width - rowOffset) / boardWidth) + 1;
			for (int j = 0; j < boardColumnCount; j++) {
				FSprite board = WTSquareMaker.Square(boardWidth + padding * 2, boardHeight + padding * 2);
				float rand = Random.Range(-0.05f, 0.05f) + 0.25f;
				board.color = new Color(0.66f + rand, 0.55f + rand, 0.4f + rand);
				board.anchorX = 0;
				board.anchorY = 0;
				board.x = j * boardWidth + rowOffset - padding;
				board.y = i * boardHeight - padding;
				AddChild(board);
			}
		}
	}
}
