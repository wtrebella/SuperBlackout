using UnityEngine;
using System.Collections;

public class SBBackgroundLayer : FContainer {
	public SBBackgroundLayer() {
		MakeFloor();
		MakeBorder();
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
	
	private void MakeBorder() {
		float borderPieceWidth = 120f;
		int horizontalPieceCount = (int)(Futile.screen.width / borderPieceWidth) + 1;
		int verticalPieceCount = (int)(Futile.screen.height / borderPieceWidth) - 1;
		
		for (int i = horizontalPieceCount / 2; i >= 0; i--) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 1;
			borderPiece.rotation = 90;
			borderPiece.x = Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - borderPieceWidth * i;
			borderPiece.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
			AddChild(borderPiece);
		}
		
		for (int i = 0; i < horizontalPieceCount / 2f; i++) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 0;
			borderPiece.rotation = 90;
			borderPiece.x = Futile.screen.halfWidth + SBConfig.BATHROOM_WIDTH / 2f + borderPieceWidth * i;
			borderPiece.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
			AddChild(borderPiece);
		}
		
		for (int i = horizontalPieceCount / 2; i >= 0; i--) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 1;
			borderPiece.rotation = -90;
			borderPiece.x = Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - borderPieceWidth * i;
			borderPiece.y = 0;
			AddChild(borderPiece);
		}
		
		for (int i = 0; i < horizontalPieceCount / 2f; i++) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 0;
			borderPiece.rotation = -90;
			borderPiece.x = Futile.screen.halfWidth + SBConfig.BATHROOM_WIDTH / 2f + borderPieceWidth * i;
			borderPiece.y = 0;
			AddChild(borderPiece);
		}
		
		for (int i = 0; i < verticalPieceCount; i++) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 0;
			borderPiece.x = 0;
			borderPiece.y = i * borderPieceWidth;
			AddChild(borderPiece);
		}
		
		for (int i = 0; i < verticalPieceCount; i++) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 1;
			borderPiece.rotation = 180;
			borderPiece.x = Futile.screen.width;
			borderPiece.y = i * borderPieceWidth;
			AddChild(borderPiece);
		}
		
		FSprite corner1 = new FSprite("borderBarPieceCurved.psd");
		corner1.anchorX = 0;
		corner1.anchorY = 1;
		corner1.x = 0;
		corner1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
		AddChild(corner1);
		
		FSprite corner2 = new FSprite("borderBarPieceCurved.psd");
		corner2.anchorX = 0;
		corner2.anchorY = 1;
		corner2.rotation = 90;
		corner2.x = Futile.screen.width;
		corner2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
		AddChild(corner2);
		
		FSprite corner3 = new FSprite("borderBarPieceCurved.psd");
		corner3.anchorX = 0;
		corner3.anchorY = 1;
		corner3.rotation = 180;
		corner3.x = Futile.screen.width;
		corner3.y = 0;
		AddChild(corner3);
		
		FSprite corner4 = new FSprite("borderBarPieceCurved.psd");
		corner4.anchorX = 0;
		corner4.anchorY = 1;
		corner4.rotation = 270;
		corner4.x = 0;
		corner4.y = 0;
		AddChild(corner4);
		
		FSprite bathroom1 = new FSprite("bathroomTop.psd");
		bathroom1.x = Futile.screen.halfWidth;
		bathroom1.anchorY = 1;
		bathroom1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
		AddChild(bathroom1);
		
		FSprite bathroom2 = new FSprite("bathroomBottom.psd");
		bathroom2.x = Futile.screen.halfWidth;
		bathroom2.anchorY = 0;
		bathroom2.y = 0;
		AddChild(bathroom2);	
	}
}
