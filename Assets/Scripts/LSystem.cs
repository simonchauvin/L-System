using UnityEngine;
using System.Collections;

public class LSystem : MonoBehaviour {
	
	public int depth;
	public string axiom;
	public string varXRewriting;
	public string varFRewriting;
	public float growthLength;
	public int growthAngle;

	private ArrayList currentStack;
	private Vector2 lastBranchPosition;
	private float lastAngle;
	private Stack savedPositions;
	private Stack savedAngles;
	private int currentIndex;

	// Use this for initialization
	void Start () {
		currentIndex = 0;
		savedPositions = new Stack();
		savedAngles = new Stack();
		lastBranchPosition = transform.position;
		lastAngle = 0;
		currentStack = new ArrayList();
		for (int i = 0; i < axiom.Length; i++) {
			currentStack.Add(axiom[i]);
		}
		// Apply rewriting rules
		ArrayList nextStack;
		for (int i = 0; i < depth; i++) {
			nextStack = new ArrayList();
			for (int j = 0; j < currentStack.Count; j++) {
				string symbol = currentStack[j].ToString();
				switch (symbol) {
				case "F":
					for (int n = 0; n < varFRewriting.Length; n++) {
						nextStack.Add(varFRewriting[n]);
					}
					break;
				case "X":
					for (int n = 0; n < varXRewriting.Length; n++) {
						nextStack.Add(varXRewriting[n]);
					}
					break;
				case "+":
					nextStack.Add("+");
					break;
				case "-":
					nextStack.Add("-");
					break;
				case "[":
					nextStack.Add("[");
					break;
				case "]":
					nextStack.Add("]");
					break;
				}
			}
			currentStack = nextStack;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Grow one branch on click hold
		if (Input.GetMouseButton(0) && currentIndex < currentStack.Count) {
			Vector2 position = new Vector2();
			string symbol = currentStack[currentIndex].ToString();
			currentIndex++;
			switch (symbol) {
			case "F":
				// Moving
				position.x = lastBranchPosition.x + growthLength * Mathf.Sin(lastAngle * (Mathf.PI / 180));
				position.y = lastBranchPosition.y + growthLength * Mathf.Cos(lastAngle * (Mathf.PI / 180));
				// Drawing
                Debug.DrawLine(lastBranchPosition, position, Color.black, 60);
				// Current branch is now the previous one
				lastBranchPosition = position;
				break;
			case "+":
				lastAngle += growthAngle;
				break;
			case "-":
				lastAngle -= growthAngle;
				break;
			case "[":
				// Save angles and positions
				savedPositions.Push(lastBranchPosition);
				savedAngles.Push(lastAngle);
				break;
			case "]":
				lastBranchPosition = (Vector2) savedPositions.Pop();
				lastAngle = (float) savedAngles.Pop();
				break;
			}
		}
	}
}
