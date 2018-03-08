using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine {

	private Color color;

	List<KeyValuePair<GridSquare, GridSquare.GridDirection>> linePoints = new List<KeyValuePair<GridSquare, GridSquare.GridDirection>>();

	public Color GetColor() {
		return color;
	}

	public void Add(GridSquare square, GridSquare.GridDirection dir) {
		square.line[(int)dir] = this;
		linePoints.Add(new KeyValuePair<GridSquare, GridSquare.GridDirection>(square, dir));
		PingLine();
	}

	public void Remove(GridSquare square, GridSquare.GridDirection dir) {
		KeyValuePair<GridSquare, GridSquare.GridDirection> found = linePoints.Find(x => (x.Key == square && x.Value == dir));
		linePoints.Remove(found);
		PingLine();
	}

	public void PingLine() {
		UpdateLineColor();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			entry.Key.sprites.lines[(int)entry.Value].color = color;
			if (entry.Key.component != null)
				entry.Key.component.CheckOutput();
		}
	}

	private void UpdateLineColor() {
		List<GridSquare> found = new List<GridSquare>();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			if (entry.Key.component != null) {
				if (entry.Key.socketState[(int)entry.Value] == GridSquare.SocketState.Output) {
					found.Add(entry.Key);
				}
			}
		}
		Debug.Log("linePoints " + linePoints.Count);
		if (found.Count == 1) {
			color = found[0].component.GetOutput().color;
		}
		else {
			color = Color.white;
		}
	}

	//Takes the direction given and follows a line until it hits a component
	public KeyValuePair<GridSquare, GridSquare.GridDirection>[] FindDataComponents(List<GridSquare> ignoreList = null) {
		List<KeyValuePair<GridSquare, GridSquare.GridDirection>> found = new List<KeyValuePair<GridSquare, GridSquare.GridDirection>>();
		foreach (KeyValuePair<GridSquare, GridSquare.GridDirection> entry in linePoints) {
			if (ignoreList != null && ignoreList.Contains(entry.Key))
				continue;
			if (entry.Key.component != null) {
				found.Add(entry);
			}
		}

		return found.ToArray();
	}
}
