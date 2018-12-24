using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Script : MonoBehaviour {

	public int column;
	public int row;
	public int previousColumn;
	public int previousRow;
	public int targetX;
	public int targetY;
	public float angle = 0;
	public bool isMatch = false;
	private Board_Script board;
	private GameObject otherCat;
	private Vector2 FirstPosition;
	private Vector2 FinalPosition;
	private Vector2 TempPosition;


	// Use this for initialization
	void Start () {
		board = FindObjectOfType<Board_Script>();
		targetX = (int)transform.position.x;
		targetY = (int)transform.position.y;
		row = targetY;
		column = targetX;
		previousRow = row;
		previousColumn = column;
	}
	
	// Update is called once per frame
	void Update () {
		FindMatches();
		if(isMatch){
			SpriteRenderer MySprite = GetComponent<SpriteRenderer>();
			MySprite.color = new Color(0f,0f,0f,0.2f);
		}
		targetX = column;
		targetY = row;
		if(Mathf.Abs(targetX - transform.position.x) > 0.1) {
			TempPosition = new Vector2(targetX, transform.position.y);
			transform.position = Vector2.Lerp(transform.position, TempPosition, 0.4f);
		}
		if(board.allCats[column,row] != this.gameObject) {
			board.allCats[column,row] = this.gameObject;
		}
		else {
			TempPosition = new Vector2(targetX, transform.position.y);
			transform.position = TempPosition;
		}
		if(Mathf.Abs(targetY - transform.position.y) > 0.1) {
			TempPosition = new Vector2(transform.position.x, targetY);
			transform.position = Vector2.Lerp(transform.position, TempPosition, 0.4f);
		}
		else {
			TempPosition = new Vector2(transform.position.x, targetY);
			transform.position = TempPosition;
			board.allCats[column,row] = this.gameObject;
		}
	}

public IEnumerator CheckMoveCorout(){
	yield return new WaitForSeconds(0.5f);
	if(otherCat != null){
		if(!isMatch && !otherCat.GetComponent<Cat_Script>().isMatch){
			otherCat.GetComponent<Cat_Script>().row = row;
			otherCat.GetComponent<Cat_Script>().column = column;
			row = previousRow;
			column = previousColumn;
		}else{
		board.DestroyMatch();
		}
		otherCat = null;

	}
}
	private void OnMouseDown() {
		FirstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseUp() {
		FinalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		CalculateAngle();
	}

	void CalculateAngle() {
		angle = Mathf.Atan2(FinalPosition.y - FirstPosition.y, FinalPosition.x - FirstPosition.x) * 180 / Mathf.PI ;
		MoveCats();
	}

	void MoveCats() {
		//right side
		if(angle > -45 && angle <= 45 && column < board.width - 1) {
		otherCat = board.allCats[column + 1, row];
		otherCat.GetComponent<Cat_Script>().column -=1;
		column += 1;
		} 
		//up side
		else if(angle > 45 && angle <= 135 && row < board.height - 1) {
		otherCat = board.allCats[column, row + 1];
		otherCat.GetComponent<Cat_Script>().row -=1;
		row += 1;
		} 
		// left side
		else if((angle > 135 || angle <= -135) && column > 0) {
		otherCat = board.allCats[column - 1, row];
		otherCat.GetComponent<Cat_Script>().column +=1;
		column -= 1;
		} 
		//down side
		else if((angle < -45 && angle >= -135) && row > 0) {
		otherCat = board.allCats[column, row - 1];
		otherCat.GetComponent<Cat_Script>().row +=1;
		row -= 1;
		} 
		StartCoroutine(CheckMoveCorout());
	}

	void FindMatches() {
		if(column > 0 && column < board.width - 1 ) {
			GameObject leftCat1 = board.allCats[column -1, row];
			GameObject rightCat1 = board.allCats[column +1, row];
			if(leftCat1 != null && rightCat1 != null){
			if(leftCat1.tag == this.gameObject.tag && rightCat1.tag == this.gameObject.tag) {
				leftCat1.GetComponent<Cat_Script>().isMatch = true;
				rightCat1.GetComponent<Cat_Script>().isMatch = true;
				isMatch = true;
			}
			}
		}

		if(row > 0 && row < board.height - 1 ) {
			GameObject upCat1 = board.allCats[column, row + 1];
			GameObject downCat1 = board.allCats[column, row - 1];
			if(upCat1 != null && downCat1 != null) {
			if(upCat1.tag == this.gameObject.tag && downCat1.tag == this.gameObject.tag) {
				upCat1.GetComponent<Cat_Script>().isMatch = true;
				downCat1.GetComponent<Cat_Script>().isMatch = true;
				isMatch = true;
			}
			}
		}

	}
}
