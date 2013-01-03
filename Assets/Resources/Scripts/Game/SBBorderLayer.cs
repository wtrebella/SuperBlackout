using UnityEngine;
using System.Collections;

public class SBBorderLayer : FContainer {
	public SBBorderLayer() {
		MakeBorder();
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
			borderPiece.anchorY = 0;
			borderPiece.rotation = -90;
			borderPiece.x = Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - borderPieceWidth * i;
			borderPiece.y = 0;
			AddChild(borderPiece);
		}
		
		for (int i = 0; i < horizontalPieceCount / 2f; i++) {
			FSprite borderPiece = new FSprite("borderBarPieceStraight.psd");
			borderPiece.anchorX = 0;
			borderPiece.anchorY = 1;
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
	}
}
